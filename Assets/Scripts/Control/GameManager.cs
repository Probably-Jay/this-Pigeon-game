using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// created jay 12/02

/// <summary>
/// <see cref="Singleton{}"/> class to allow for easy referencing of important objects
/// </summary>
[RequireComponent(typeof(HotSeatManager))]
[RequireComponent(typeof(DisplayManager))]
public class GameManager : Singleton<GameManager>
{
   

    public new static GameManager Instance { get => Singleton<GameManager>.Instance; }
    public HotSeatManager HotSeatManager { get; private set; }
    public DisplayManager PlantManager { get; private set; }

    public GoalManagerScript.Goal CurrentGoal { get; private set; }
    public GoalManagerScript.Goal AlternateGoal { get; private set; }

    public Player ActivePlayer => HotSeatManager.ActivePlayer;
    public int TurnCount => HotSeatManager.TurnTracker.Turn;

    public Player.PlayerEnum CurrentVisibleGarden => Camera.main.GetComponent<CameraMovementControl>().CurrentGardenView; // OPTIMISE


    //public override void Awake()
    //{
    //    base.InitSingleton();
    //    HotSeatManager = GetComponent<HotSeatManager>();
    //    PlantManager = GetComponent<PlantManager>();
    //}

    public override void Initialise()
    {
        base.InitSingleton();
        HotSeatManager = GetComponent<HotSeatManager>();
        PlantManager = GetComponent<DisplayManager>();
    }

    private void OnEnable()
    {
        EventsManager.BindEvent(EventsManager.EventType.StartGame, BeginGame);
    }

    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.StartGame, BeginGame);
    }

    private void Start()
    {
       // EventsManager.InvokeEvent(EventsManager.EventType.StartGame);

        //EventsManager.InvokeEvent(EventsManager.EventType.EndTurn);
      //  EventsManager.InvokeEvent(EventsManager.EventType.UpdateScore); // Maybe here for a reason?




    }

    void BeginGame()
    {
        CurrentGoal = GoalStore.GetGoal();
        AlternateGoal = GoalStore.GetAltGoal();
    }

    public void EndTurn() => EventsManager.InvokeEvent(EventsManager.EventType.EndTurn);



}
