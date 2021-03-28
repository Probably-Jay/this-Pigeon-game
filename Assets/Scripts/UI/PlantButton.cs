using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantButton : MonoBehaviour
{
    public GameObject myPlant;
    public CurrentSeedStorage seedStorage;

    GardenSlotDirectory gardenSlotDirectory;

    // Start is called before the first frame update
    void Start()
    {
        gardenSlotDirectory = GameObject.FindObjectOfType<GardenSlotDirectory>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToPlanting()
    {
        seedStorage.SetCurrentPlant(myPlant);
        seedStorage.isStoringSeed = true;
        EventsManager.InvokeEvent(EventsManager.EventType.PlantingBegin);
        Debug.Log("Begin Planting Mode");

        gardenSlotDirectory.AccessAppropriateSlotManager();
    }
}
