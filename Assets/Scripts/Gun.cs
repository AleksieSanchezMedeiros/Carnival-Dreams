using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] float cameraLookDistance = 60f; // Distance from the camera to the target point, basically adjusts look angle for the gun
    [SerializeField] int ammoMax = 6; // ammo count for the gun, 6 like a revolver as default
    [SerializeField] public float reloadTime = 2f; // time it takes to reload the gun
    [SerializeField] public float fireRate = 0.5f; // time between shots


    public GameObject bullet; // bullet prefab
    public float bulletSpeed = 20f; // speed of the bullet, mostly for visuals since it is hitscan
    public float bulletDeathHitTime = 0.1f; // time before the bullet is destroyed after hitting an item

    public int currentAmmoCount;

    Camera mainCamera;
    
    [SerializeField] AudioClip shootingSound;
    [SerializeField] AudioSource audioSource;

    bool canFire; // Flag to check if the gun can fire
    public bool reloading = false; // Flag to check if the gun is currently reloading

    [SerializeField] GameUI gameUI; // Reference to the GameUI script to update ammo

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        currentAmmoCount = ammoMax; // Initialize ammo count
        canFire = true; // Gun can fire at the start
    }

    // Update is called once per frame
    void Update()
    {

        if(Time.timeScale == 0) return; // If the game is paused, do not update the gun

        if (!reloading && !gameUI.isReloadActive)
            gameUI.UpdateAmmo(currentAmmoCount.ToString()); // Update ammo UI
        else if (gameUI.isReloadActive)
            gameUI.UpdateAmmo("∞"); // Update ammo UI to max if reload power-up is active
        else
            gameUI.DisplayReload(); // Display reloading message if gun cannot fire

        //3d position of the mouse in the world
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);


        Vector3 targetPosition = ray.GetPoint(cameraLookDistance); // Adjust the distance as needed above
        transform.LookAt(targetPosition); // Rotate the gun to look at the target position

        if (currentAmmoCount < 1 && !reloading) // If ammo is less than 1 and not reloading, force reload
        {
            currentAmmoCount = 0; // Prevent negative ammo count
            Debug.Log("Out of ammo!");
            Reload(); // Force reload if out of ammo
            return; // Exit if no ammo left
        }

        // Check for fire input
        if (Input.GetMouseButtonDown(0))
        {
            audioSource.PlayOneShot(shootingSound);
            if (canFire && (currentAmmoCount > 0 || gameUI.isReloadActive)) // Only fire if canFire is true and ammo is available
            {

                if (!gameUI.isReloadActive) currentAmmoCount--; //If reload power-up is not active, decrease ammo count
                else currentAmmoCount = ammoMax;

                // Instantiate bullet at the gun's position and rotation
                GameObject firedBullet = Instantiate(bullet, transform.position, bullet.transform.rotation);

                // Set the bullet's speed as it leaves the gun
                firedBullet.GetComponent<Rigidbody>().linearVelocity = transform.forward * bulletSpeed; // Adjust speed as needed

                //fireRateDelay and also deletes previous bullet
                StartCoroutine(FireRateDelay(firedBullet));

                // Check if the bullet hits an item
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    // Check if the hit object is an Item
                    Item item = hit.collider.GetComponent<Item>();
                    if (item != null)
                    {
                        Debug.Log("Hit item: " + item.name);
                        // Handle the item interaction here
                        item.OnHit(); // something will happen depending on the item type
                        StartCoroutine(BulletDeathAfterHit(firedBullet)); // Destroy the bullet after hitting an item
                    }
                }
            }

        }

        // Check for reload input
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload(); // Reload when R is pressed
        }

    }

    // Method to reload the gun
    void Reload()
    {
        if (currentAmmoCount < ammoMax) // Only reload if not already full
        {
            StartCoroutine(ReloadCoroutine());
        }
    }

    IEnumerator ReloadCoroutine()
    {
        canFire = false; // Disable firing while reloading
        reloading = true; // Set reloading flag to true
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(reloadTime);
        currentAmmoCount = ammoMax; // Reset ammo count after reloading
        canFire = true; // Re-enable firing after reloading
        reloading = false; // Reset reloading flag
        Debug.Log("Reloaded!");
    }

    IEnumerator FireRateDelay(GameObject firedBullet)
    {
        canFire = false; // Disable firing while waiting for fire rate
        yield return new WaitForSeconds(fireRate); // Wait for the fire rate delay
        Destroy(firedBullet); // Destroy the bullet after the delay
        canFire = true; // Re-enable firing after the delay
    }

    IEnumerator BulletDeathAfterHit(GameObject firedBullet)
    {
        yield return new WaitForSeconds(bulletDeathHitTime); // Wait for the fire rate delay
        Destroy(firedBullet); // Destroy the bullet after the delay
    }

    public void setDifficultyVariables(int ammoMax, float reloadTime, float fireRate)
    {
        this.ammoMax = ammoMax;
        this.reloadTime = reloadTime;
        this.fireRate = fireRate;
    }
}
