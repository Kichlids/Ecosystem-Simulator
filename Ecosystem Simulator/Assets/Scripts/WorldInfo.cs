using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WorldInfo : MonoBehaviour {
    [Header("Tiles")]
    public float tileSizeMultiplier = 10f;
    public GameObject waterTilePrefab;
    [Range(0, 1)]
    public float waterThreshold = 0.3f;
    [Space]
    public GameObject sandTilePrefab;
    [Range(0, 1)]
    public float sandThreshold = 0.4f;
    [Space]
    public GameObject landTilePrefab;
    [Range(0, 1)]
    public float landThreshold = 1f;

    [Header("Item Spawns")]
    public GameObject grainPrefab;
    [Range(0, 1)]
    public float grainInitSpawnChance = 0.1f;
    public float grainSpawnTimeMin = 60f;
    public float grainSpawnTimeMax = 300f;
    public float grainConsumptionValue = 40f;

    [Header("Animal Spawns")]
    public GameObject chickenPrefab;
    public float chickenMaxHealth = 100f;
    public float chickenMaxTimeUntilDeathHunger = 100;
    public float chickenHungerThreshold = 40;
    public float chickenMaxTimeUntilDeathThirst = 100;
    public float chickenThirstThreshold = 40;
    public float chickenWalkSpeed = 0f;
    public int chickenVisionRadius = 8;

    public float waterConsumptionValue = 40f;






    public static WorldInfo _instance;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        }
        else {
            _instance = this;
        }
    }
}
