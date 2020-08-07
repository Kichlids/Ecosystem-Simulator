using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : Animal
{
    
    private new void Start() {

        base.Start();

        health = worldInfo.chickenMaxHealth;
        hungerMeter = worldInfo.chickenMaxTimeUntilDeathHunger;
        thirstMeter = worldInfo.chickenMaxTimeUntilDeathThirst;
        walkSpeed = worldInfo.chickenWalkSpeed;
        visionRadius = worldInfo.chickenVisionRadius;
    }  

    private void Update() {
        if (isDead) {
            Destroy(gameObject);
            enabled = false;
        }

        DetermineNextState(worldInfo.chickenHungerTriggerThreshold, worldInfo.chickenThirstTriggerThreshold);
        Act(LivingEntityType.Grain);
        ExpendResources();


        if (isMoving) {
            MoveAction(worldInfo.chickenMaxTimeUntilDeathHunger, worldInfo.chickenMaxTimeUntilDeathThirst);
        }
    }

    protected void MoveAction(float maxHungerTime, float maxThirstTime) {

        transform.LookAt(Navigation.CoordToWorldPosition(waypointCoord));

        Vector3 loc = Navigation.CoordToWorldPosition(myLocation);

        float speed = walkSpeed;
        if (hungerMeter <= maxHungerTime * 0.1f || thirstMeter <= maxThirstTime * 0.1f) {
            speed /= 2;
        }
        moveTime = Mathf.Min(1, moveTime + Time.deltaTime * speed);
        transform.position = Vector3.Lerp(loc, Navigation.CoordToWorldPosition(waypointCoord), moveTime);

        // Arrived at destination
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
            LandTile currTile = worldGenerator.activeTiles[myLocation.x, myLocation.y].gameObject.GetComponent<LandTile>();
            if (currTile != null && currTile.grainCount > 0) {
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

    public void Consume(ConsumptionType type) {
        if (type == ConsumptionType.Food) {
            hungerMeter += worldInfo.grainConsumptionValue;
            if (hungerMeter > worldInfo.chickenMaxTimeUntilDeathHunger) {
                hungerMeter = worldInfo.chickenMaxTimeUntilDeathHunger;
            }
        }
        else if (type == ConsumptionType.Water) {
            thirstMeter += worldInfo.waterConsumptionValue;
            if (thirstMeter > worldInfo.chickenMaxTimeUntilDeathThirst) {
                thirstMeter = worldInfo.chickenMaxTimeUntilDeathThirst;
            }
        }
    }
}
