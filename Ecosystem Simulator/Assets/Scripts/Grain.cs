using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grain : MonoBehaviour
{
    void Start()
    {
        transform.parent.GetComponent<Tile>().hasGrain = true;
    }

    private void OnDestroy() {
        transform.parent.GetComponent<Tile>().hasGrain = false;
    }
}
