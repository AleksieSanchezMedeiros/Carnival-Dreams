using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//inherits from Item class
//this class represents a power-up that grants score invincibility in the game
public class InvinciblePowerUp : Item
{
    public float invincibilityDuration = 7f;
    private bool isInvincible = false;
    
    //logic for when the obstacle is hit
    //IMPLEMENT THIS
    public override void OnHit()
    {
        if (!isInvincible) {
            isInvincible = true;
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    private IEnumerator InvincibilityCoroutine()
    {
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
        Destroy(gameObject);

    }
}
