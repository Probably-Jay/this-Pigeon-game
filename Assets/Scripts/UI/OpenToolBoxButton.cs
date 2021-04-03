using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Created Zap 27/03, adapted from EndTurnButtonScript.cs
public class OpenToolBoxButton : MonoBehaviour
{
    Button button;
    int frames;
    int enableDelay;
    private void Awake()
    {
        button = this.GetComponent<Button>();
    }

    private void OnEnable()
    {
        EventsManager.BindEvent(EventsManager.EventType.ToolBoxClose, InitiateEnable);
        EventsManager.BindEvent(EventsManager.EventType.ToolBoxOpen, DisableButton);
        EventsManager.BindEvent(EventsManager.ParameterEventType.SwappedGardenView, InitiateUpdateAction);
       // EventsManager.BindEvent(EventsManager.ParameterEventType.SwappedGardenView, (_) => InitiateUpdate());
        EventsManager.BindEvent(EventsManager.EventType.StartGame, InitiateUpdate);
        EventsManager.BindEvent(EventsManager.EventType.NewTurnBegin, InitiateUpdate);
    }

    private void InitiateUpdateAction(EventsManager.EventParams _) => InitiateUpdate();
    

    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.ToolBoxOpen, DisableButton);
        EventsManager.UnbindEvent(EventsManager.EventType.ToolBoxClose, InitiateEnable);
        EventsManager.UnbindEvent(EventsManager.ParameterEventType.SwappedGardenView, InitiateUpdateAction);
       // EventsManager.UnbindEvent(EventsManager.ParameterEventType.SwappedGardenView, (_) => InitiateUpdate()); ;
        EventsManager.UnbindEvent(EventsManager.EventType.StartGame, InitiateUpdate);
        EventsManager.UnbindEvent(EventsManager.EventType.NewTurnBegin, InitiateUpdate);
    }

    // Update is called once per frame
    void Update()
    {
        enableDelay--;
        if (enableDelay == 1)
        {
            EnableButton();
        }
        frames--;
        if (frames > 0)
        {
            UpdateButton();
        }
        
    }
    void EnableButton()
    {
        if (GameManager.Instance.InOwnGarden)
        {
            button.interactable = true;
        }
    }
    void DisableButton()
    {
        button.interactable = false;
    }
    void InitiateUpdate()
    {
        frames = 10;
        UpdateButton();
    }
    void InitiateEnable()
    {
        enableDelay = 100;
    }
    void UpdateButton()
    {
        
        if (GameManager.Instance.InOwnGarden)
        {
            EnableButton();
        }
        else
        {
            DisableButton();
        }
    }
}
