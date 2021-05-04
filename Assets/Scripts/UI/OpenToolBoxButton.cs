using System;
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

    private void Start()
    {
        UpdateButton();
    }

    private void OnEnable()
    {
        EventsManager.BindEvent(EventsManager.EventType.ToolBoxClose, InitiateEnable);
        EventsManager.BindEvent(EventsManager.EventType.ToolBoxOpen, DisableButton);
        EventsManager.BindEvent(EventsManager.ParameterEventType.SwappedGardenView, InitiateUpdateAction);
  
    }

    private void InitiateUpdateAction(EventsManager.EventParams _) => InitiateUpdate();
    

    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.ToolBoxOpen, DisableButton);
        EventsManager.UnbindEvent(EventsManager.EventType.ToolBoxClose, InitiateEnable);
        EventsManager.UnbindEvent(EventsManager.ParameterEventType.SwappedGardenView, InitiateUpdateAction);
       
    }

    // Update is called once per frame
    void Update()
    {
        enableDelay--;
        if (enableDelay <= 1)
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
        if (GameCore.GameManager.Instance.InOwnGarden)
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
    
        if (!GameCore.GameManager.Instance.InOwnGarden)
        {
            DisableButton();
        }
        else
        {
            EnableButton();
        }
    }

  
}
