using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameCore;

//created Zap 26/03
// edited Xander 22/04/2021
public class UICommands : MonoBehaviour
{
    //[SerializeField] Button button;

    private void OnEnable()
    {

    }
    private void OnDisable()
    {

    }


    public void OpenToolBox() => EventsManager.InvokeEvent(EventsManager.EventType.ToolBoxOpen);
    public void CloseToolBox() => EventsManager.InvokeEvent(EventsManager.EventType.ToolBoxClose);

    

   

   
    public void CloseSeedBag() { EventsManager.InvokeEvent(EventsManager.EventType.SeedBagClose); Debug.Log("Closed Bag"); }
    public void Skip() => EventsManager.InvokeEvent(EventsManager.EventType.DialogueSkip);
    public void OkayNext() => EventsManager.InvokeEvent(EventsManager.EventType.DialogueNext);
    public void DialogueBack() => EventsManager.InvokeEvent(EventsManager.EventType.DialoguePrevious);
}
