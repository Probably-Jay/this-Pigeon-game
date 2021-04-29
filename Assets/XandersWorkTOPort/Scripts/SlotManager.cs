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
    [SerializeField] List<SlotControls> gardenSlots;

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
        switch (plant.ThisPlantsSize)
        {
            case Plant.PlantSize.Wide:
                EventsManager.InvokeEvent(EventsManager.EventType.PlacedTallPlant);
                // Is wide even a thing anymore?
                break;
            case Plant.PlantSize.Tall:
                EventsManager.InvokeEvent(EventsManager.EventType.PlacedTallPlant);
                break;
            case Plant.PlantSize.Single:
                EventsManager.InvokeEvent(EventsManager.EventType.PlacedSmallPlant);
                break;
            default:
                EventsManager.InvokeEvent(EventsManager.EventType.PlacedSmallPlant);
                break;
        }

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
            var collider = slot.gameObject.GetComponent<BoxCollider2D>();

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = collider.transform.position.z;

            if (collider.OverlapPoint(mousePos))
            {
                return slot;
            }
        }
        return null;
    }
    
    /// <summary>
    /// Returns the plant of the slot the mouse is in, else returns null
    /// </summary>
    public Plant PlantOfSlotMouseIsIn(ToolDrag tool)
    {
       
        foreach (var slot in gardenSlots)
        {
       
            if (slot.plantsInThisSlot.Count!=0)
            {
                var plant = slot.plantsInThisSlot[0].GetComponent<Plants.Plant>();

            
                Rect plantRect = GetPlantRect(plant);

               /// plantZPos = plantRect.z

                Rect toolRect = tool.GetWorldRect();


                if (plantRect.Overlaps(toolRect))
                {  
                    return plant;
                }
            }
        }
        return null;
    }

    private static Rect GetPlantRect(Plant plant)
    {
        Collider2D plantCollider = plant.PlantGrowth.GetActiveCollider();

        Bounds plantBouds = plantCollider.bounds;

        var plantRect = new Rect(plantBouds.min.x, plantBouds.min.y, plantBouds.size.x, plantBouds.size.y);

        return plantRect;
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