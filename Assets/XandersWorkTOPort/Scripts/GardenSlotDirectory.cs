﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Plants;
using GameCore;
using System;

// Script created by Alexander Purvis 01/05/2021
public class GardenSlotDirectory : MonoBehaviour
{
    [SerializeField] CurrentSeedStorage SeedSelected;

    GameObject currentPlantSeed;

    [SerializeField] SlotManager garden1Slots;
    [SerializeField] SlotManager garden2Slots;

    public SeedIndicator garden1SeedIndicator;
    public SeedIndicator garden2SeedIndicator;


    // asigns the gardens to the apropriete Player
    private void Awake()
    {
       // localPlayerSlotManager = GameManager.Instance.LocalPlayerID == Player.PlayerEnum.Player1 ? garden1Slots : garden2Slots;
        GameManager.Instance.RegisterLocalSlotManager(garden1Slots, garden1Slots.gardenplayerID);
        GameManager.Instance.RegisterLocalSlotManager(garden2Slots, garden2Slots.gardenplayerID);
    }



    //private void OnEnable()
    //{
    //   // GameManager.Instance.RegisterSlotManager(garden1Slots.gardenplayerID, garden1Slots);
    //   // GameManager.Instance.RegisterSlotManager(garden2Slots.gardenplayerID, garden2Slots);
    //   //var localSlot =
    //  // GameManager.Instance.RegisterLocalSlotManager()
    //   throw new NotImplementedException("Above line was removed in hotseat removal and has not been re-implimented");

    //}
    //private void OnDisable()
    //{
    //    if (GameManager.InstanceExists)
    //    {
    //        //GameManager.Instance.UnregisterSlotManager(garden1Slots.gardenplayerID);
    //        //GameManager.Instance.UnregisterSlotManager(garden2Slots.gardenplayerID);
    //        throw new NotImplementedException("Above line was removed in hotseat removal and has not been re-implimented");

    //    }
    //}

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
