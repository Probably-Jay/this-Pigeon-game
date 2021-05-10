using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mood;
using NetSystem;

// created by Alexander Purvis 04/03
// Edited SJ 10/03
// Edited again Jay 10/03, 26/03  
// converted to non-hotseat jay 26/04

namespace GameCore
{
    /// <summary>
    /// Manages the garden's emotions
    /// </summary>
    public class EmotionTracker : MonoBehaviour // re-named from DisplayManager
    {
        
        private bool otherPlayerHasWon;

        private void OnEnable()
        {
            EventsManager.BindEvent(EventsManager.EventType.GameLoaded, UpdateGardenStats);
            EventsManager.BindEvent(EventsManager.EventType.PlantChangedStats, UpdateGardenStats);
            EventsManager.BindEvent(EventsManager.EventType.PlacedOwnObject, UpdateGardenStats);
        }

        private void OnDisable()
        {
            EventsManager.UnbindEvent(EventsManager.EventType.GameLoaded, UpdateGardenStats);
            EventsManager.UnbindEvent(EventsManager.EventType.PlantChangedStats, UpdateGardenStats);
            EventsManager.UnbindEvent(EventsManager.EventType.PlacedOwnObject, UpdateGardenStats);
        }

        ///// <summary>
        ///// The mood given by the current state of the garden, re-calculated when a mood changes
        ///// </summary>
        //public Dictionary<Player.PlayerEnum, TraitValue> GardenCurrentTraits { get; } = new Dictionary<Player.PlayerEnum, TraitValue>()
        //{
        //    {Player.PlayerEnum.Player1, TraitValue.Uninitialised }
        //    ,{Player.PlayerEnum.Player2, TraitValue.Uninitialised }
        //};

        ///// <summary>
        ///// The trait values of the goal emotions of the gardens, set on instance creation based on <see cref="GameManager.Player1Goal"/>, <see cref="GameManager.Player2Goal"/>
        ///// </summary>
        //public Dictionary<Player.PlayerEnum, TraitValue> GardenGoalTraits => new Dictionary<Player.PlayerEnum, TraitValue>()
        //{
        //    { Player.PlayerEnum.Player1, Emotion.EmotionValues[GameManager.Instance.Player1Goal]}
        //    ,{Player.PlayerEnum.Player2, Emotion.EmotionValues[GameManager.Instance.Player2Goal] }

        //};

        ///// <summary>
        ///// The goal emotions of the gardens, set on instance creation based on <see cref="GameManager.Player1Goal"/>, <see cref="GameManager.Player2Goal"/>
        ///// </summary>
        //public Dictionary<Player.PlayerEnum, Emotion.Emotions> GardenGoalEmotions => new Dictionary<Player.PlayerEnum, Emotion.Emotions>()
        //{
        //    { Player.PlayerEnum.Player1, GameManager.Instance.Player1Goal }
        //    ,{Player.PlayerEnum.Player2, GameManager.Instance.Player2Goal }

        //};

        public Emotion EmotionGoal { get; private set; } 
        public TraitValue CurrentGardenTraits { get; private set; } = TraitValue.Uninitialised;


        // public bool HasAcheivedGoal(Player.PlayerEnum player) => GardenCurrentTraits[player] >= GardenGoalTraits[player];

        public bool PlayerHasAcheivedGoal() => CurrentGardenTraits >= EmotionGoal.traits;



        private void CheckForAcheivedGoal()
        {
            if (PlayerHasAcheivedGoal())
            {
                Debug.LogError("Won");
                EventsManager.InvokeEvent(EventsManager.ParameterEventType.AcheivedGoal, new EventsManager.EventParams() { EnumData1 = GameManager.Instance.LocalPlayerEnumID });
                switch (GameManager.Instance.LocalPlayerEnumID)
                {
                    case Player.PlayerEnum.Player1:
                        GameManager.Instance.DataManager.SetPlayer1AchivedMood(true);
                        break;
                    case Player.PlayerEnum.Player2:
                        GameManager.Instance.DataManager.SetPlayer2AchivedMood(true);
                        break;
                }
            }

            //if (HasAcheivedGoal(Player.PlayerEnum.Player1))
            //{
            //    EventsManager.InvokeEvent(EventsManager.ParameterEventType.AcheivedGoal, new EventsManager.EventParams() { EnumData = Player.PlayerEnum.Player1 });
            //}

            //if (HasAcheivedGoal(Player.PlayerEnum.Player2))
            //{
            //    EventsManager.InvokeEvent(EventsManager.ParameterEventType.AcheivedGoal, new EventsManager.EventParams() { EnumData = Player.PlayerEnum.Player2 });
            //}
        }

