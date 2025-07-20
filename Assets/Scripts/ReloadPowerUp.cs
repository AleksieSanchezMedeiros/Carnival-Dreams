using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//inherits from Item class
//this class represents a power-up that gives no cooldown the player's weapon in the game
public class ReloadPowerUp : Item
{
    public float noReloadDuration = 7f;
    private bool isActive = false;

    Gun gun;

    void Start()
    {
        //only need to find the gun once
        gun = FindFirstObjectByType<Gun>();
    }

    //logic for when the obstacle is hit
    //IMPLEMENT THIS
    public override void OnHit()
    {
        if (!isActive)
        {
            isActive = true;
            StartCoroutine(noReloadCoroutine());
        }
    }

    private IEnumerator noReloadCoroutine()
    {
        if (gun != null)
        {
            float originalReloadTime = gun.reloadTime;
            float originalFireRate = gun.fireRate;
            gun.reloadTime = 0f;
            gun.fireRate = 0.1f; // Disable fire rate to max firing speed
            gameUI.DisplayPowerUp("Inf Ammo!"); // Update the UI to indicate reloading is disabled
            StartCoroutine(gameUI.timerCoroutine(noReloadDuration));
            yield return new WaitForSeconds(noReloadDuration);
            gun.reloadTime = originalReloadTime;
            gun.fireRate = originalFireRate; // Restore the original fire rate
            gameUI.UpdateTimer(0f); // Clear the timer text
        }
        Destroy(gameObject);
    }
}