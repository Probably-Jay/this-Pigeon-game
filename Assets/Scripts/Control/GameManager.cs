using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// created jay 12/02

/// <summary>
/// <see cref="Singleton{}"/> class to allow for easy referancing of important objects 
/// </summary>
[RequireComponent(typeof(HotSeatManager))]
public class GameManager : Singleton<GameManager>
{

    public new static GameManager Instance { get => Singleton<GameManager>.Instance; }
    public HotSeatManager HotSeatManager { get; private set; }

    public override void Awake()
    {
        base.InitSingleton();
        HotSeatManager = GetComponent<HotSeatManager>();
    }

    private void Start()
    {
        EventsManager.InvokeEvent(EventsManager.EventType.EndTurn);
    }

    public Player ActivePlayer => HotSeatManager.ActivePlayer;
    public int TurnCount => HotSeatManager.TurnTracker.Turn;


    public void EndTurn() => EventsManager.InvokeEvent(EventsManager.EventType.EndTurn);


}
