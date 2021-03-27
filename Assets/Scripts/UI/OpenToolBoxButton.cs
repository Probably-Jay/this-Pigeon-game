using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Created Zap 27/03, adapted from EndTurnButtonScript.cs
public class OpenToolBoxButton : MonoBehaviour
{
    Button button;
    private void OnEnable()
    {
        button = this.GetComponent<Button>();
        EventsManager.BindEvent(EventsManager.EventType.ToolBoxClose, EnableButton);
        EventsManager.BindEvent(EventsManager.EventType.ToolBoxOpen, DisableButton);
    }
    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.ToolBoxOpen, DisableButton);
        EventsManager.UnbindEvent(EventsManager.EventType.ToolBoxClose, EnableButton);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void EnableButton()
    {
        button.interactable = true;
    }
    void DisableButton()
    {
        button.interactable = false;
    }
}
