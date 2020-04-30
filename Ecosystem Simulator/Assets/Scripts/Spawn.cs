using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{

    GameObject animal;
    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            SpawnAnimal(WorldInfo._instance.chickenPrefab, new Coord(0, 0));
        }
        if (Input.GetKeyDown(KeyCode.Q)) {
            animal.GetComponent<Chicken>().MoveCommand(new Coord(49, 49));
        }
        if (Input.GetKeyDown(KeyCode.W)) {
            animal.GetComponent<Chicken>().MoveCommand(new Coord(0, 0));
        }

    }


    private void SpawnAnimal(GameObject prefab, Coord location) {

        Vector3 position = WorldGenerator._instance.activeTiles[location.x, location.y].transform.position + new Vector3(0, 5, 0);

        animal = Instantiate(prefab, position, Quaternion.identity, transform);
        animal.GetComponent<Chicken>().Init(new Coord((int)position.x, (int)position.z));

        
    }
}
