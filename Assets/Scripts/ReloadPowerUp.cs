using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//inherits from Item class
//this class represents a power-up that gives no cooldown the player's weapon in the game
public class ReloadPowerUp : Item
{
    public float noReloadDuration = 7f;
    private bool isActive = false;

    //logic for when the obstacle is hit
    //IMPLEMENT THIS
    public override void OnHit()
    {
        if (!isActive) {
            isActive = true;
            StartCoroutine(noReloadCoroutine());
        }
    }

    private IEnumerator noReloadCoroutine()
    {
        Gun gun = FindObjectOfType<Gun>();

        if (gun != null) {
            float originalReloadTime = gun.reloadTime;
            gun.reloadTime = 0f;
            yield return new WaitForSeconds(noReloadDuration);
            gun.reloadTime = originalReloadTime;
        }
        Destroy(gameObject);
    }
}