        //public void InitialiseNewGame(int newGameMoodGoalTemp)
        //{
        //    EmotionGoal = new Emotion((Mood.Emotion.Emotions)newGameMoodGoalTemp);

        //    switch (GameCore.GameManager.Instance.LocalPlayerID)
        //    {
        //        case Player.PlayerEnum.Player1:
        //            GameCore.GameManager.Instance.DataManager.SetPlayer1GoalMood(newGameMoodGoalTemp);
        //            break;
        //        case Player.PlayerEnum.Player2:
        //            GameCore.GameManager.Instance.DataManager.SetPlayer2GoalMood(newGameMoodGoalTemp);
        //            break;
        //    }
        //}

        public void InitialisePlayer(NetworkGame.EnterGameContext context)
        {
            EmotionGoal = new Emotion(context.emotion);
            switch (GameCore.GameManager.Instance.LocalPlayerEnumID)
            {
                case Player.PlayerEnum.Player1:
                    GameCore.GameManager.Instance.DataManager.SetPlayer1GoalMood((int)EmotionGoal.enumValue);
                    break;
                case Player.PlayerEnum.Player2:
                    GameCore.GameManager.Instance.DataManager.SetPlayer2GoalMood((int)EmotionGoal.enumValue);
                    // set play 1s
                    var data = NetSystem.NetworkHandler.Instance.NetGame.CurrentNetworkGame.CurrentGameData.playerData;
                    GameManager.Instance.DataManager.SetPlayer1GoalMood(data.player1ChosenMood);
                    otherPlayerHasWon = data.player1MoodAchieved;
                    break;
            }
    
        }

        public void ReLoadPlayer(NetworkGame.EnterGameContext context)
        {
            var data = NetSystem.NetworkHandler.Instance.NetGame.CurrentNetworkGame.CurrentGameData.playerData;
            switch (context.playerWeAre)
            {
                case Player.PlayerEnum.Player1:
                    EmotionGoal = new Emotion((Mood.Emotion.Emotions)data.player1ChosenMood);
                    break;
                case Player.PlayerEnum.Player2:
                    EmotionGoal = new Emotion((Mood.Emotion.Emotions)data.player2ChosenMood);
                    break;
            }
            GameManager.Instance.DataManager.SetPlayer1GoalMood(data.player1ChosenMood);
            GameManager.Instance.DataManager.SetPlayer2GoalMood(data.player2ChosenMood);

            GameManager.Instance.DataManager.SetPlayer1AchivedMood(data.player1MoodAchieved);
            GameManager.Instance.DataManager.SetPlayer2AchivedMood(data.player2MoodAchieved);

            // GameManager.Instance.LocalPlayer. = data.player1MoodAchieved;
            switch (GameManager.Instance.LocalPlayerEnumID)
            {
                case Player.PlayerEnum.Player1:
                    otherPlayerHasWon = data.player2MoodAchieved;
                    break;
                case Player.PlayerEnum.Player2:
                    otherPlayerHasWon = data.player1MoodAchieved;
                    break;
            }

        }



        internal void HotRealoadPlayerData()
        {
            var data = NetSystem.NetworkHandler.Instance.NetGame.CurrentNetworkGame.CurrentGameData.playerData;

            GameManager.Instance.DataManager.SetPlayer1GoalMood(data.player1ChosenMood);
            GameManager.Instance.DataManager.SetPlayer2GoalMood(data.player2ChosenMood);

            GameManager.Instance.DataManager.SetPlayer1AchivedMood(data.player1MoodAchieved);
            GameManager.Instance.DataManager.SetPlayer2AchivedMood(data.player2MoodAchieved);

            // p1won = data.player1MoodAchieved;
            switch (GameManager.Instance.LocalPlayerEnumID)
            {
                case Player.PlayerEnum.Player1:
                    otherPlayerHasWon = data.player2MoodAchieved;
                    break;
                case Player.PlayerEnum.Player2:
                    otherPlayerHasWon = data.player1MoodAchieved;
                    break;
            }
        }

