using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//inherits from Item class
//this class represents a power-up that grants score invincibility in the game
public class InvinciblePowerUp : Item
{
    public float invincibilityDuration = 7f;
    private bool isInvincible = false;
    public static bool isScoreBlocked = false;

    void Start()
    {
        gameUI = FindFirstObjectByType<GameUI>();
    }
    
    //logic for when the obstacle is hit
    //IMPLEMENT THIS
    public override void OnHit()
    {
        if (!isInvincible) {
            isInvincible = true;

            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer != null) meshRenderer.enabled = false;

            Collider collider = GetComponent<Collider>();
            if (collider != null) collider.enabled = false;

            StartCoroutine(InvincibilityCoroutine());
        }
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isScoreBlocked = true;

        if (gameUI != null) {
            gameUI.DisplayPowerUp("Score Invincibility!");
            StartCoroutine(gameUI.timerCoroutine(invincibilityDuration));
        }

        yield return new WaitForSeconds(invincibilityDuration);
        isScoreBlocked = false;

        if (gameUI != null) {
            gameUI.UpdateTimer(0f);
        }

        Destroy(gameObject);
    }
}
