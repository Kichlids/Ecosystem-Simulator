using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandTile : Tile {

    private GameObject grain;

    [SerializeField]
    private float time;
    [SerializeField]
    private float nextGrainSpawnTime;

    private void Start() {

        grain = WorldInfo._instance.grainPrefab;

        grainCount = 0;
        chickenCount = 0;
        foxCount = 0;

        time = 0;

        nextGrainSpawnTime = Random.Range(WorldInfo._instance.grainSpawnTimeMin, WorldInfo._instance.grainSpawnTimeMax);
    }

    private void Update() {

        if (grainCount <= 0) {

            if (time >= nextGrainSpawnTime) {
                SpawnGrain();

                time = 0;
                nextGrainSpawnTime = Random.Range(WorldInfo._instance.grainSpawnTimeMin, WorldInfo._instance.grainSpawnTimeMax);
            }
            else {
                time += Time.deltaTime;
            }
        }
    }

    private void SpawnGrain() {
        if (grainCount <= 0) {
            grain = Instantiate(WorldInfo._instance.grainPrefab, transform.position + new Vector3(0, 5.5f, 0), Quaternion.identity);
            grain.transform.parent = transform;
            //hasGrain = true;
            
            WorldGenerator._instance.grainCount++;
        }
    }

    public void PickGrain() {
        if (grainCount <= 0) {
            Destroy(grain.gameObject);
            //hasGrain = false;

            WorldGenerator._instance.grainCount--;
        }
    }
}
