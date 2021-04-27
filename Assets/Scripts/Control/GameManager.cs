using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mood;
using System;

// created jay 12/02
// converted to non-hotseat jay 26/04

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

      //  public Emotion.Emotions Player1Goal { get; private set; }
      //  public Emotion.Emotions Player2Goal { get; private set; }

       // public Player ActivePlayer => OnlineTurnManager.ac;
        public Player LocalPlayer => OnlineTurnManager.LocalPlayer;
        public Player.PlayerEnum LocalPlayerID => OnlineTurnManager.LocalPlayer.EnumID;

        /// <summary>
        /// The player owns the turn and is currently playing
        /// </summary>
        public bool Playing => OnlineTurnManager.TurnTracker.CanPlayTurn;

        /// <summary>
        /// The player does not own the turn and is not currently playing
        /// </summary>
        public bool Spectating => !Playing;

        public int NewGameMoodGoalTemp;


        public DataManager DataManager { get; private set; }


       // public Player.PlayerEnum ActivePlayerID => ActivePlayer.EnumID;

       // public Player GetPlayer(Player.PlayerEnum player) => HotSeatManager.players[player];

      //  public int TurnCount => HotSeatManager.TurnTracker.Turn;

        public Player.PlayerEnum PlayerWhosGardenIsCurrentlyVisible => Camera.main.GetComponent<CameraMovementControl>().CurrentGardenView; // todo OPTIMISE

        
        public bool InOwnGarden => PlayerWhosGardenIsCurrentlyVisible == LocalPlayerID;

        public SlotManager LocalPlayerSlotManager { get; private set; }

        //public Dictionary<Player.PlayerEnum, SlotManager> SlotManagers { get; private set; } = new Dictionary<Player.PlayerEnum, SlotManager>();

        public override void Initialise()
        {
            base.InitSingleton();
            OnlineTurnManager = GetComponent<OnlineTurnManager>();
            EmotionTracker = GetComponent<EmotionTracker>();
        }

        private void OnEnable()
        {
            EventsManager.BindEvent(EventsManager.EventType.EnterGameScene, BeginOrResumeGame);
        }

        private void OnDisable()
        {
            EventsManager.UnbindEvent(EventsManager.EventType.EnterGameScene, BeginOrResumeGame);
        }


        public void EndTurn()
        {
            if (Spectating)
            {
                return;
            }
            OnlineTurnManager.EndTurn();
        }

        void BeginOrResumeGame()
        {
            DataManager = FindObjectOfType<DataManager>();


            if (OnlineTurnManager.Game.CurrentNetworkGame.NewGameJustCreated)
            {
                BeginNewGame();
                return;
            }

            ResumeGame();


            //Player1Goal = GoalStore.GetGoal();
            //Player2Goal = GoalStore.GetAltGoal();
            //Player1Goal = GoalStore.GetLoaclGoal(); // both for now, to be replaced when loading introduced
           //Player2Goal = GoalStore.GetLoaclGoal();
        }

        private void ResumeGame()
        {

            bool firstTimeEnteringGame = OnlineTurnManager.TurnTracker.Turn == 1;
            if (firstTimeEnteringGame)
            {
                OnlineTurnManager.ResumedGame(Player.PlayerEnum.Player2);
                EmotionTracker.InitialiseNewGame(NewGameMoodGoalTemp);
            }
            else
            {
                // OnlineTurnManager.ResumedGame(Player.PlayerEnum.Player2); // tofix
                EmotionTracker.ResumeGame();
                throw new NotImplementedException();
            }


            if (Playing)
            {
                EventsManager.InvokeEvent(EventsManager.EventType.ResumeGameOwnTurn);

                if (firstTimeEnteringGame)
                {
                    EventsManager.InvokeEvent(EventsManager.EventType.FirstTimeEnteringGame);
                }
            }
            else
            {
                EventsManager.InvokeEvent(EventsManager.EventType.ResumeGameSpectating);
            }
            EventsManager.InvokeEvent(EventsManager.EventType.GameLoaded);

        }

        private void BeginNewGame()
        {




            OnlineTurnManager.InitialiseNewGame();


            EmotionTracker.InitialiseNewGame(NewGameMoodGoalTemp);

            EventsManager.InvokeEvent(EventsManager.EventType.StartNewGame);
            EventsManager.InvokeEvent(EventsManager.EventType.GameLoaded);
            EventsManager.InvokeEvent(EventsManager.EventType.FirstTimeEnteringGame);
        }

       // public void EndTurn() => EventsManager.InvokeEvent(EventsManager.EventType.EndTurn);

        // public void RegisterSlotManager(Player.PlayerEnum player, SlotManager slotManager) => SlotManagers.Add(player, slotManager);
        // public void UnregisterSlotManager(Player.PlayerEnum player) => SlotManagers.Remove(player);

        public void RegisterLocalSlotManager(SlotManager slotManager) => LocalPlayerSlotManager = slotManager;

     
    }
}