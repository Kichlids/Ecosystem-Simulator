using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Action { Explore, FindWater, FindFood, Run };
public enum ConsumptionType { Food, Water };
public enum LivingEntityType { Grain, Chicken, Fox };

[System.Serializable]
public class Animal : MonoBehaviour
{
    public Action action;

    // Movement variables
    [SerializeField]
    protected Coord myLocation;
    [SerializeField]
    protected bool isMoving = false;
    protected List<Coord> path;
    protected int pathIndex = 0;
    protected Coord waypointCoord;
    protected float moveTime = 0;
    [SerializeField]
    protected int visionRadius;
    [SerializeField]
    protected int exploreRadius;

    // Behavior variables
    [SerializeField]
    protected float health;
    [SerializeField]
    protected float hungerMeter;
    [SerializeField]
    public float thirstMeter;
    [SerializeField]
    protected float walkSpeed;


    public void Init(Coord location) {
        this.myLocation = location;
    }

    protected Coord FindRandomCoord(Coord src, int visionRadius) {

        List<Coord> validCoords = Navigation.FindVisibleTiles(src, visionRadius);

        if (validCoords.Count <= 0) {
            return null;
        }

        int random = Random.Range(0, validCoords.Count);
        return validCoords[random];
    }

    protected bool IsAdjacentToWaterTile(Coord src) {

        Coord left = new Coord(src.x - 1, src.y);
        Coord right = new Coord(src.x + 1, src.y);
        Coord top = new Coord(src.x, src.y + 1);
        Coord down = new Coord(src.x, src.y - 1);

        Coord[] list = new Coord[] { left, right, top, down };

        for (int i = 0; i < list.Length; i++) {
            if (Navigation.IsValid(list[i])) {
                if (!Navigation.IsNotWaterTile(list[i])) {
                    return true;
                }
            }
        }

        return false;
    }

    protected Coord FindWaterCoord(Coord src, int visionRadius) {

        List<Coord> validTiles = Navigation.FindVisibleTiles(src, visionRadius);

        if (validTiles.Count <= 0) {
            return null;
        }

        Coord minCoord = null;
        int minDist = int.MaxValue;

        foreach(Coord coord in validTiles) {

            if (IsAdjacentToWaterTile(coord)) {
                int dist = Navigation.Distance(src, coord);
                if (dist < minDist) {
                    minCoord = coord;
                    minDist = dist;
                }
            }
        }

        return minCoord;

    }

    public void Consume(ConsumptionType type) {

        if (type == ConsumptionType.Food) {
            hungerMeter += WorldInfo._instance.grainConsumptionValue;
            if (hungerMeter > WorldInfo._instance.chickenMaxTimeUntilDeathHunger) {
                hungerMeter = WorldInfo._instance.chickenMaxTimeUntilDeathHunger;
            }
        }
        else if (type == ConsumptionType.Water) {
            thirstMeter += WorldInfo._instance.waterConsumptionValue;
            if (thirstMeter > WorldInfo._instance.chickenMaxTimeUntilDeathThirst) {
                thirstMeter = WorldInfo._instance.chickenMaxTimeUntilDeathThirst;
            }
        }
    }
}
