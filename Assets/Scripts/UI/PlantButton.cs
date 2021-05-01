using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script called by "select plant" buttom in seed menu
/// </summary>
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
        EventsManager.InvokeEvent(EventsManager.EventType.SeedbagShuffle);
        seedStorage.SetCurrentPlant(myPlant);
        EventsManager.InvokeEvent(EventsManager.EventType.PlantingBegin);
      //  Debug.Log("Begin Planting Mode");

        gardenSlotDirectory.AccessAppropriateSlotManager();
    }
}
