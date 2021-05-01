using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Plants;
using GameCore;

// Created by Alexander purvis 

// Plant-getters added - Jay 

// eddited by Alexander purvis 29/04/2021

public class SlotManager : MonoBehaviour
{
    public Player.PlayerEnum gardenplayerID ;
    [SerializeField] List<SlotControls> gardenSlots;

   // SlotControls slotControls;
    public CurrentSeedStorage seedStorage;
    GameObject newPlant;

   // SeedIndicator seedIndicator;
    public SeedIndicator gardenSeedIndicator;

    //// for network to add plants
    //[SerializeField] List<GameObject> plantList;
    
    // for network to add plants
    [SerializeField] InventoryList plantList;

    private void Awake()
    {
        foreach (var slot in gardenSlots)
        {
            slot.playersGarden = gardenplayerID;
        }
    }

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
                        PlantPlant(slotControls,slotNumber);
                        gardenSeedIndicator.HideIndicator();
                    }
                }
            }
       }
    }

    private void PlantPlant(SlotControls slotControls, int slotNumber)
    {
        var newPlant = seedStorage.GetCurrentPlant();
     
        var plantedPlant = slotControls.SpawnPlantInSlot(newPlant, slotNumber);

        seedStorage.ClearCurrentPlant();

        InvokePlantedEvent(plantedPlant, slotControls);

        SavePlant(plantedPlant, slotControls);

        HideSlots();
    }

      // NetworkFunctions 
    public void AddPlantFromServer(int slotNumber, NetSystem.GardenDataPacket.Plant plantData)
    {
        if (!plantData.Initilised)
        {
            return;
        }

        var slotControls = gardenSlots[slotNumber];

        var newPlant = plantList.list[plantData.plantType].itemGameObject;

        var plantedPlant = slotControls.ReSpawnPlantInSlot(newPlant, slotNumber, plantData);

        InvokePlantedEvent(plantedPlant, slotControls);

        SavePlant(plantedPlant, slotControls);
    }

    private void InvokePlantedEvent(Plant plant, SlotControls slotControls)
    {
        switch (plant.ThisPlantsSize)
        {
            // case Plant.PlantSize.Wide:
            //    EventsManager.InvokeEvent(EventsManager.EventType.PlacedOwnObject);
            //  break;
            case Plant.PlantSize.Tall:
                EventsManager.InvokeEvent(EventsManager.EventType.PlacedTallPlant);
                break;
            case Plant.PlantSize.Single:
                EventsManager.InvokeEvent(EventsManager.EventType.PlacedSmallPlant);
                break;
            default:
                break;
        }

        if (GameManager.Instance.InOwnGarden)
        {
            EventsManager.InvokeEvent(EventsManager.EventType.PlacedOwnObject);

            Mood.TraitValue moodGoal = GameManager.Instance.EmotionTracker.EmotionGoal.traits;

            if (moodGoal.Overlaps(plant.TraitsUnscaled)) // for tutorial
            {
                EventsManager.InvokeEvent(EventsManager.EventType.PlacedOwnObjectMoodRelevant);
            }
        }
        else
        {
            EventsManager.InvokeEvent(EventsManager.EventType.PlacedCompanionObject);
        }

    }

    private void SavePlant(Plant plant, SlotControls slotControls)
    {
       
        ReadOnlyCollection<Plants.PlantActions.TendingActions> requiredActions = plant.PlantGrowth.TendingState.GetRequiredActions();

        Plant.PlantName plantname = plant.plantname;
        int slotNumber = gardenSlots.IndexOf(slotControls);
        int stage = plant.InternalGrowthStage;
        bool watering = requiredActions.Contains(Plants.PlantActions.TendingActions.Watering);
        bool spraying = requiredActions.Contains(Plants.PlantActions.TendingActions.Spraying);
        bool trimming = requiredActions.Contains(Plants.PlantActions.TendingActions.Trimming);

        switch (slotControls.playersGarden)
        {
            case Player.PlayerEnum.Player1:
                GameManager.Instance.DataManager.AddPlantToGarden1(
                    plantType: (int)plantname,
                    slotNumber: slotNumber,
                    stage: stage,
                    watering: watering,
                    spraying: spraying,
                    trimming: trimming
                    );
                break;
            case Player.PlayerEnum.Player2:
                GameManager.Instance.DataManager.AddPlantToGarden2(
                   plantType: (int)plantname,
                   slotNumber: slotNumber,
                   stage: stage,
                   watering: watering,
                   spraying: spraying,
                   trimming: trimming
                   );
                break;
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

    public void RemovePlantWithTrowel(int slotNumber)
    {
        var slotControls = gardenSlots[slotNumber];
        slotControls.RemovePlantFromSlot();
    }

  

    public void ClearGarden()
    {
        for(int i =0; i< gardenSlots.Count; i++)
        {
            var slotControls = gardenSlots[i];
            slotControls.RemovePlantFromSlot();
        }    
    }
}