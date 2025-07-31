using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//inherits from Item class
//this class represents a power-up that gives no cooldown the player's weapon in the game
public class ReloadPowerUp : Item
{
    public float buffDuration = 8f;

    GameUI gui;

    void Start()
    {
        //only need to find the gun once
        gui = FindFirstObjectByType<GameUI>();
    }

    //logic for when the obstacle is hit
    //IMPLEMENT THIS
    public override void OnHit()
    {
        gui.ReloadPowerUp(buffDuration); // Call the method to activate the reload power-up
        Destroy(gameObject); // Destroy the power-up object after activation
    }
}