        public void UpdateGardenStats()
        {
            var newGardenState = GetLocalGardenStats();

            if(CurrentGardenTraits == newGardenState)
            {
                CheckForAcheivedGoal();
                if ((GameManager.Instance.LocalPlayer.HasAcheivedGoal && otherPlayerHasWon))
                {
                    EventsManager.InvokeEvent(EventsManager.EventType.GameOver);
                }
                return; // we do not need to signal update as this is already up to date
            }


            CurrentGardenTraits = newGardenState;
            CheckForAcheivedGoal();

            EventsManager.InvokeEvent(EventsManager.EventType.GardenStatsUpdated);


            if ((GameManager.Instance.LocalPlayer.HasAcheivedGoal && otherPlayerHasWon))
            {
                EventsManager.InvokeEvent(EventsManager.EventType.GameOver);
            }
   

        }

        private void Update() {
            //if (Input.GetKeyDown(KeyCode.P))
            //{
            //    EventsManager.InvokeEvent(EventsManager.EventType.GameOver);
            //}
        }

        private TraitValue GetLocalGardenStats()
        {
            TraitValue value = TraitValue.Zero;
            var plants = GameManager.Instance.LocalPlayerSlotManager.GetAllPlants();

            foreach (var plant in plants)
            {
                value += plant.Traits;
            }

            return value;
        }

        public void ResetGardenStats()
        {
            CurrentGardenTraits = TraitValue.Zero;
            //GardenCurrentTraits[Player.PlayerEnum.Player1] = TraitValue.Zero;
            //GardenCurrentTraits[Player.PlayerEnum.Player2] = TraitValue.Zero;
        }

    }
}

    //public void AddToGardenStats(Player.PlayerEnum player, TraitValue traits)
    //{
    //    GardenCurrentTraits[player] += traits;
    //    EventsManager.InvokeEvent(EventsManager.EventType.PlantAlterStats);
    //}


    //public void SubtractFromGardenStats(Player.PlayerEnum player, TraitValue traits)
    //{
    //    GardenCurrentTraits[player] -= traits;
    //    EventsManager.InvokeEvent(EventsManager.EventType.PlantAlterStats);
    //}


    //TMP_Text displayText;


    ////public TMP_Text P1PleasanceDisplay;
    ////public TMP_Text P1SociabilityDisplay;
    ////public TMP_Text P1EnergyTextDisplay;

    ////public TMP_Text P2PleasanceDisplay;
    ////public TMP_Text P2SociabilityDisplay;
    ////public TMP_Text P2EnergyTextDisplay;


    //private void Awake()
    //{
    //    displayText = GetComponent<TMP_Text>();
    //}

    ////Start is called before the first frame update
    //void Start()
    //{
    //    DisplayCurrentGardenEmotion();
    //}





    //void DisplayCurrentGardenEmotion()
    //{
    //   // displayText.text = $"P1:\n\nP2:";

    //    //P1PleasanceDisplay.text = gardenMood1.GetDisplayWithImage(MoodAttributes.Scales.Pleasance);
    //    //P2PleasanceDisplay.text = gardenMood2.GetDisplayWithImage(MoodAttributes.Scales.Pleasance);

    //    //P1SociabilityDisplay.text = gardenMood1.GetDisplayWithImage(MoodAttributes.Scales.Sociability);
    //    //P2SociabilityDisplay.text = gardenMood2.GetDisplayWithImage(MoodAttributes.Scales.Sociability);

    //    //P1EnergyTextDisplay.text = gardenMood1.GetDisplayWithImage(MoodAttributes.Scales.Energy);
    //    //P2EnergyTextDisplay.text = gardenMood2.GetDisplayWithImage(MoodAttributes.Scales.Energy);

    //    EventsManager.InvokeEvent(EventsManager.EventType.UpdateScore);
    //}





