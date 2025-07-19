using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//base class for all items that will be spawned in the game
//this class can be extended for specific item types like enemies, astronauts, etc.
//inherit from here
public abstract class Item : MonoBehaviour
{
    public float speed = 3f; //speed of the item, can be set in the inspector or dynamically

    public float killLength = 150f; //would be better to link it with the GamePanel, but for now this is fine

    private Vector3 startPosition; //initial position of the item, used to determine when to destroy it
    private bool isLeftSpawn;

    private bool spawnOrientation; // true for left, false for right, used to determine which side the item spawns on

    [SerializeField] GameUI gameUI; // Reference to the GameUI script to update score and ammo
    void Awake()
    {
        // Set the initial position of the item
        startPosition = transform.position;

    }

    //movement is handled here instead of in GamePanel
    //this allows each item to move independently
    void Update()
    {
        // Move the item depending on left or right depending on spawn point at the specified speed
        //currently checking every frame, could be optimized if needed
        if (isLeftSpawn)
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        else
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }

        // If the item has moved beyond the kill length, destroy it
        if (Vector3.Distance(startPosition, transform.position) > killLength)
        {
            Destroy(gameObject);
        }
    }

    //to be used by the GamePanel to set the speed of the item
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void SetSpawnOrientation(bool newIsLeftSpawn)
    {
        isLeftSpawn = newIsLeftSpawn;
    }

    public abstract void OnHit(); // abstract method to be implemented by derived classes, ensure that each item can handle being hit
}
