using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox : Animal
{
    private new void Start() {
        base.Start();

        health = worldInfo.foxMaxHealth;
        hungerMeter = worldInfo.foxMaxTimeUntilDeathHunger;
        thirstMeter = worldInfo.foxMaxTimeUntilDeathThirst;
        walkSpeed = worldInfo.foxWalkSpeed;
        visionRadius = worldInfo.foxVisionRadius;
    }

    private void Update() {
        if (isDead) {
            Destroy(gameObject);
            enabled = false;
        }

        DetermineNextState(worldInfo.foxHungerTriggerThreshold, worldInfo.foxThirstTriggerThreshold);
        Act(LivingEntityType.Chicken);
        ExpendResources();

        if (isMoving) {
            MoveAction() // implement action here
        }
    }
}
