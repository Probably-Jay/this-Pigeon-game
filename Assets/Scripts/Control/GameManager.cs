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
        public Coroutine updateFromServerCoroutuine;

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


      





        // player just entererd into the game
        void EnterGame()
        {
            DataManager = new DataManager();


            ActiveGame netGame = NetworkHandler.Instance.NetGame;

            var context = netGame.CurrentNetworkGame.EnteredGameContext;

            Debug.Log(context.enumValue);

            if (context == null)
            {
                Debug.LogError("Error loading in");
                return;
            }

            Debug.LogWarning("a");
            if (context.createNewGame)
            {
                Debug.LogWarning("c");
                CreateNewGame(context);
                Debug.LogWarning("b");
            }
            else
            {
                ReloadGameOnly(context);
            }
            Debug.LogWarning("d");

            if (context.initialisePlayer)
            {
                Debug.LogWarning("e");

                InitialisePlayer(context);

                Debug.LogWarning("f");

            }
            else
            {
                Debug.LogWarning("g");

                ReloadPlayerOnly(context);

                Debug.LogWarning("h");

            }

            Debug.LogWarning("i");


            SetLocalSlotManager();

            Debug.LogWarning("j");

            if (!context.createNewGame)
            {
                LoadGarden();
            }

            if (context.claimingTurn)
            {
                ClaimTurn(context);
            }

    

            SaveGame();

            StartCoroutine(InvokeGameStartEventNextFrame(context));

            updateFromServerCoroutuine = StartCoroutine(UpdateFromServerCoroutine());
        }

   

        private IEnumerator InvokeGameStartEventNextFrame(NetworkGame.EnterGameContext context)
        {
            yield return null;  // wait a frame

            switch (context.interactionState)
            {
                case NetworkGame.EnterGameContext.InteractionState.Playing:
                    Debug.Log("Playing");
                    EventsManager.InvokeEvent(EventsManager.EventType.EnterPlayingState);

                    break;
                case NetworkGame.EnterGameContext.InteractionState.Spectating:
                    Debug.Log("Spectating");
                    EventsManager.InvokeEvent(EventsManager.EventType.EnterSpectatingState);
                    break;
            }

            if (context.initialisePlayer)
            {
                EventsManager.InvokeEvent(EventsManager.EventType.FirstTimeEnteringGame);
            }

            EventsManager.InvokeEvent(EventsManager.EventType.GameLoaded);
        }

        private IEnumerator UpdateFromServerCoroutine()
        {
            while (true)
            {
                if (!Spectating)
                {
                    yield return new WaitUntil(()=>Spectating);
                }


                APIOperationCallbacks<NetworkGame.UsableData> callbacks = new APIOperationCallbacks<NetworkGame.UsableData>
                    (
                        onSucess: (newData) =>
                        {
                            OnReceiveUpdateFromServerSucess(newData);
                        }
                        , onfailure: (e) =>
                        {
                            OnReceiveUpdateFromServerFailure(e);
                        }
                    );

                Debug.Log("Attempting update");
                NetworkHandler.Instance.ReceiveData(callbacks);

                yield return new WaitForSeconds(5);

            }

          

        }

        private void OnReceiveUpdateFromServerSucess(NetworkGame.UsableData newData)
        {
            NetGameDataDifferencesTracker.DataDifferences differences = NetworkHandler.Instance.NetGame.CurrentNetworkGame.DataDifferences.CompareNewGameData(newData);

            if (!differences.AnyDifferences)
            {
                Debug.Log("No differences");
                return;
            }

            ApplyNewData(newData, differences);

            bool claimedTurn = AttemptToClaimTurn();

            if (!claimedTurn)
            {
                return;
            }

            EventsManager.InvokeEvent(EventsManager.EventType.EnterPlayingState);
        }

        private static void OnReceiveUpdateFromServerFailure(FailureReason e)
        {
            Debug.LogError($"Receive Data failure {e}");
        }

        private void ApplyNewData(NetworkGame.UsableData newData, NetGameDataDifferencesTracker.DataDifferences differences)
        {
            NetworkHandler.Instance.NetGame.CurrentNetworkGame.SetGameData(newData);

            Debug.Log("Applying new data");

            ClearLocalDataManager();

            HotRealoadPlayeData();
            LoadGarden();

            PlayChangesGardenEffects(differences);

            NetworkHandler.Instance.NetGame.CurrentNetworkGame.DataDifferences.ResetGameDataDifferences();
        }

        private void ClearLocalDataManager()
        {
            DataManager = new DataManager();
        }

        //private void HotReloadGarden()
        //{
        //    throw new NotImplementedException();
        //}

        private void HotRealoadPlayeData()
        {
            OnlineTurnManager.HotRealoadPlayerData();
            EmotionTracker.HotRealoadPlayerData();
        }

        private void PlayChangesGardenEffects(NetGameDataDifferencesTracker.DataDifferences differences)
        {
            foreach (var palntDifferences in differences.plantDifferences)
            {
                switch (palntDifferences.garden)
                {
                    case Player.PlayerEnum.Player1:
                       // SlotManagers[0].SignalDifferenceToSlot(palntDifferences);
                        break;
                    case Player.PlayerEnum.Player2:
                       // SlotManagers[1].SignalDifferenceToSlot(palntDifferences);
                        break;

                }
            }
        }

        private bool AttemptToClaimTurn()
        {
            if (OnlineTurnManager.TurnTracker.CanClaimTurn)
            {
                OnlineTurnManager.ClaimTurn();
                return true;
            }
            return false;
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

        private void LoadGarden()
        {
            var data = NetworkHandler.Instance.NetGame.CurrentNetworkGame.CurrentGameData;
            SlotManagers[(int)Player.PlayerEnum.Player1].ClearGarden();
            SlotManagers[(int)Player.PlayerEnum.Player2].ClearGarden();
            foreach (var plant in data.gardenData.newestGarden1)
            {
                int garden = (int)Player.PlayerEnum.Player1;
                SlotManagers[garden].AddPlantFromServer(plant.slotNumber, plant);
            }  
            foreach (var plant in data.gardenData.newestGarden2)
            {
                int garden = (int)Player.PlayerEnum.Player2;
                SlotManagers[garden].AddPlantFromServer(plant.slotNumber, plant);
            }
        }

      

        private void SetLocalSlotManager()
        {
            LocalPlayerSlotManager = SlotManagers[(int)LocalPlayerEnumID];
        }



        public void RegisterLocalSlotManager(SlotManager slotManager, Player.PlayerEnum gardenplayerID)
        {
            //LocalPlayerSlotManager = slotManager;
            SlotManagers[(int)gardenplayerID] = slotManager;
        }

        public void EndTurn()
        {
            if (Spectating)
            {
                return;
            }

            OnlineTurnManager.EndTurn();

            EventsManager.InvokeEvent(EventsManager.EventType.EnterSpectatingState);

            Save();


        }

        public void QuitToMenu()
        {
            SaveGame();
           // StopAllCoroutines();
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
            EventsManager.InvokeEvent(EventsManager.EventType.SaveGame);
            NetworkHandler.Instance.NetGame.CurrentNetworkGame.UpdateCurrentData(DataManager);
        }

     

    }


}