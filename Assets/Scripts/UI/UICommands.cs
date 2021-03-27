using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//created Zap 26/03
public class UICommands : MonoBehaviour
{
    private void OnEnable()
    {
    }
    private void OnDisable()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OpenToolBox() => EventsManager.InvokeEvent(EventsManager.EventType.ToolBoxOpen);
    public void CloseToolBox() => EventsManager.InvokeEvent(EventsManager.EventType.ToolBoxClose);

}
