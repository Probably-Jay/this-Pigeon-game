using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mood;
using System;
using NetSystem;

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

        SlotManager[] SlotManagers = new SlotManager[2];

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


            EndTurnAndSaveGame();
        }

        private void EndTurnAndSaveGame()
        {
            if (!Spectating)
            {
                OnlineTurnManager.EndTurn();
                EventsManager.InvokeEvent(EventsManager.EventType.SaveGatheredData);
            }
        }

        void BeginOrResumeGame()
        {
            DataManager = FindObjectOfType<DataManager>();


            ActiveGame netGame = NetworkHandler.Instance.NetGame;
            if (netGame.CurrentNetworkGame.NewGameJustCreated)
            {
                BeginNewGame();
                return;
            }

            ResumeGame();


        
        }

        private void SetLocalSlotManager()
        {
            LocalPlayerSlotManager = SlotManagers[(int)LocalPlayerID];
        }

        private void ResumeGame()
        {

            //bool firstTimeEnteringGame = OnlineTurnManager.TurnTracker.Turn == 1;
            //if()

            var game = NetworkHandler.Instance.NetGame.CurrentNetworkGame;
            
            string netPlayerGameBelongsTo = game.usableData.gameStartedBy;
            string player1ID = netPlayerGameBelongsTo;
            string player2ID = game.usableData.playerData.player2ID;

            Player.PlayerEnum playerWeAre = GetPlayerWeAre();
            Player.PlayerEnum playerWhoOwnsTurn = game.usableData.playerData.turnOwner == NetSystem.NetworkHandler.Instance.ClientEntity.Id ? playerWeAre : Player.OtherPlayer(playerWeAre);

            bool turnComplete = game.usableData.turnComplete;
            bool ourTurn = (playerWhoOwnsTurn == playerWeAre);

            if (NetUtility.CanClaimTurn(turnComplete, ourTurn))
            {
                // claim turn
                QuickClaimTurn(game);
                turnComplete = false;
                ourTurn = true;
                playerWhoOwnsTurn =  playerWeAre ;

            }

            bool playing = NetSystem.NetUtility.CanTakeTurn(turnComplete, ourTurn);

            if (playing)
            {
                ResumePlaying(playerWeAre, player2ID);
            }
            else
            {
                ResumeSpectating(playerWeAre, playerWhoOwnsTurn, player2ID);
            }

            // load the garden

            EventsManager.InvokeEvent(EventsManager.EventType.GameLoaded);

        }

        private static void QuickClaimTurn(NetworkGame game)
        {
            Debug.Log("Claiming turn");
            game.usableData.playerData.turnOwner = NetSystem.NetworkHandler.Instance.ClientEntity.Id;
            game.usableData.playerData.turnComplete = false;
            game.usableData.NewTurn = true;
        }


        private void ResumePlaying(Player.PlayerEnum playerWeAre, string player2ID)
        {
            Debug.Log("Resuming game playing");

            var game = NetworkHandler.Instance.NetGame.CurrentNetworkGame;

            if (playerWeAre == Player.PlayerEnum.Player1 && !game.usableData.gameBegun)  // game has not begun
            {
                BeginNewGame();
                return;
            }

            if (playerWeAre == Player.PlayerEnum.Player2 && Player2NotInGame(player2ID))
            {
                OnlineTurnManager.JoinedGameNewPlaying(Player.PlayerEnum.Player2, Player.PlayerEnum.Player2);
            }
            else
            {
                OnlineTurnManager.ResumedGamePlaying(playerWeAre, playerWeAre);

            }

            EmotionTracker.ResumeGame(playerWeAre);

            SetLocalSlotManager();

            EventsManager.InvokeEvent(EventsManager.EventType.ResumeGameOwnTurn);
        }
        

        private void ResumeSpectating(Player.PlayerEnum playerWeAre,Player.PlayerEnum turnOwner, string player2ID)
        {
            Debug.Log("Resuming game spectating");


            // might be first time logging on
            if (playerWeAre == Player.PlayerEnum.Player2 && Player2NotInGame(player2ID))
            {
                OnlineTurnManager.JoinedGameNewSpectator(Player.PlayerEnum.Player2, Player.PlayerEnum.Player2);
            }
            else
            {
                OnlineTurnManager.ResumedGameSpectating(playerWeAre, turnOwner);

            }


            EmotionTracker.ResumeGame(playerWeAre);

            SetLocalSlotManager();

            EventsManager.InvokeEvent(EventsManager.EventType.ResumeGameSpectating);
        }

        private bool Player2NotInGame(string player2ID)
        {
            return player2ID == "" || player2ID == "NULL";
        }

        private Player.PlayerEnum GetPlayerWeAre()
        {
            if (NetworkHandler.Instance.NetGame.CurrentNetworkGame.usableData.gameStartedBy == NetworkHandler.Instance.PlayerClient.ClientEntityKey.Id)
            {
                return Player.PlayerEnum.Player1;
            }
            else
            {
                return Player.PlayerEnum.Player2;
            }
        }

        private void BeginNewGame()
        {
            Debug.Log("Begin new game");

            OnlineTurnManager.InitialiseNewGame();


            EmotionTracker.InitialiseNewGame(NewGameMoodGoalTemp);

            SetLocalSlotManager();

            EventsManager.InvokeEvent(EventsManager.EventType.StartNewGame);
            EventsManager.InvokeEvent(EventsManager.EventType.GameLoaded);
            EventsManager.InvokeEvent(EventsManager.EventType.FirstTimeEnteringGame);
        }

        // public void EndTurn() => EventsManager.InvokeEvent(EventsManager.EventType.EndTurn);

        // public void RegisterSlotManager(Player.PlayerEnum player, SlotManager slotManager) => SlotManagers.Add(player, slotManager);
        // public void UnregisterSlotManager(Player.PlayerEnum player) => SlotManagers.Remove(player);

        public void RegisterLocalSlotManager(SlotManager slotManager, Player.PlayerEnum gardenplayerID)
        {
            //LocalPlayerSlotManager = slotManager;
            SlotManagers[(int)gardenplayerID] = slotManager;
        }

        public void QuitToMenu()
        {
            SaveGame();
            NetSystem.NetworkHandler.Instance.LogoutPlayer();
            SceneChangeController.Instance.ChangeScene(SceneChangeController.Scenes.MainMenu);
        }

        public void SaveGame()
        {
            if (!Spectating)
            {
                EventsManager.InvokeEvent(EventsManager.EventType.SaveGatheredData);
            }
        }
    }
}