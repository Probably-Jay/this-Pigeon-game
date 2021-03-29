using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mood;

// created jay 12/02

/// <summary>
/// <see cref="Singleton{}"/> class to allow for easy referencing of important objects
/// </summary>
[RequireComponent(typeof(HotSeatManager))]
[RequireComponent(typeof(EmotionTracker))]
public class GameManager : Singleton<GameManager>
{

    public Dictionary<Player.PlayerEnum, SlotManager> slotManagers = new Dictionary<Player.PlayerEnum, SlotManager>();

    public new static GameManager Instance { get => Singleton<GameManager>.Instance; }
    public HotSeatManager HotSeatManager { get; private set; }

    public EmotionTracker CurrentMoods { get; private set; }

    public Emotion.Emotions Player1Goal { get; private set; }
    public Emotion.Emotions Player2Goal { get; private set; }

    public Player ActivePlayer => HotSeatManager.ActivePlayer;
    public int TurnCount => HotSeatManager.TurnTracker.Turn;

    public Player.PlayerEnum CurrentVisibleGarden => Camera.main.GetComponent<CameraMovementControl>().CurrentGardenView; // todo OPTIMISE

    public bool InOwnGarden => CurrentVisibleGarden == ActivePlayer.PlayerEnumValue;

    public override void Initialise()
    {
        base.InitSingleton();
        HotSeatManager = GetComponent<HotSeatManager>();
        CurrentMoods = GetComponent<EmotionTracker>();
    }

    private void OnEnable()
    {
        EventsManager.BindEvent(EventsManager.EventType.StartGame, BeginGame);
    }

    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.StartGame, BeginGame);
    }

    


    void BeginGame()
    {
        Player1Goal = GoalStore.GetGoal();
        Player2Goal = GoalStore.GetAltGoal();
    }

    public void EndTurn() => EventsManager.InvokeEvent(EventsManager.EventType.EndTurn);



}
