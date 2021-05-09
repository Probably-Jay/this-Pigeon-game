using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Plants;

// Script for finding / growing all plants in scene
// Scott Jarvis, 24/03/21

// depracated jay 27/03 

[System.Obsolete("Plants now responsible for growing themselves")]
public class PlantFinder : MonoBehaviour
{

    public List<Plant> gardenPlants = new List<Plant>(); // Holds both gardens in same var

    //private void OnEnable()
    //{
    //    EventsManager.BindEvent(EventsManager.EventType.PlacedOwnObject, FindPlants);
    //    EventsManager.BindEvent(EventsManager.EventType.PlacedCompanionObject, FindPlants);
    //    EventsManager.BindEvent(EventsManager.EventType.EndTurn, GrowPlants);
    //}

    //private void OnDisable()
    //{
    //    EventsManager.UnbindEvent(EventsManager.EventType.PlacedOwnObject, FindPlants);
    //    EventsManager.UnbindEvent(EventsManager.EventType.PlacedCompanionObject, FindPlants);
    //    EventsManager.UnbindEvent(EventsManager.EventType.EndTurn, GrowPlants);
    //}

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
            //gardenPlants[i].PlantGrowth.GrowIfShould();
        }
    }

}
