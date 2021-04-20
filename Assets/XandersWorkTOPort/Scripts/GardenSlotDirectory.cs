using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Plants;
public class GardenSlotDirectory : MonoBehaviour
{
    [SerializeField] CurrentSeedStorage SeedSelected;

    GameObject currentPlantSeed;

    public SlotManager garden1Slots;
    public SlotManager garden2Slots;

   // SeedIndicator seedIndicator;

    public SeedIndicator garden1SeedIndicator;
    public SeedIndicator garden2SeedIndicator;



    private void OnEnable()
    {
        GameManager.Instance.RegisterSlotManager(garden1Slots.gardenplayerID, garden1Slots);
        GameManager.Instance.RegisterSlotManager(garden2Slots.gardenplayerID, garden2Slots);
    }
    private void OnDisable()
    {
        GameManager.Instance.UnregisterSlotManager(garden1Slots.gardenplayerID);
        GameManager.Instance.UnregisterSlotManager(garden2Slots.gardenplayerID);
    }

    public void AccessAppropriateSlotManager()
    {
        currentPlantSeed = SeedSelected.GetCurrentPlant();

        int requiredSlotType = currentPlantSeed.GetComponent<Plant>().requiredSlot;

        if (GameManager.Instance.PlayerWhosGardenIsCurrentlyVisible == Player.PlayerEnum.Player1)
        {
            HideAllSlotsAndHideIndicators();
            garden1Slots.ShowSlots(requiredSlotType);
            setGarden1IndicatorSeed();
        }
        else if (GameManager.Instance.PlayerWhosGardenIsCurrentlyVisible == Player.PlayerEnum.Player2)
        {
           HideAllSlotsAndHideIndicators();
           garden2Slots.ShowSlots(requiredSlotType);
           setGarden2IndicatorSeed();
        }
    }


    public void HideAllSlotsAndHideIndicators()
    {
        garden1Slots.HideSlots();
        garden2Slots.HideSlots();

        garden1SeedIndicator.HideIndicator();
        garden2SeedIndicator.HideIndicator();
       
    }


    void setGarden1IndicatorSeed()
    {
        garden1SeedIndicator.ShowIndicator(currentPlantSeed);
    }


    void setGarden2IndicatorSeed()
    {
        garden2SeedIndicator.ShowIndicator(currentPlantSeed);
    }
}
