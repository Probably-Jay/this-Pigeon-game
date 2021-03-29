using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Plants;
public class GardenSlotDirectory : MonoBehaviour
{
    public GameObject SeedSelected;

    GameObject currentPlantSeed;

    public GameObject garden1Slots;
    public GameObject garden2Slots;

    private void Start()
    {
        GameManager.Instance.slotManagers.Add(Player.PlayerEnum.Player1, garden1Slots.GetComponent<SlotManager>());
        GameManager.Instance.slotManagers.Add(Player.PlayerEnum.Player2, garden2Slots.GetComponent<SlotManager>());
    }

    public void AccessAppropriateSlotManager()
    {
        currentPlantSeed = SeedSelected.GetComponent<CurrentSeedStorage>().GetCurrentPlant();

        int requiredSlotType = currentPlantSeed.GetComponent<Plant>().requiredSlot;

        if (GameManager.Instance.CurrentVisibleGarden == Player.PlayerEnum.Player1)
        {
            garden1Slots.GetComponent<SlotManager>().ShowSlots(requiredSlotType);
        }
        else if (GameManager.Instance.CurrentVisibleGarden == Player.PlayerEnum.Player2)
        {
            garden2Slots.GetComponent<SlotManager>().ShowSlots(requiredSlotType);
        }
    }
}
