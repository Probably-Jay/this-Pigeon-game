using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Plants;

// please sign script creation

// all-plants-getter added - Jay 



public class SlotManager : MonoBehaviour
{
    public Player.PlayerEnum gardenplayerID ;
    public List<SlotControls> gardenSlots;

   // SlotControls slotControls;

    public CurrentSeedStorage seedStorage;
    GameObject newPlant;

   // SeedIndicator seedIndicator;

    public SeedIndicator gardenSeedIndicator;


  
    private void Update()
    {          
       if (seedStorage.isStoringSeed)
       {
            for (int slotNumber = 0; slotNumber < gardenSlots.Count; slotNumber++)
            {
                var slotControls = gardenSlots[slotNumber];
         
                if (slotControls.slotActive == true && slotControls.slotFull == false)
                {
                    var colider = gardenSlots[slotNumber].GetComponent<BoxCollider2D>();
                    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mousePos.z = colider.transform.position.z;

                    if (colider.OverlapPoint(mousePos) && Input.GetMouseButtonDown(0))
                    {
                        PlantPlant(slotControls);
                        gardenSeedIndicator.HideIndicator();
                    }
                }
            }
       }
    }

    private void PlantPlant(SlotControls slotControls)
    {
        newPlant = seedStorage.GetCurrentPlant();
        slotControls.SpawnPlantInSlot(newPlant);
        seedStorage.isStoringSeed = false;
        InvokePlantedEvent(newPlant.GetComponent<Plants.Plant>());

        HideSlots();
    }

    private static void InvokePlantedEvent(Plant plant)
    {
        if (GameManager.Instance.InOwnGarden)
        {
            EventsManager.InvokeEvent(EventsManager.EventType.PlacedOwnObject);

            Mood.TraitValue moodGoal = GameManager.Instance.EmotionTracker.GardenGoalTraits[GameManager.Instance.ActivePlayerID];
            if (moodGoal.Overlaps(plant.TraitsUnscaled))
            {
                EventsManager.InvokeEvent(EventsManager.EventType.PlacedOwnObjectMoodRelevant);
            }
        }
        else
        {
            EventsManager.InvokeEvent(EventsManager.EventType.PlacedCompanionObject);
        }
    }


    public SlotControls SlotMouseIsIn()
    {
        foreach (var slot in gardenSlots)
        {
            var colider = slot.gameObject.GetComponent<BoxCollider2D>();

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = colider.transform.position.z;

            if (colider.OverlapPoint(mousePos))
            {
                return slot;
            }
        }
        return null;
    }
    
    public Plant PlantMouseIsIn()
    {
       
        foreach (var slot in gardenSlots)
        {
       
            if (slot.plantsInThisSlot.Count!=0) {
                var plant = slot.plantsInThisSlot[0].GetComponent<Plants.Plant>();
                Collider2D plantCollider = plant.PlantGrowth.GetActiveCollider();
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = plantCollider.transform.position.z;

                if (plantCollider.OverlapPoint(mousePos))
                {
                    return plant;
                }
            }
                

            
        }
        return null;
    }
    

    public void ShowSlots(int requiredSlotType)
    {       
        for (int slotNumber = 0; slotNumber < gardenSlots.Count; slotNumber++)
        {
            var slotControls = gardenSlots[slotNumber];
            if (slotControls.slotType == requiredSlotType)
            {               
                slotControls.ShowSlot();
            }
        }
    }


    public void HideSlots()
    {
        for (int gridNumber = 0; gridNumber < gardenSlots.Count; gridNumber++)
        {
            gardenSlots[gridNumber].HideSlot();
        }
    }

    public ReadOnlyCollection<Plant> GetAllPlants()
    {
        List<Plant> plants = new List<Plant>();
        foreach (var slot in gardenSlots)
        {
            plants.AddRange(slot.GetAllPlants());
        }

        return new ReadOnlyCollection<Plant>(plants);

    }

}