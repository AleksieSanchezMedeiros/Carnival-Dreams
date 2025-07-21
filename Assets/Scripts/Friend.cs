using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//inherits from Item class
//this class represents an astronaut in the game
public class Friend : Item
{
    // Start is called before the first frame update
    void Start()
    {

    }

    //logic for when the obstacle is hit
    //IMPLEMENT THIS
    public override void OnHit()
    {
        gameUI.UpdateScore(-1); // Update the score by 10 points when hit
        
        // You might also want to play a sound or spawn a particle effect here

        // For now, let's just destroy the enemy object
        Destroy(this.gameObject);
    }
}
