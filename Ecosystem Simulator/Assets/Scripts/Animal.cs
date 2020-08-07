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

    [Header("Movement")]
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

    [Header("Behavior")]
    [SerializeField]
    protected float health;
    [SerializeField]
    protected float hungerMeter;
    [SerializeField]
    public float thirstMeter;
    [SerializeField]
    protected float walkSpeed;
    [SerializeField]
    protected bool isDead;

    protected WorldGenerator worldGenerator;
    protected WorldInfo worldInfo;

    protected void Start() {
        worldInfo = WorldInfo._instance;
        worldGenerator = WorldGenerator._instance;

        action = Action.Explore;
    }

    public void Init(Coord location) {
        this.myLocation = location;
        isDead = false;
    }

    // Find the closest tile that contains food
    protected Coord FindFoodCoord(Coord src, LivingEntityType type, int visionRadius) {
        return Navigation.FindItem(src, type, visionRadius);
    }

    // Find the closest tile adjacent to water tile
    protected Coord FindWaterCoord(Coord src, int visionRadius) {

        List<Coord> validTiles = Navigation.FindVisibleTiles(src, visionRadius);

        if (validTiles.Count <= 0) {
            return null;
        }

        Coord minCoord = null;
        int minDist = int.MaxValue;

        foreach (Coord coord in validTiles) {

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

    // Find a random coordinate within vision radius
    protected Coord FindRandomCoord(Coord src, int visionRadius) {

        List<Coord> validCoords = Navigation.FindVisibleTiles(src, visionRadius);

        if (validCoords.Count <= 0) {
            return null;
        }

        int random = Random.Range(0, validCoords.Count);
        return validCoords[random];
    }

    // Check if a coordinate is adjacent to water tile
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

    // Decrement hunger/health
    public void ExpendResources() {

        float hungerDecrement = Time.deltaTime;
        float thirstDecrement = Time.deltaTime;

        if (action == Action.Run) {
            hungerMeter *= 2;
            thirstDecrement *= 2;
        }

        if (hungerMeter < 0) {
            hungerMeter = 0;
        }
        else {
            hungerMeter -= hungerDecrement;
        }

        if (thirstMeter < 0) {
            thirstMeter = 0;
        }
        else {
            thirstMeter -= thirstDecrement;
        }
    }

    #region Behavioral Related Functions

    protected void DetermineNextState(float hungerThreshold, float thirstThreshold) {

        if (hungerMeter <= hungerThreshold) {
            action = Action.FindFood;
        }
        else if (thirstMeter <= thirstThreshold) {
            action = Action.FindWater;
        }
        else {
            action = Action.Explore;
        }
    }

    protected void Act(LivingEntityType type) {

        if (action == Action.FindFood) {
            if (!isMoving) {
                MoveCommand(FindFoodCoord(myLocation, LivingEntityType.Grain, visionRadius));
            }
        }
        else if (action == Action.FindWater) {
            if (!isMoving) {
                MoveCommand(FindWaterCoord(myLocation, visionRadius));
            }
        }
        else if (action == Action.Explore) {
            if (!isMoving) {
                MoveCommand(FindRandomCoord(myLocation, visionRadius));
            }
        } 
    }

    public void MoveCommand(Coord dest) {

        if (dest != null) {
            path = null;
            pathIndex = 0;
            waypointCoord = null;
            isMoving = false;

            path = Navigation.PathFind(myLocation, dest);

            if (path != null) {
                pathIndex = 0;

                waypointCoord = path[pathIndex];
                isMoving = true;
            }
            else {
                print("path not found");
            }
        }
        else {
            MoveCommand(FindRandomCoord(myLocation, visionRadius));
            print("Target not found. Go random coord");
        }
    }


    protected void EvaluateNextWaypoint() {

        if (pathIndex <= path.Count - 1) {
            waypointCoord = path[pathIndex];
            isMoving = true;
        }
        else {
            path = null;
            pathIndex = 0;
            isMoving = false;
        }
    }

    #endregion


}
