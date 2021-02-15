using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputFaker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EventsManager.InvokeEvent(EventsManager.EventType.triedToPlaceOwnObject);
        }        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EventsManager.InvokeEvent(EventsManager.EventType.triedToPlaceCompanionObject);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            EventsManager.InvokeEvent(EventsManager.EventType.triedToRemoveOwnObject);
        }        
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            EventsManager.InvokeEvent(EventsManager.EventType.triedToWaterOwnPlant);
        }
    }
}
