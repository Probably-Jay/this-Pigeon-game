using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenSlotDirectory : MonoBehaviour
{
    public GameObject SeedSelected;

    public GameObject garden1Slots;
    public GameObject garden2Slots;


    public void AccessAppropriateSlotManager()
    {
        int plantType = 1;

        if (GameManager.Instance.CurrentVisibleGarden == Player.PlayerEnum.Player1)
        {
            garden1Slots.GetComponent<SlotManager>().ShowSlots(plantType);
        }
        else if (GameManager.Instance.CurrentVisibleGarden == Player.PlayerEnum.Player2)
        {
            garden2Slots.GetComponent<SlotManager>().ShowSlots(plantType);
        }
    }
}
