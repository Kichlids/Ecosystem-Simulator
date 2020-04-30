using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    public Coord location;

    public bool hasGrain = false;
    public bool hasChicken = false;
    public bool hasFox = false;
}
