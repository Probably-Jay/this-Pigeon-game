using System;
using System.Collections;
using System.Collections.Generic;
using Plants;
using UnityEngine;

// Script created by Alexander Purvis 05/02/2021

// (please sign alteration to script -jay)

public class SlotControls : MonoBehaviour
{
    // enums that control the state of this tile
 
    public bool slotActive = false;
    public bool slotFull = false;

    public List<GameObject> plantsInThisSlot;

 
    // to change the alpha of the object that is moving
    SpriteRenderer slotSprite;
    Color slotColourValues;

    public int slotType = 1;

    public Player.PlayerEnum playersGarden;
   
    private void Awake()
    {
        slotSprite = this.GetComponent<SpriteRenderer>();
        slotColourValues = slotSprite.material.color;
        slotColourValues.a = 0.0f;
        slotSprite.material.color = slotColourValues;
    }


    public void ShowSlot()
    {
        // changes the alpha of the slot so that it can be seen and 
        // changes the bool to true so that it can be interacted with 
        slotColourValues = slotSprite.material.color;
        slotColourValues.a = 0.6f;
        slotSprite.material.color = slotColourValues;

        slotActive = true;
    }

    public void HideSlot()
    {   // changes the alpha of the slot so that it can not be seen and 
        // changes the bool to false so that it can not be interacted with 
        slotColourValues = slotSprite.material.color;
        slotColourValues.a = 0.0f;
        slotSprite.material.color = slotColourValues;

        slotActive = false;
    }
 

    public void FreeSlot()
    {
        // switches the colours of the Slot to reflect the that this tiSlotle is not occupied    
        slotColourValues = slotSprite.material.color;

        slotColourValues.r = 1.0f;
        slotColourValues.g = 1.0f;
        slotColourValues.b = 1.0f;

        slotSprite.material.color = slotColourValues;

        plantsInThisSlot.Clear();

        slotFull = false;
    }


    public void SlotOccupied()
    {
        // switches the colours to reflect the that this Slot is occupied       
        slotColourValues = slotSprite.material.color;

        slotColourValues.r = 3.5f;
        slotColourValues.g = 1.0f;
        slotColourValues.b = 0.1f;
        slotSprite.material.color = slotColourValues;

        // sets the Slot state to taken
        slotFull = true;
    }


    public Plant SpawnPlantInSlot(GameObject PlantToSpawn, int slotNumber)
    {
        GameObject newPlant = Instantiate(PlantToSpawn, transform);
        newPlant.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z -1);


        Plant plant = newPlant.GetComponent<Plant>();
        plant.Init(
            garden: playersGarden,
            slot: slotNumber
        );


        plantsInThisSlot.Clear();
        plantsInThisSlot.Add(newPlant);
        SlotOccupied();

        return plant;
    }  
    
    public Plant ReSpawnPlantInSlot(GameObject PlantToSpawn, int slotNumber, NetSystem.GardenDataPacket.Plant plantData)
    {
        Plant plant = SpawnPlantInSlot(PlantToSpawn, slotNumber);
        plant.SetState(plantData);
        return plant;
    }

    public void RemovePlantFromSlot()
    {
        for (int i = 0; i < plantsInThisSlot.Count; i++)
        {
            Destroy(plantsInThisSlot[i]);
        }

        FreeSlot();
    }


    public List<Plant> GetAllPlants()
    {
        List<Plant> plants = new List<Plant>();
        foreach (var plantObject in plantsInThisSlot)
        {
            plants.Add(plantObject.GetComponent<Plant>());
        }
        return plants;
    }
}