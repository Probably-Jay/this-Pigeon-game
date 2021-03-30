using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mood;

// created by Alexander Purvis 04/03
// Edited SJ 10/03
// Edited again Jay 10/03, 26/03  

/// <summary>
/// Manages the garden's emotions
/// </summary>
public class EmotionTracker : MonoBehaviour // re-named from DisplayManager
{


    private void OnEnable()
    {
        EventsManager.BindEvent(EventsManager.EventType.NewTurnBegin, CheckForAcheivedGoal);
    }

    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.NewTurnBegin, CheckForAcheivedGoal);
    }

    public Dictionary<Player.PlayerEnum, TraitValue> GardenCurrentTraits { get; } = new Dictionary<Player.PlayerEnum, TraitValue>()
    {
        {Player.PlayerEnum.Player1, TraitValue.Zero }
        ,{Player.PlayerEnum.Player2, TraitValue.Zero }
    };   
    
    public Dictionary<Player.PlayerEnum, TraitValue> GardenGoalTraits => new Dictionary< Player.PlayerEnum, TraitValue>()    
    {
        { Player.PlayerEnum.Player1, Emotion.EmotionValues[GameManager.Instance.Player1Goal]}
        ,{Player.PlayerEnum.Player2, Emotion.EmotionValues[GameManager.Instance.Player2Goal] }
            
    };

    public Dictionary<Player.PlayerEnum, Emotion.Emotions> GardenGoalEmotions => new Dictionary<Player.PlayerEnum, Emotion.Emotions>()
    {
        { Player.PlayerEnum.Player1, GameManager.Instance.Player1Goal }
        ,{Player.PlayerEnum.Player2, GameManager.Instance.Player2Goal }

    };


    public bool HasAcheivedGoal(Player.PlayerEnum player) => GardenCurrentTraits[player] >= GardenGoalTraits[player];

 


    public void AddToGardenStats(Player.PlayerEnum player, TraitValue traits)
    {
        GardenCurrentTraits[player] += traits;
        EventsManager.InvokeEvent(EventsManager.EventType.PlantAlterStats);
    }

    private void CheckForAcheivedGoal()
    {
        if (HasAcheivedGoal(Player.PlayerEnum.Player1)){
            EventsManager.InvokeEvent(EventsManager.ParameterEventType.AcheivedGoal, new EventsManager.EventParams() { EnumData = Player.PlayerEnum.Player1 });
        }

        if (HasAcheivedGoal(Player.PlayerEnum.Player2)){
            EventsManager.InvokeEvent(EventsManager.ParameterEventType.AcheivedGoal, new EventsManager.EventParams() { EnumData = Player.PlayerEnum.Player2 });
        }
    }

    public void SubtractFromGardenStats(Player.PlayerEnum player, TraitValue traits)
    {
        GardenCurrentTraits[player] -= traits;
        EventsManager.InvokeEvent(EventsManager.EventType.PlantAlterStats);
    }


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




}
