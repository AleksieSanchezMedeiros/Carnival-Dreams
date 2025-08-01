using System.Collections.Generic;
using UnityEngine;

public class DifficultyPicker : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] DifficultySetting[] settings;
    [SerializeField] Gun playerGun;
    [SerializeField]GamePanel gamePanel;

    [SerializeField] GameObject difficultyScreen;
    [SerializeField] GameObject tutorialScreen;

    [SerializeField] GameObject restartButton;

    public void Start()
    {
        Time.timeScale = 0; // Pause the game at the start
    }

    public void ShowDifficultyScreen()
    {
        difficultyScreen.SetActive(true);
    }

    public void HideDifficultyScreen()
    {
        difficultyScreen.SetActive(false);
    }

    public void ShowTutorialScreen()
    {
        Time.timeScale = 0; // Pause the game
        tutorialScreen.SetActive(true);
        restartButton.SetActive(true); // Show the restart button
    }

    public void HideTutorialScreen()
    {
        Time.timeScale = 1; // Resume the game
        tutorialScreen.SetActive(false);
        restartButton.SetActive(false); // Hide the restart button
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            if (!tutorialScreen.activeSelf)
            {
                ShowTutorialScreen();
            }
            else
            {
                HideTutorialScreen();
            }
        }

    }

    public void setDificulty(int difficultyPicked)
    {
        playerGun.setDifficultyVariables(settings[difficultyPicked].ammoMax, settings[difficultyPicked].reloadTime, settings[difficultyPicked].fireRate);
        for (int i = 0; i < settings[difficultyPicked].spawnChances.Length; i++)
        {
            gamePanel.itemsOnRail[i].spawnChance = settings[difficultyPicked].spawnChances[i];
        }
        Debug.Log("Difficulty switched to: " + settings[difficultyPicked].name);
        HideDifficultyScreen();
        ShowTutorialScreen();
    }
}
