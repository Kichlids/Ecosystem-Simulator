using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            SpawnAnimal(WorldInfo._instance.chickenPrefab, new Coord(0, 0));
        }
    }


    private void SpawnAnimal(GameObject prefab, Coord location) {

        Vector3 position = WorldGenerator._instance.activeTiles[location.x, location.y].transform.position + new Vector3(0, 5, 0);

        GameObject animal = Instantiate(prefab, position, Quaternion.identity, transform);
        animal.GetComponent<Animal>().location = location;
    }
}
