using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DifficultySetting", menuName = "Scriptable Objects/DifficultySetting")]
public class DifficultySetting : ScriptableObject
{
    public int ammoMax;
    public float reloadTime;
    public float fireRate;
    public float targetSlidingSpeed;
    public float timeBetweenSpawn;
    [Range(0,1)] public float[] spawnChances;
}
