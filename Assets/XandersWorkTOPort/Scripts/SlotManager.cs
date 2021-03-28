using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    public int gardenID = 1;
    public List<GameObject> gardenSlots;
    SlotControls slotControls;


    public void ShowSlots(int platType)
    {       
        for (int gridNumber = 0; gridNumber < gardenSlots.Count; gridNumber++)
        {
            slotControls = gardenSlots[gridNumber].GetComponent<SlotControls>();
            if (slotControls.slotType == platType)
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