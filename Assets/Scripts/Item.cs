using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//base class for all items that will be spawned in the game
//this class can be extended for specific item types like enemies, astronauts, etc.
//inherit from here
public class Item : MonoBehaviour
{
    public float speed = 3f; //speed of the item, can be set in the inspector or dynamically

    public float killLength = 150f;

    private Vector3 startPosition;
    void Start()
    {
        // Set the initial position of the item
        startPosition = transform.position;
    }

    //movement is handled here instead of in GamePanel
    //this allows each item to move independently
    void Update()
    {
        // Move the item downwards at the specified speed
        transform.Translate(Vector3.right * speed * Time.deltaTime);

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
}
