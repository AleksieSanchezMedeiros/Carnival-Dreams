using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    private int score = 0; // Player's score
    public TextMeshProUGUI scoreText; // Reference to the UI text component

    public TextMeshProUGUI ammoText; // Reference to the Game Over text component

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        score = 0; // Initialize score
        scoreText.text = "Score: " + score; // Update the UI text
    }

    public void UpdateScore(int points)
    {
        score += points; // Update the score
        scoreText.text = "Score: " + score; // Update the UI text
    }

    public void UpdateAmmo(int ammo)
    {
        ammoText.text = "Ammo: " + ammo; // Update the UI text
    }
    
    public void DisplayReload()
    {
        ammoText.text = "Reloading..."; // Update the UI text to indicate reloading
    }
}
