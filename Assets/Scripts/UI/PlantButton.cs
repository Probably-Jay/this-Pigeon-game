using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantButton : MonoBehaviour
{
    public GameObject myPlant;
    public CurrentSeedStorage seedStorage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GoToPlanting()
    {
        seedStorage.SetCurrentPlant(myPlant);
        EventsManager.InvokeEvent(EventsManager.EventType.PlantingBegin);
        Debug.Log("Begin Planting Mode");
    }
}
