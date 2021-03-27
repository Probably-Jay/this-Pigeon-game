﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Plants;

// Script for finding / growing all plants in scene
// Scott Jarvis, 24/03/21

public class PlantFinder : MonoBehaviour
{

    public List<Plant> gardenPlants = new List<Plant>(); // Holds both gardens in same var

    private void OnEnable()
    {
        EventsManager.BindEvent(EventsManager.EventType.PlacedOwnObject, FindPlants);
        EventsManager.BindEvent(EventsManager.EventType.PlacedCompanionObject, FindPlants);
        EventsManager.BindEvent(EventsManager.EventType.EndTurn, GrowPlants);
    }

    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.PlacedOwnObject, FindPlants);
        EventsManager.UnbindEvent(EventsManager.EventType.PlacedCompanionObject, FindPlants);
        EventsManager.UnbindEvent(EventsManager.EventType.EndTurn, GrowPlants);
    }

    /// <summary>
    /// Search through all plants, adding them to an internal array
    /// </summary>
    public void FindPlants()
    {
        gardenPlants.Clear(); // Stops duplicates - Hacky, replace later
        var foundPlants = FindObjectsOfType<Plant>();
        for (int i = 0; i < foundPlants.Length; i++)
        {
            gardenPlants.Add(foundPlants[i]);
        }
    }


    /// <summary>
    /// Grows any that fill the requirements to go to the next stage
    /// </summary>
    public void GrowPlants()
    {
        for (int i = 0; i < gardenPlants.Count; i++)
        {

            // Water-based growth, add more later
            if (gardenPlants[i].currGrowth >= gardenPlants[i].growthGoal)
            {
                switch (gardenPlants[i].plantGrowthState)
                {
                    case Plant.PlantGrowthStage.Seed:
                        gardenPlants[i].plantGrowthState = Plant.PlantGrowthStage.Sprout;
                        gardenPlants[i].moodMult = 1;
                        break;
                    case Plant.PlantGrowthStage.Sprout:
                        gardenPlants[i].plantGrowthState = Plant.PlantGrowthStage.Bloom;
                        gardenPlants[i].moodMult = 2;
                        break;
                    case Plant.PlantGrowthStage.Bloom: 
                        // Nothing happens for now, plant is fully grown
                        // Maybe add little event later if we have time
                        break;
                }
                gardenPlants[i].currGrowth = 0;
            }
            gardenPlants[i].UpdateSprite();

        }
    }

}
