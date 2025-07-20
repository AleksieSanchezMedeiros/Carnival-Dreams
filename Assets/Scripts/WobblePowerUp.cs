using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//inherits from Item class
//this class represents a power-up that causes the player's weapon to wobble in the game??
public class WobblePowerUp : Item
{
    public float wobbleReductionDuration = 5f;
    public float reducedLookDistance = 10f;
    public bool isActive = false;

    //logic for when the obstacle is hit
    //IMPLEMENT THIS
    public override void OnHit()
    {
        if (!isActive) {
            isActive = true;
            StartCoroutine(WobbleCoroutine());
        }
    }

    private IEnumerator WobbleCoroutine()
    {
        Gun gun = FindObjectOfType<Gun>();

        if (gun != null) {
            yield return new WaitForSeconds(wobbleReductionDuration);
        }
        Destroy(gameObject);
    }
}