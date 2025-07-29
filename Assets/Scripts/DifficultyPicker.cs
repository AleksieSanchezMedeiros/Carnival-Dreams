using System.Collections.Generic;
using UnityEngine;

public class DifficultyPicker : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] DifficultySetting[] settings;
    [SerializeField] Gun playerGun;
    [SerializeField]GamePanel gamePanel;
    void setDificulty(int difficultyPicked)
    {
        playerGun.setDifficultyVariables(settings[difficultyPicked].ammoMax, settings[difficultyPicked].reloadTime, settings[difficultyPicked].fireRate);
        for (int i = 0; i < gamePanel.itemsOnRail.Length; i++)
        {
            gamePanel.itemsOnRail[i].spawnChance = settings[difficultyPicked].spawnChances[i];
        }
        
    }
}
