﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class EndTurnButtonScript : MonoBehaviour
{
    public void CallEndTurn() => GameManager.Instance.EndTurn();
    Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        EventsManager.BindEvent(EventsManager.EventType.EndTurn, DisableButton);
        EventsManager.BindEvent(EventsManager.EventType.NewTurnBegin, EnableButton);
    }
    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.EndTurn, DisableButton);
        EventsManager.UnbindEvent(EventsManager.EventType.NewTurnBegin, EnableButton);
    }

    private void EnableButton()
    {
        button.interactable = true;
    }

    private void DisableButton()
    {
        button.interactable = false;
    }


}
