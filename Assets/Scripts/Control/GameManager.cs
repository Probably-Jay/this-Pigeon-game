﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// created jay 12/02

/// <summary>
/// <see cref="Singleton{}"/> class to allow for easy referencing of important objects 
/// </summary>
[RequireComponent(typeof(HotSeatManager))]
[RequireComponent(typeof(PlantManager))]
public class GameManager : Singleton<GameManager>
{

    public new static GameManager Instance { get => Singleton<GameManager>.Instance; }
    public HotSeatManager HotSeatManager { get; private set; }
    public PlantManager PlantManager { get; private set; }

    public override void Awake()
    {
        base.InitSingleton();
        HotSeatManager = GetComponent<HotSeatManager>();
        PlantManager = GetComponent<PlantManager>();
    }

    private void Start()
    {
        EventsManager.InvokeEvent(EventsManager.EventType.EndTurn);
        EventsManager.InvokeEvent(EventsManager.EventType.UpdateScore);
    }

    public Player ActivePlayer => HotSeatManager.ActivePlayer;
    public int TurnCount => HotSeatManager.TurnTracker.Turn;


    public void EndTurn() => EventsManager.InvokeEvent(EventsManager.EventType.EndTurn);


}
