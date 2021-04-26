using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mood;

// created scott 05/03
// altered jay 05/03
// altered Alexander Purvis 06/03
// altered again SJ 10/03
// refactored jay 26/03

// depracated Jay 02/04


[System.Obsolete("Replaced by " + nameof(GameCore.EmotionTracker),true)]
public class GoalManagerScript : MonoBehaviour
{
  

    //[SerializeField] EmotionTracker gardenScoreCalculator;



    //public TraitValue CurrentPlayerGoal => GetGoal(GameManager.Instance.ActivePlayerID);
    //public Emotion.Emotions CurrentPlayerGoalEnumValue => GetGoalEnum(GameManager.Instance.ActivePlayerID);

    //public Emotion.Emotions GetGoalEnum(Player.PlayerEnum player)
    //{
    //    switch (player)
    //    {
    //        case Player.PlayerEnum.Player1: return GameManager.Instance.Player1Goal;

    //        case Player.PlayerEnum.Player2: return GameManager.Instance.Player1Goal;

    //        default: throw new System.ArgumentException();
                
    //    }
    //}

    //public TraitValue GetGoal(Player.PlayerEnum player) => Emotion.EmotionValues[GetGoalEnum(player)];




    //public Text goalDisplay;
    //public TMP_Text UnpleasantPleasantDisplay;
    //public TMP_Text PersonalSocialDisplay;
    //public TMP_Text CalmEnergisedDisplay;


  

    //void UpdateText()
    //{
    //    goalDisplay.text = $"<b>Goal: {CurrentPlayerGoalEnumValue.ToString()}</b>";

    //    //UnpleasantPleasantDisplay.text = CurrentPlayerGoal.GetDisplayWithImage(MoodAttributes.Scales.Pleasance);
    //    //PersonalSocialDisplay.text = CurrentPlayerGoal.GetDisplayWithImage(MoodAttributes.Scales.Sociability);
    //    //CalmEnergisedDisplay.text = CurrentPlayerGoal.GetDisplayWithImage(MoodAttributes.Scales.Energy);
    //}



    //private void OnEnable()
    //{
    //    EventsManager.BindEvent(EventsManager.EventType.NewTurnBegin, UpdateText);
    //    EventsManager.BindEvent(EventsManager.EventType.UpdateScore, CheckIfWin);
    //}

    //private void OnDisable()
    //{
    //    EventsManager.UnbindEvent(EventsManager.EventType.NewTurnBegin, UpdateText);
    //    EventsManager.UnbindEvent(EventsManager.EventType.UpdateScore, CheckIfWin);
    //}


    //// Update is called once per frame
    //void Update()
    //{
    //    UpdateText();

    //    CheckIfWin();
    //}

 

    //void CheckIfWin()
    //{
    //    if ((GoalMatches(Player.PlayerEnum.Player1) && GoalMatches(Player.PlayerEnum.Player2)) || Input.GetKeyDown(KeyCode.Space))
    //    {
    //        EventsManager.InvokeEvent(EventsManager.EventType.GameOver);
    //    }
    //}
    


    //bool GoalMatches(Player.PlayerEnum player)
    //{
    //    var currentMoods = gardenScoreCalculator.GardenCurrentTraits;
    //    return currentMoods[player] == GetGoal(player); // this is an overloaded operator
    //}
}
