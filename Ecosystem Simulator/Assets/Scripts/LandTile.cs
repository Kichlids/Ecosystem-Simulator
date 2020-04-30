using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandTile : Tile {

    public GameObject grain;

    [SerializeField]
    private float time;
    [SerializeField]
    private float nextGrainSpawnTime;

    private void Start() {

        hasGrain = false;
        time = 0;

        nextGrainSpawnTime = Random.Range(WorldInfo._instance.grainSpawnTimeMin, WorldInfo._instance.grainSpawnTimeMax);
    }

    private void Update() {

        if (!hasGrain) {

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
        if (!hasGrain) {
            grain = Instantiate(WorldInfo._instance.grainPrefab, transform.position + new Vector3(0, 5.5f, 0), Quaternion.identity);
            grain.transform.parent = transform;
           // hasGrain = true;

            WorldGenerator._instance.grainCount++;
        }
    }

    public void PickGrain() {
        if (hasGrain) {
            Destroy(grain.gameObject);
            hasGrain = false;

            WorldGenerator._instance.grainCount--;
        }
    }
}
