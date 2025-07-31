using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//inherits from Item class
//this class represents a power-up that grants score invincibility in the game
public class InvinciblePowerUp : Item
{
    public float buffDuration = 8f;

    GameUI gui;

    void Start()
    {
        gui = FindFirstObjectByType<GameUI>();
    }

    //logic for when the obstacle is hit
    //IMPLEMENT THIS
    public override void OnHit()
    {
        gui.InvinciblePowerUp(buffDuration); // Call the method to activate the invincibility power-up
        Destroy(gameObject); // Destroy the power-up object after activation
    }
}
