using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mood;
using System;
using NetSystem;
using System.Collections.ObjectModel;

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
        public Player.PlayerEnum LocalPlayerEnumID => OnlineTurnManager.LocalPlayer.EnumID;

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

        
        public bool InOwnGarden => PlayerWhosGardenIsCurrentlyVisible == LocalPlayerEnumID;

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
            EventsManager.BindEvent(EventsManager.EventType.EnterGameScene, EnterGame);
        }

        private void OnDisable()
        {
            EventsManager.UnbindEvent(EventsManager.EventType.EnterGameScene, EnterGame);
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
                Save();
            }
        }



        // player just entererd into the game
        void EnterGame()
        {
            DataManager = FindObjectOfType<DataManager>();


            ActiveGame netGame = NetworkHandler.Instance.NetGame;

            var context = netGame.CurrentNetworkGame.EnteredGameContext;

            Debug.Log(context.enumValue);

            if (context == null)
            {
                Debug.LogError("Error loading in");
                return;
            }

            if (context.createNewGame)
            {
                CreateNewGame(context);
            }
            else
            {
                ReloadGameOnly(context);
            }

            if (context.initialisePlayer)
            {
                InitialisePlayer(context);
            }
            else
            {
                ReloadPlayerOnly(context);
            }

            if (context.claimingTurn)
            {
                ClaimTurn(context);
            }

            SetLocalSlotManager();

            if (!context.createNewGame)
            {
                LoadGarden(context);
            }


    

            SaveGame();

            StartCoroutine(InvokeGameStartEventNextFrame(context));
        }

      

        private IEnumerator InvokeGameStartEventNextFrame(NetworkGame.EnterGameContext context)
        {
            yield return null;  // wait a frame

            switch (context.interactionState)
            {
                case NetworkGame.EnterGameContext.InteractionState.Playing:
                    Debug.Log("Playing");
                    EventsManager.InvokeEvent(EventsManager.EventType.ResumeGameOwnTurn);

                    break;
                case NetworkGame.EnterGameContext.InteractionState.Spectating:
                    Debug.Log("Spectating");
                    EventsManager.InvokeEvent(EventsManager.EventType.ResumeGameSpectating);
                    break;
            }

            if (context.initialisePlayer)
            {
                EventsManager.InvokeEvent(EventsManager.EventType.FirstTimeEnteringGame);
            }

            EventsManager.InvokeEvent(EventsManager.EventType.GameLoaded);
        }

        private void CreateNewGame(NetworkGame.EnterGameContext context)
        {
            Debug.Log("Creating game");

            OnlineTurnManager.CreateGame(context);
        }

        private void ReloadGameOnly(NetworkGame.EnterGameContext context)
        {
            Debug.Log("Reload game");

            OnlineTurnManager.ReloadGameOnly(context);
        }


        private void InitialisePlayer(NetworkGame.EnterGameContext context)
        {
            Debug.Log("Initilising player");

            OnlineTurnManager.CreatePlayer(context);
            EmotionTracker.InitialisePlayer(context);
        }


        private void ReloadPlayerOnly(NetworkGame.EnterGameContext context)
        {
            OnlineTurnManager.ReloadPlayer(context);
            EmotionTracker.ReLoadPlayer(context);
        }

        private void ClaimTurn(NetworkGame.EnterGameContext context)
        {
            OnlineTurnManager.ClaimTurn();
            context.interactionState = NetworkGame.EnterGameContext.InteractionState.Playing;
        }

        private void LoadGarden(NetworkGame.EnterGameContext context)
        {
            var data = NetworkHandler.Instance.NetGame.CurrentNetworkGame.usableData;
            foreach (GardenDataPacket.Plant plant in data.gardenData.newestGarden1)
            {
                int slot = (int)Player.PlayerEnum.Player1;
                SlotManagers[slot].ClearGarden();
                SlotManagers[slot].AddPlantFromServer(slot, plant);
            }  
            foreach (var plant in data.gardenData.newestGarden2)
            {
                int slot = (int)Player.PlayerEnum.Player2;
                SlotManagers[slot].ClearGarden();
                SlotManagers[slot].AddPlantFromServer(slot, plant);
            }
        }

        //private void EnterGamePlaying(NetworkGame.EnterGameContext context)
        //{
        //    Debug.Log("Playing");

        //   // OnlineTurnManager.ResumeGame(context);

        //    if (!context.createNewGame)
        //    {
        //       // LoadGameIntoScene(context);
        //    }

        //    // plants and stuff


        //   // EventsManager.InvokeEvent(EventsManager.EventType.ResumeGameOwnTurn);

        // //   throw new NotImplementedException();
        //}


        //private void EnterGameSpecting(NetworkGame.EnterGameContext context)
        //{
        //    Debug.Log("Spectating");

        //   // OnlineTurnManager.ResumeGame(context);

        //    if (!context.createNewGame)
        //    {
        //     //   LoadGameIntoScene(context);
        //    }

        //    // plants and stuff


        //    //EventsManager.InvokeEvent(EventsManager.EventType.ResumeGameSpectating);

        // //   throw new NotImplementedException();
        //}

        //private void LoadGameIntoScene(NetworkGame.EnterGameContext context)
        //{
        //    OnlineTurnManager.ReloadGame(context);
        //}
     

      

        private void SetLocalSlotManager()
        {
            LocalPlayerSlotManager = SlotManagers[(int)LocalPlayerEnumID];
        }

        ////private void ResumeGame()
        ////{

        ////    //bool firstTimeEnteringGame = OnlineTurnManager.TurnTracker.Turn == 1;
        ////    //if()

        ////    var game = NetworkHandler.Instance.NetGame.CurrentNetworkGame;

        ////    bool gameBegan = game.usableData.gameBegun;

        ////    Player.PlayerEnum playerWeAre = GetPlayerWeAre();




        ////    if (gameBegan)
        ////    {
        ////        ResumGameThatHasBegun(game, playerWeAre);
        ////    }
        ////    else
        ////    {
        ////        switch (playerWeAre)
        ////        {
        ////            case Player.PlayerEnum.Player1:
        ////                BeginNewGame();
        ////                break;
        ////            case Player.PlayerEnum.Player2:
        ////                break;
        ////        }
        ////    }


        ////}

        ////private void ResumGameThatHasBegun(NetworkGame game, Player.PlayerEnum playerWeAre)
        ////{
        ////    PlayerDataPacket playerData = game.usableData.playerData;


        ////    string netPlayerGameBelongsTo = game.usableData.gameStartedBy;
        ////    string player1ID = netPlayerGameBelongsTo;
        ////    string player2ID = playerData.player2ID;

        ////    Player.PlayerEnum playerWhoOwnsTurn = playerData.turnOwner == NetSystem.NetworkHandler.Instance.ClientEntity.Id ? playerWeAre : Player.OtherPlayer(playerWeAre);

        ////    bool turnComplete = game.usableData.turnComplete;
        ////    bool ourTurn = (playerWhoOwnsTurn == playerWeAre);

        ////    if (NetUtility.CanClaimTurn(turnComplete, ourTurn))
        ////    {
        ////        // claim turn
        ////        QuickClaimTurn(game);
        ////        turnComplete = false;
        ////        ourTurn = true;
        ////        playerWhoOwnsTurn = playerWeAre;

        ////    }

        ////    bool playing = NetSystem.NetUtility.CanTakeTurn(turnComplete, ourTurn);

        ////    if (playing)
        ////    {
        ////        ResumePlaying(playerWeAre, player2ID);
        ////    }
        ////    else
        ////    {
        ////        ResumeSpectating(playerWeAre, playerWhoOwnsTurn, player2ID);
        ////    }

        ////    // load the garden

        ////    EventsManager.InvokeEvent(EventsManager.EventType.GameLoaded);
        ////}

        ////private static void QuickClaimTurn(NetworkGame game)
        ////{
        ////    Debug.Log("Claiming turn");
        ////    game.usableData.playerData.turnOwner = NetSystem.NetworkHandler.Instance.ClientEntity.Id;
        ////    game.usableData.playerData.turnComplete = false;
        ////    game.usableData.NewTurn = true;
        ////}


        ////private void ResumePlaying(Player.PlayerEnum playerWeAre, string player2ID)
        ////{
        ////    Debug.Log("Resuming game playing");

        ////    var game = NetworkHandler.Instance.NetGame.CurrentNetworkGame;

        ////    if (playerWeAre == Player.PlayerEnum.Player1 && !game.usableData.gameBegun)  // game has not begun
        ////    {
        ////        BeginNewGame();
        ////        return;
        ////    }

        ////    if (playerWeAre == Player.PlayerEnum.Player2 && Player2NotInGame(player2ID))
        ////    {
        ////        OnlineTurnManager.JoinedGameNewPlaying(Player.PlayerEnum.Player2, Player.PlayerEnum.Player2);
        ////    }
        ////    else
        ////    {
        ////        OnlineTurnManager.ResumedGamePlaying(playerWeAre, playerWeAre);

        ////    }

        ////    EmotionTracker.ResumeGame(playerWeAre);

        ////    SetLocalSlotManager();

        ////    EventsManager.InvokeEvent(EventsManager.EventType.ResumeGameOwnTurn);
        ////}
        

        ////private void ResumeSpectating(Player.PlayerEnum playerWeAre,Player.PlayerEnum turnOwner, string player2ID)
        ////{
        ////    Debug.Log("Resuming game spectating");


        ////    // might be first time logging on
        ////    if (playerWeAre == Player.PlayerEnum.Player2 && Player2NotInGame(player2ID))
        ////    {
        ////        OnlineTurnManager.JoinedGameNewSpectator(Player.PlayerEnum.Player2, Player.PlayerEnum.Player2);
        ////    }
        ////    else
        ////    {
        ////        OnlineTurnManager.ResumedGameSpectating(playerWeAre, turnOwner);

        ////    }


        ////    EmotionTracker.ResumeGame(playerWeAre);

        ////    SetLocalSlotManager();

        ////    EventsManager.InvokeEvent(EventsManager.EventType.ResumeGameSpectating);
        ////}

        ////private bool Player2NotInGame(string player2ID)
        ////{
        ////    return player2ID == "" || player2ID == "NULL";
        ////}

        ////private Player.PlayerEnum GetPlayerWeAre()
        ////{
        ////    if (NetworkHandler.Instance.NetGame.CurrentNetworkGame.usableData.gameStartedBy == NetworkHandler.Instance.PlayerClient.ClientEntityKey.Id)
        ////    {
        ////        return Player.PlayerEnum.Player1;
        ////    }
        ////    else
        ////    {
        ////        return Player.PlayerEnum.Player2;
        ////    }
        ////}

        ////private void BeginNewGame()
        ////{
        ////    Debug.Log("Begin new game");

        ////    OnlineTurnManager.InitialiseNewGame();


        ////    EmotionTracker.InitialiseNewGame(NewGameMoodGoalTemp);

        ////    SetLocalSlotManager();

        ////    EventsManager.InvokeEvent(EventsManager.EventType.StartNewGame);
        ////    EventsManager.InvokeEvent(EventsManager.EventType.GameLoaded);
        ////    EventsManager.InvokeEvent(EventsManager.EventType.FirstTimeEnteringGame);
        ////}

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
                Save();
            }
        }

        private void Save()
        {
            //EventsManager.InvokeEvent(EventsManager.EventType.GatherSaveData);
            //GatherSaveData();
            EventsManager.InvokeEvent(EventsManager.EventType.OnSaveDataGathered);
        }

        //private void GatherSaveData()
        //{
        //    foreach(var player in Helper.Utility.GetEnumValues<Player.PlayerEnum>())
        //    {
        //        var plants = SlotManagers[(int)player].GetAllPlants();
        //        SavePlants(plants, player);
        //    }
        //}

        //private void SavePlants(ReadOnlyCollection<Plants.Plant> plants, Player.PlayerEnum player)
        //{
        //    foreach (var plant in plants)
        //    {
        //        switch (player)
        //        {
        //            case Player.PlayerEnum.Player1:
        //                DataManager.AddPlantToGarden1(
        //                    plantType: plant.name,
        //                    slotNumber: plant.StoredInSlot,
        //                    stage: plant.InternalGrowthStage,
        //                    watering: plant.RequiresAction(Plants.PlantActions.TendingActions.Watering),
        //                    spraying: plant.RequiresAction(Plants.PlantActions.TendingActions.Spraying),
        //                    trimming: plant.RequiresAction(Plants.PlantActions.TendingActions.Trimming)

        //                    ) ;
        //                break;
        //            case Player.PlayerEnum.Player2:
        //                break;

                
        //        }
        //    }
        //}


    }


}