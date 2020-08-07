using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WorldInfo : MonoBehaviour {
    [Header("Tiles")]
    public float tileSizeMultiplier = 10f;
    public GameObject waterTilePrefab;
    [Range(0, 1)]
    public float waterThreshold;
    [Space]
    public GameObject sandTilePrefab;
    [Range(0, 1)]
    public float sandThreshold;
    [Space]
    public GameObject landTilePrefab;
    [Range(0, 1)]
    public float landThreshold;
    [Space]
    public GameObject hillTilePrefab;
    [Range(0, 1)]
    public float hillThreshold;

    [Header("Grain Constants")]
    public GameObject grainPrefab;
    [Range(0, 1)]
    public float grainInitSpawnChance = 0.1f;
    public float grainSpawnTimeMin = 60f;
    public float grainSpawnTimeMax = 300f;
    public float grainConsumptionValue = 40f;

    [Header("Water Constants")]
    public float waterConsumptionValue = 40f;

    [Header("Chicken Constants")]
    public GameObject chickenPrefab;
    public float chickenMaxHealth = 100f;
    public float chickenMaxTimeUntilDeathHunger = 100f;
    public float chickenHungerTriggerThreshold = 40f;
    public float chickenMaxTimeUntilDeathThirst = 100f;
    public float chickenThirstTriggerThreshold = 40f;
    public float chickenWalkSpeed = 1f;
    public int chickenVisionRadius = 8;

    [Header("Fox Constants")]
    public GameObject foxPrefab;
    public float foxMaxHealth = 100f;
    public float foxMaxTimeUntilDeathHunger = 100f;
    public float foxHungerTriggerThreshold = 40f;
    public float foxMaxTimeUntilDeathThirst = 100f;
    public float foxThirstTriggerThreshold = 40f;
    public float foxWalkSpeed = 4f;
    public int foxVisionRadius = 4;





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
