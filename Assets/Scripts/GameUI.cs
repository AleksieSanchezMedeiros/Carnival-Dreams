using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    private int score = 0; // Player's score
    public TextMeshProUGUI scoreText; // Reference to the UI text component

    public TextMeshProUGUI ammoText; // Reference to the ammo text component

    public TextMeshProUGUI powerUpText; // Reference to the power up text component

    public TextMeshProUGUI timerText; // Reference to the timer text component

    public TextMeshProUGUI gameTimerText; // Reference to the time left component

    public TextMeshProUGUI timerTextReload; // Reference to the game over text component

    public TextMeshProUGUI gameOverText; // Reference to the game over text component

    public float amountOfPopUpTime = 3f; // Time to display power-up text

    public float gameDuration = 5f;

    public GameObject blur; // Reference to the game over panel
    public GameObject gameOverButton; // Reference to the game over button
    public GameObject replayButton; // Reference to the replay button

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    Gun gun;

    public bool gameEnded = false; // Flag to check if the game has ended

    public bool isReloadActive = false; // Flag to check if the reload power-up is active

    bool isInvincibleActive = false; // Flag to check if the invincibility power-up is active
    void Start()
    {
        Time.timeScale = 1f; // Ensure the game is running at normal speed
        isReloadActive = false; // Initialize the reload power-up flag
        gameEnded = false; // Initialize the game ended flag
        isInvincibleActive = false; // Initialize the invincibility power-up flag
        blur.SetActive(false); // Hide the blur effect at the start
        gameOverButton.SetActive(false); // Hide the game over button at the start
        replayButton.SetActive(false); // Hide the replay button at the start
        score = 0; // Initialize score
        gameOverText.text = ""; // Clear game over text at the start
        scoreText.text = "Score: " + score; // Update the UI text
        powerUpText.text = ""; // Clear power-up text at the start
        ammoText.text = "Ammo: 0"; // Initialize ammo text
        gun = FindFirstObjectByType<Gun>();
        StartCoroutine(GameTimer());
    }

    public void UpdateScore(int points)
    {
        if (isInvincibleActive && points < 0)
        {
            return;
        }

        score += points; // Update the score
        if (score < 0)
        {
            score = 0;
        }

        scoreText.text = "Score: " + score; // Update the UI text
    }

    public void UpdateAmmo(string ammo)
    {
        if (gameEnded)
        {
            ammoText.text = ""; // Clear ammo text if the game has ended
            return; // Exit if the game has ended
        }
        ammoText.text = "Ammo: " + ammo; // Update the UI text
    }

    public void DisplayReload()
    {
        ammoText.text = "Reloading..."; // Update the UI text to indicate reloading
    }

    public void ReloadPowerUp(float duration)
    {
        if (isReloadActive)
        {
            Debug.LogWarning("Reload power-up is already active.");
            return; // Prevent multiple activations
        }
        StartCoroutine(ReloadPowerUpCoroutine(duration)); // Start the coroutine to handle the reload power-up
    }

    private IEnumerator ReloadPowerUpCoroutine(float duration)
    {
        isReloadActive = true; // Set the flag to indicate the power-up is active
        DisplayPowerUp("Inf Ammo!"); // Display the power-up message
        float originalReloadTime = gun.reloadTime; // Store the original reload time
        float originalFireRate = gun.fireRate; // Store the original fire rate
        gun.reloadTime = 0f;
        gun.fireRate = 0.1f;
        StartCoroutine(timerCoroutineReload(duration));
        yield return new WaitForSeconds(duration); // Wait for the duration of the power-up
        DisplayPowerUp("Reload Over!"); // Display the end of the power-up message
        isReloadActive = false; // Reset the flag
        gun.reloadTime = originalReloadTime;
        gun.fireRate = originalFireRate;
    }

    public void InvinciblePowerUp(float duration)
    {
        if (isInvincibleActive)
        {
            Debug.LogWarning("Invincibility power-up is already active.");
            return; // Prevent multiple activations
        }

        StartCoroutine(InvinciblePowerUpCoroutine(duration)); // Start the coroutine to handle the invincibility power-up
    }

    private IEnumerator InvinciblePowerUpCoroutine(float duration)
    {
        isInvincibleActive = true; // Set the flag to indicate the power-up is active
        DisplayPowerUp("Score Invincible!"); // Display the power-up message
        StartCoroutine(timerCoroutine(duration));
        yield return new WaitForSeconds(duration); // Wait for the duration of the power-up
        DisplayPowerUp("Invincibility Over!"); // Display the end of the power-up message
        isInvincibleActive = false; // Reset the flag
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

    private IEnumerator GameTimer()
    {
        float timeRemaining = gameDuration;
        while (timeRemaining > 0f)
        {
            gameTimerText.text = "Time: " + Mathf.CeilToInt(timeRemaining);
            yield return new WaitForSeconds(1f);
            timeRemaining--;
        }

        gameTimerText.text = "Time: 0";
        EndGame();
    }

    private void EndGame()
    {
        gameEnded = true; // Set the game ended flag to true
        blur.SetActive(true); // Show the blur effect
        gameOverButton.SetActive(true); // Show the game over button
        replayButton.SetActive(true); // Show the replay button
    
        powerUpText.text = ""; // Clear power-up text
        timerText.text = ""; // Clear the timer text
        timerTextReload.text = ""; // Clear the reload timer text
        scoreText.text = ""; // Clear the score text
        gameOverText.text = "Game Over!\nFinal Score: " + score; // Display game over message
        Time.timeScale = 0f;
    }

    public void ReplayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene to restart the game
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game..."); // Log the quit action
        Application.Quit(); // Quit the application
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

    
    public void UpdateTimerReload(float timeRemaining)
    {
        if (timeRemaining <= 0)
        {
            timerTextReload.text = ""; // Clear the timer text if time is 0
            return;
        }

        timerTextReload.text = "" + Mathf.Max(0, Mathf.RoundToInt(timeRemaining)); // Update the timer text
    }
    public IEnumerator timerCoroutineReload(float time)
    {
        float timeRemaining = time;
        while (timeRemaining > 0)
        {
            UpdateTimerReload(timeRemaining); // update first
            yield return new WaitForSeconds(1f); // then wait
            timeRemaining--;
        }
        UpdateTimerReload(0); // ensure timer is cleared at the end
    }
}
