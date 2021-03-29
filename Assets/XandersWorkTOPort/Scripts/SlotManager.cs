using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Plants;

public class SlotManager : MonoBehaviour
{
    public int gardenID = 1;
    public List<GameObject> gardenSlots;
    SlotControls slotControls;

    public GameObject seedStorage;
    GameObject newPlant;

    SeedIndicator seedIndicator;

    public GameObject gardenSeedIndicator;


    private void Update()
    {          
       if (seedStorage.GetComponent<CurrentSeedStorage>().isStoringSeed)
       {
            for (int slotNumber = 0; slotNumber < gardenSlots.Count; slotNumber++)
            {
                slotControls = gardenSlots[slotNumber].GetComponent<SlotControls>();
         
                if (slotControls.slotActive == true && slotControls.slotFull == false)
                {
                    var colider = gardenSlots[slotNumber].GetComponent<BoxCollider2D>();
                    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mousePos.z = colider.transform.position.z;

                    if (colider.OverlapPoint(mousePos) && Input.GetMouseButtonDown(0))
                    {
                        PlantPlant();
                        seedIndicator = gardenSeedIndicator.GetComponent<SeedIndicator>();
                        seedIndicator.HideIndicator();
                    }
                }
            }
       }
    }

    private void PlantPlant()
    {
        newPlant = seedStorage.GetComponent<CurrentSeedStorage>().GetCurrentPlant();
        slotControls.SpawnPlantInSlot(newPlant);
        seedStorage.GetComponent<CurrentSeedStorage>().isStoringSeed = false;
        InvokePlantedEvent(newPlant.GetComponent<Plants.Plant>());

        HideSlots();
    }

    private static void InvokePlantedEvent(Plant plant)
    {
        if (GameManager.Instance.InOwnGarden)
        {
            EventsManager.InvokeEvent(EventsManager.EventType.PlacedOwnObject);

            Mood.TraitValue CurrentMood = GameManager.Instance.EmotionTracker.GardenEmotions[GameManager.Instance.ActivePlayer.PlayerEnumValue];
            if (CurrentMood.Overlaps(plant.TraitsUnscaled))
            {
                EventsManager.InvokeEvent(EventsManager.EventType.PlacedOwnObjectMoodRelavent);
            }
        }
        else
        {
            EventsManager.InvokeEvent(EventsManager.EventType.PlacedCompanionObject);
        }
    }


    public SlotControls SlotMouseIsIn()
    {
        foreach (var gameObject in gardenSlots)
        {
            var colider = gameObject.GetComponent<BoxCollider2D>();

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = colider.transform.position.z;

            if (colider.OverlapPoint(mousePos))
            {
                return gameObject.GetComponent<SlotControls>();
            }
        }
        return null;
    }
    /*
    public GameObject PlantMouseIsIn()
    {
        foreach (var gameObject in gardenSlots)
        {
            var slot 
            var colider = gameObject.GetComponent<BoxCollider2D>();

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = colider.transform.position.z;

            if (colider.OverlapPoint(mousePos))
            {
                return gameObject.GetComponent<SlotControls>();
            }
        }
        return null;
    }
    */

    public void ShowSlots(int requiredSlotType)
    {       
        for (int slotNumber = 0; slotNumber < gardenSlots.Count; slotNumber++)
        {
            slotControls = gardenSlots[slotNumber].GetComponent<SlotControls>();
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
            slotControls = gardenSlots[gridNumber].GetComponent<SlotControls>();
      
            slotControls.HideSlot();        
        }
    }
}