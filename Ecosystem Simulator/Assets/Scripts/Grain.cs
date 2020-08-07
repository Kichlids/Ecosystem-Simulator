using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grain : MonoBehaviour
{
    private void Start() {
        transform.parent.GetComponent<Tile>().grainCount++;
    }

    private void OnDestroy() {
        transform.parent.GetComponent<Tile>().grainCount--;
    }
}
