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

    // for network to add plants
    [SerializeField] List<GameObject> plantList;


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
        newPlant = seedStorage.GetCurrentPlant();
        slotControls.SpawnPlantInSlot(newPlant, slotNumber);
        var plantedPlant = slotControls.SpawnPlantInSlot(newPlant);

        plantedPlant.GetComponent<Plant>().Init(
            garden: (int)GameCore.GameManager.Instance.PlayerWhosGardenIsCurrentlyVisible,
            slot: gardenSlots.IndexOf(slotControls)
        );

        seedStorage.isStoringSeed = false;
        InvokePlantedEvent(plantedPlant.GetComponent<Plants.Plant>(), slotControls);

        HideSlots();
    }

    private void InvokePlantedEvent(Plant plant, SlotControls slotControls)
    {
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


        ReadOnlyCollection<Plants.PlantActions.TendingActions> requiredActions = plant.PlantGrowth.TendingState.GetRequiredActions();

        Plant.PlantName plantname = plant.plantname;
        int slotNumber = gardenSlots.IndexOf(slotControls);
        bool watering = requiredActions.Contains(Plants.PlantActions.TendingActions.Watering);
        bool spraying = requiredActions.Contains(Plants.PlantActions.TendingActions.Spraying);
        bool trimming = requiredActions.Contains(Plants.PlantActions.TendingActions.Trimming);

        switch (GameManager.Instance.PlayerWhosGardenIsCurrentlyVisible)
        {
            case Player.PlayerEnum.Player1:
                GameManager.Instance.DataManager.AddPlantToGarden1(
                    plantType: (int)plantname,
                    slotNumber: slotNumber,
                    stage: 0,
                    watering: watering,
                    spraying: spraying,
                    trimming: trimming
                    ) ;
                break;
            case Player.PlayerEnum.Player2:
                GameManager.Instance.DataManager.AddPlantToGarden2(
                   plantType: (int)plantname,
                   slotNumber: slotNumber,
                   stage: 0,
                   watering: watering,
                   spraying: spraying,
                   trimming: trimming
                   );
                break;
        }



        //EventsManager.InvokeEvent(EventsManager.ParameterEventType.OnPlantPlanted, new EventsManager.EventParams()
        //{
        //    EnumData1 = plant.plantname, // play type
        //    EnumData2 = GameManager.Instance.PlayerWhosGardenIsCurrentlyVisible, // gardenSlot
        //    IntData1 = gardenSlots.IndexOf(slotControls),// slot number
        //    IntData2 = plant.PlantGrowth.TendingState.CurrentGrowthStage, // stage
        //    Bool1 = requiredActions.Contains(Plants.PlantActions.TendingActions.Watering), // watering
        //    Bool2 = requiredActions.Contains(Plants.PlantActions.TendingActions.Spraying), // spraying
        //    Bool3 = requiredActions.Contains(Plants.PlantActions.TendingActions.Trimming), // trimming
        //});
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
        var slotControls = gardenSlots[slotNumber].GetComponent<SlotControls>();
        slotControls.RemovePlantFromSlot();
    }

    // NetworkFunctions 
    public void AddPlantFromServer(int slotNumber, int plantNumber)
    {
        newPlant = plantList[plantNumber];

        var slotControls = gardenSlots[slotNumber].GetComponent<SlotControls>();

        slotControls.SpawnPlantInSlot(newPlant, slotNumber);  
    }

    public void ClearGarden()
    {
        for(int i =0; i< gardenSlots.Count; i++)
        {
            var slotControls = gardenSlots[i].GetComponent<SlotControls>();
            slotControls.RemovePlantFromSlot();
        }    
    }
}