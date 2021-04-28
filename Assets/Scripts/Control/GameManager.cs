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

            OnlineTurnManager.EndTurn();


            SaveGame();
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
                switch (playerWeAre)
                {
                    case Player.PlayerEnum.Player1:
                        ResumePlayingPlayer1(player1ID);
                        break;
                    case Player.PlayerEnum.Player2:
                        ResumePlayingPlayer2(player2ID);
                        break;
                }
            }
            else
            {
                switch (playerWeAre)
                {
                    case Player.PlayerEnum.Player1:
                        ResumeSpectatingPlayer1(player1ID);
                        break;
                    case Player.PlayerEnum.Player2:
                        ResumeSpectatingPlayer2(player2ID);
                        break;
                }
            }



            //if (Playing)
            //{

            //    if (!game.usableData.gameBegun) // will always be player 1
            //    {
            //        BeginNewGame();
            //        return;
            //    }

            //    OnlineTurnManager.ResumedGamePlaying(playerWeAre);
            //    EmotionTracker.ResumeGame();
                        

            //    SetLocalSlotManager();

            //    EventsManager.InvokeEvent(EventsManager.EventType.ResumeGameOwnTurn);
            //}
            //else
            //{
            //    //var game = NetworkHandler.Instance.NetGame.CurrentNetworkGame;

            //    if (!game.usableData.gameBegun) // will always be player 2
            //    {
            //       // BeginNewGame();
            //        return;
            //    }

            //    if(playerWeAre == Player.PlayerEnum.Player2 && game.usableData.playerData.player2ID == null)
            //    {
            //        // player 2 just joined the game
            //    }


            //    SetLocalSlotManager();

            //    EventsManager.InvokeEvent(EventsManager.EventType.ResumeGameSpectating);
            //}
            //EventsManager.InvokeEvent(EventsManager.EventType.GameLoaded);

        }

        private static void QuickClaimTurn(NetworkGame game)
        {
            game.usableData.playerData.turnOwner = NetSystem.NetworkHandler.Instance.ClientEntity.Id;
            game.usableData.playerData.turnComplete = false;
        }


        private void ResumePlayingPlayer1(string player1ID)
        {
            var game = NetworkHandler.Instance.NetGame.CurrentNetworkGame;

            if (!game.usableData.gameBegun) // will always be player 1
            {
                BeginNewGame();
                return;
            }

            OnlineTurnManager.ResumedGamePlaying(Player.PlayerEnum.Player1, Player.PlayerEnum.Player1);
            EmotionTracker.ResumeGame(Player.PlayerEnum.Player1);

            SetLocalSlotManager();

            EventsManager.InvokeEvent(EventsManager.EventType.ResumeGameOwnTurn);

        }

        private void ResumePlayingPlayer2(string player2ID)
        {
            var game = NetworkHandler.Instance.NetGame.CurrentNetworkGame;

            if (!game.usableData.gameBegun) // will not happen
            {
                throw new Exception();
            }


            // might be first time logging on
            if(player2ID == "" || player2ID == "NULL")
            {

            }



        }

        private void ResumeSpectatingPlayer1(string player1ID)
        {
            throw new NotImplementedException();
        }

        private void ResumeSpectatingPlayer2(string player2ID)
        {
            throw new NotImplementedException();
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