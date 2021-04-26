using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mood;

// created jay 12/02

namespace GameCore
{

    /// <summary>
    /// <see cref="Singleton{}"/> class to allow for easy referencing of important objects
    /// </summary>
    //[RequireComponent(typeof(HotSeatManager))]
    [RequireComponent(typeof(EmotionTracker))]
    [RequireComponent(typeof(OnlineTurnManager))]
    public class GameManager : Singleton<GameManager>
    {
        public new static GameManager Instance { get => Singleton<GameManager>.Instance; }

        //  public HotSeatManager HotSeatManager { get; private set; }
        public OnlineTurnManager OnlineTurnManager { get; private set; }

        public EmotionTracker EmotionTracker { get; private set; }

        public Emotion.Emotions Player1Goal { get; private set; }
        public Emotion.Emotions Player2Goal { get; private set; }

        public Player ActivePlayer => HotSeatManager.ActivePlayer;

        public Player.PlayerEnum ActivePlayerID => ActivePlayer.EnumID;

        public Player GetPlayer(Player.PlayerEnum player) => HotSeatManager.players[player];

        public int TurnCount => HotSeatManager.TurnTracker.Turn;

        public Player.PlayerEnum PlayerWhosGardenIsCurrentlyVisible => Camera.main.GetComponent<CameraMovementControl>().CurrentGardenView; // todo OPTIMISE

        public bool InOwnGarden => PlayerWhosGardenIsCurrentlyVisible == ActivePlayerID;

        public Dictionary<Player.PlayerEnum, SlotManager> SlotManagers { get; private set; } = new Dictionary<Player.PlayerEnum, SlotManager>();

        public override void Initialise()
        {
            base.InitSingleton();
            if(GetComponent<HotSeatManager>() != null){
                Debug.LogError($"{nameof(HotSeatManager)} has been depracated");
            }
            OnlineTurnManager = GetComponent<OnlineTurnManager>();
            EmotionTracker = GetComponent<EmotionTracker>();
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
            //Player1Goal = GoalStore.GetGoal();
            //Player2Goal = GoalStore.GetAltGoal();
            Player1Goal = GoalStore.GetLoaclGoal(); // both for now, to be replaced when loading introduced
            Player2Goal = GoalStore.GetLoaclGoal();
        }

        public void EndTurn() => EventsManager.InvokeEvent(EventsManager.EventType.EndTurn);

        public void RegisterSlotManager(Player.PlayerEnum player, SlotManager slotManager) => SlotManagers.Add(player, slotManager);
        public void UnregisterSlotManager(Player.PlayerEnum player) => SlotManagers.Remove(player);



    }
}