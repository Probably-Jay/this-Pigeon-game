using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    public int gardenID = 1;
    public List<GameObject> gardenSlots;
    SlotControls slotControls;

    public GameObject seedStorage;
    GameObject newPlant;

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
                        newPlant = seedStorage.GetComponent<CurrentSeedStorage>().GetCurrentPlant();
                        slotControls.SpawnPlantInSlot(newPlant);
                        Debug.Log("you have planted a plant");
                        seedStorage.GetComponent<CurrentSeedStorage>().isStoringSeed = false;

                        HideSlots();
                    }
                }
            }
       }
    }


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