using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType { Hill, Land, Sand, Water };

public class WorldGenerator : MonoBehaviour {
    [Header("Map Variables")]

    public int mapSize;
    public float noiseScale;
    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;
    public int seed;
    public Vector2 offset;
    public bool autoUpdate;

    [Header("Debug")]

    public GameObject[,] activeTiles;
    public bool[,] walkableTiles;

    [SerializeField]
    private int hillTileCount = 0;
    [SerializeField]
    private int landTileCount = 0;
    [SerializeField]
    private int sandTileCount = 0;
    [SerializeField]
    private int waterTileCount = 0;
    [SerializeField]
    public int grainCount = 0;

    public static WorldGenerator _instance;

    private void Awake() {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    private void Start() {
        GenerateMap();


        print("Done");
    }

    public void GenerateMap() {
        landTileCount = 0;
        sandTileCount = 0;
        waterTileCount = 0;
        grainCount = 0;

        float[,] noiseMap = Noise.GenerateNoiseMap(mapSize, mapSize, seed, noiseScale, octaves, persistance, lacunarity, offset);
        ConstructWorld(noiseMap);
    }

    public void ConstructWorld(float[,] noiseMap) {
        DestroyAllActiveTiles();

        activeTiles = new GameObject[mapSize, mapSize];
        walkableTiles = new bool[mapSize, mapSize];

        for (int y = 0; y < mapSize; y++) {
            for (int x = 0; x < mapSize; x++) {
                Vector3 position = new Vector3(x, 0, y) * 10;

                GameObject prefab = DetermineTileToSpawn(noiseMap[x, y]);

                if (prefab != null) {
                    if (prefab == WorldInfo._instance.waterTilePrefab) {
                        position -= new Vector3(0, 5, 0);
                    }

                    GameObject tile = Instantiate(prefab, position, Quaternion.identity, transform);
                    tile.GetComponent<Tile>().location = new Coord(x, y);
                    activeTiles[x, y] = tile;

                    if(tile.tag == "Water") {
                        walkableTiles[x, y] = false;
                    }
                    else {
                        walkableTiles[x, y] = true;
                    }
                }
            }
        }
    }

    private GameObject DetermineTileToSpawn(float threshold) {
        if (threshold <= WorldInfo._instance.waterThreshold) {
            waterTileCount++;
            return WorldInfo._instance.waterTilePrefab;
        }
        else if (threshold <= WorldInfo._instance.sandThreshold) {
            sandTileCount++;
            return WorldInfo._instance.sandTilePrefab;
        }
        else if (threshold <= WorldInfo._instance.landThreshold) {
            landTileCount++;
            return WorldInfo._instance.landTilePrefab;
        }
        else if (threshold <= WorldInfo._instance.hillThreshold) {
            hillTileCount++;
            return WorldInfo._instance.hillTilePrefab;
        }
        else {
            return null;
        }
    }

    private List<GameObject> GetActiveTiles(string tag) {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag(tag);
        List<GameObject> tileList = new List<GameObject>(tiles);

        return tileList;
    }

    private void DestroyAllActiveTiles() {

        List<GameObject> tiles = new List<GameObject>();

        List<GameObject> waterTiles = GetActiveTiles("Water");
        List<GameObject> sandTiles = GetActiveTiles("Sand");
        List<GameObject> landTiles = GetActiveTiles("Land");
        List<GameObject> hillTiles = GetActiveTiles("Hill");

        tiles.AddRange(waterTiles);
        tiles.AddRange(sandTiles);
        tiles.AddRange(landTiles);
        tiles.AddRange(hillTiles);

        foreach (GameObject tile in tiles)
            DestroyImmediate(tile);

        activeTiles = null;
        walkableTiles = null;
    }

    public void RegisterMovement(Coord src, string tag) {

        if (tag == "Grain") {

        }
        else if (tag == "Chicken") {

        }
        else if (tag == "Fox") {

        }
    }

    private void OnValidate() {
        if (mapSize < 1)
            mapSize = 1;
        if (mapSize < 1)
            mapSize = 1;
        if (lacunarity < 1)
            lacunarity = 1;
        if (octaves < 0)
            octaves = 0;
    }
}

