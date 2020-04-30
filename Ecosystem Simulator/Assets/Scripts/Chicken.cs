using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : Animal
{
    private void Start() {

        action = Action.Explore;
        health = WorldInfo._instance.chickenMaxHealth;
        hungerMeter = WorldInfo._instance.chickenMaxTimeUntilDeathHunger;
        thirstMeter = WorldInfo._instance.chickenMaxTimeUntilDeathThirst;
        walkSpeed = WorldInfo._instance.chickenWalkSpeed;
        visionRadius = WorldInfo._instance.chickenVisionRadius;
    }

    private void Update() {

        DetermineNextState();

        if (isMoving) {
            MoveAction();
        }
    }

    private void DetermineNextState() {

        if (hungerMeter <= WorldInfo._instance.chickenHungerThreshold) {
            action = Action.FindFood;
        }
        else if (thirstMeter <= WorldInfo._instance.chickenThirstThreshold) {
            action = Action.FindWater;
        }
        else {
            action = Action.Explore;
        }

        Act();
    }

    public void Act() {
        
        if (action == Action.FindFood) {
            if (!isMoving) {
                MoveCommand(Navigation.FindItem(myLocation, LivingEntityType.Grain, visionRadius));
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

        ExpendResources();
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

    private void MoveAction() {
        
        transform.LookAt(Navigation.CoordToWorldPosition(waypointCoord));

        Vector3 loc = Navigation.CoordToWorldPosition(myLocation);

        float speed = walkSpeed;
        if (hungerMeter <= WorldInfo._instance.chickenMaxTimeUntilDeathHunger * 0.1f || thirstMeter <= WorldInfo._instance.chickenMaxTimeUntilDeathThirst * 0.1f) {
            speed /= 2;
        }
        moveTime = Mathf.Min(1, moveTime + Time.deltaTime * speed);
        transform.position = Vector3.Lerp(loc, Navigation.CoordToWorldPosition(waypointCoord), moveTime);

        if (moveTime >= 1) {
            isMoving = false;
            myLocation = waypointCoord;
            pathIndex++;
            moveTime = 0;

            ExecuteAction();
            EvaluateNextWaypoint();
        }
    }

    public void ExecuteAction() {
        if (action == Action.FindFood) {
            LandTile currTile = WorldGenerator._instance.activeTiles[myLocation.x, myLocation.y].gameObject.GetComponent<LandTile>();
            if (currTile != null && currTile.hasGrain) {
                Consume(ConsumptionType.Food);
                currTile.PickGrain();
                print("Consumed grain");
            }
        }
        else if (action == Action.FindWater) {
            if (IsAdjacentToWaterTile(myLocation)) {
                Consume(ConsumptionType.Water);
                print("Consumed water");
            }
        }
    }

    public void EvaluateNextWaypoint() {

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
}
