using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// created scott 05/03
// altered jay 05/03
// altered Alexander Purvis 06/03
// altered again SJ 10/03


public class GoalManagerScript : MonoBehaviour
{
    // Vars ---------------------
    public enum Goal { Proud, Anxious, Content }

    [SerializeField] CurrentMood gardenScoreCalculator;

   // [SerializeField] GardenEmotionIndicatorControls pleasant;
    //[SerializeField] GardenEmotionIndicatorControls social;
    //[SerializeField] GardenEmotionIndicatorControls energy;
   



    Goal CurrentPlayerGoalEnumValue => GameManager.Instance.ActivePlayer.PlayerEnumValue == Player.PlayerEnum.Player0 ? GameManager.Instance.CurrentGoal : GameManager.Instance.AlternateGoal;
    MoodAttributes CurrentPlayerGoal => allGoals[CurrentPlayerGoalEnumValue];




    public Text goalDisplay;
    public TMP_Text UnpleasantPleasantDisplay;
    public TMP_Text PersonalSocialDisplay;
    public TMP_Text CalmEnergisedDisplay;


    Dictionary<Player.PlayerEnum, MoodAttributes> goalMood = new Dictionary<Player.PlayerEnum, MoodAttributes>();
    
    Dictionary<Goal, MoodAttributes> allGoals;

    private void Awake()
    {
        allGoals = new Dictionary<Goal, MoodAttributes>()
        {
             {Goal.Proud,   new MoodAttributes( 2, -1, -1 ) }, // Unpleasant/Pleasant, Personal/Social, Calm/Energised
             {Goal.Anxious, new MoodAttributes(-1, -1, 2 ) },
             {Goal.Content, new MoodAttributes( 1, 1, 2 ) }
        };
    }


    private void GetCurrentGoals()
    {
        var player1Goal = GameManager.Instance.CurrentGoal;
        var player2Goal = GameManager.Instance.AlternateGoal;

        goalMood[Player.PlayerEnum.Player0] = allGoals[player1Goal];
        goalMood[Player.PlayerEnum.Player1] = allGoals[player2Goal];


    }


    void UpdateText()
    {
        goalDisplay.text = $"<b>Goal: {CurrentPlayerGoalEnumValue.ToString()}</b>";

        UnpleasantPleasantDisplay.text = CurrentPlayerGoal.GetDisplayWithImage(MoodAttributes.Scales.Pleasance);
        PersonalSocialDisplay.text = CurrentPlayerGoal.GetDisplayWithImage(MoodAttributes.Scales.Sociability);
        CalmEnergisedDisplay.text = CurrentPlayerGoal.GetDisplayWithImage(MoodAttributes.Scales.Energy);
    }



    private void OnEnable()
    {
        EventsManager.BindEvent(EventsManager.EventType.NewTurnBegin, UpdateText);
        EventsManager.BindEvent(EventsManager.EventType.StartGame, GetCurrentGoals);
        EventsManager.BindEvent(EventsManager.EventType.UpdateScore, CheckIfWin);
    }

    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.NewTurnBegin, UpdateText);
        EventsManager.UnbindEvent(EventsManager.EventType.StartGame, GetCurrentGoals);
        EventsManager.UnbindEvent(EventsManager.EventType.UpdateScore, CheckIfWin);
    }


    // Update is called once per frame
    void Update()
    {
        GetCurrentGoals();
        UpdateText();

        CheckIfWin();
    }

 

    void CheckIfWin()
    {
        if ((GoalMatches(Player.PlayerEnum.Player0) && GoalMatches(Player.PlayerEnum.Player1)) || Input.GetKeyDown(KeyCode.Space))
        {
            EventsManager.InvokeEvent(EventsManager.EventType.GameOver);
        }
    }
    


    bool GoalMatches(Player.PlayerEnum player)
    {
        if(goalMood.Count == 0) return false; // goal not set

        var currentMood = gardenScoreCalculator.GetMoodValuesGardens();
        return currentMood[player] == goalMood[player]; // this is an overloaded operator
    }
}
