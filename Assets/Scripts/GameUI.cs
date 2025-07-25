using UnityEngine;
using System.Collections;
using TMPro;

public class GameUI : MonoBehaviour
{
    private int score = 0; // Player's score
    public TextMeshProUGUI scoreText; // Reference to the UI text component

    public TextMeshProUGUI ammoText; // Reference to the ammo text component

    public TextMeshProUGUI powerUpText; // Reference to the power up text component

    public TextMeshProUGUI timerText; // Reference to the timer text component

    public TextMeshProUGUI gameTimerText; // Reference to the time left component

    public float amountOfPopUpTime = 3f; // Time to display power-up text

    public float gameDuration = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        score = 0; // Initialize score
        scoreText.text = "Score: " + score; // Update the UI text
        powerUpText.text = ""; // Clear power-up text at the start
        ammoText.text = "Ammo: 0"; // Initialize ammo text
        StartCoroutine(GameTimer());
    }

    public void UpdateScore(int points)
    {
        if (InvinciblePowerUp.isScoreBlocked && points < 0) {
            return;
        }

        score += points; // Update the score
        if (score < 0) {
            score = 0;
        }

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

    public void DisplayPowerUp(string powerUpName)
    {
        if (!gameObject.activeInHierarchy)
        {
            Debug.LogWarning("Tried to display power-up, but GameUI (or its GameObject) is inactive.");
            return;
        }

        powerUpText.text = powerUpName; // Update the UI text to show the power-up
        StartCoroutine(PowerUpDissapearDelay());
    }

    private IEnumerator PowerUpDissapearDelay()
    {
        yield return new WaitForSeconds(amountOfPopUpTime); // Wait for 3 seconds
        powerUpText.text = ""; // Clear the power-up text after the delay
    }

    public void UpdateTimer(float timeRemaining)
    {
        if (timeRemaining <= 0)
        {
            timerText.text = ""; // Clear the timer text if time is 0
            return;
        }

        timerText.text = "" + Mathf.Max(0, Mathf.RoundToInt(timeRemaining)); // Update the timer text
    }
    public IEnumerator timerCoroutine(float time)
    {
        float timeRemaining = time;
        while (timeRemaining > 0)
        {
            UpdateTimer(timeRemaining); // update first
            yield return new WaitForSeconds(1f); // then wait
            timeRemaining--;
        }
        UpdateTimer(0); // ensure timer is cleared at the end
    }

    private IEnumerator GameTimer()
    {
        float timeRemaining = gameDuration;
        while (timeRemaining > 0f) {
            gameTimerText.text = "Time:" + Mathf.CeilToInt(timeRemaining);
            yield return new WaitForSeconds(1f);
            timeRemaining--;
        }

        gameTimerText.text = "Game Over!";
        EndGame();
    }

    private void EndGame()
    {
        Debug.Log("Game Over!");
        Time.timeScale = 0f;

    }
}
