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


public class GoalManagerScript : MonoBehaviour
{
  

    [SerializeField] CurrentMood gardenScoreCalculator;

 


    //Emotion.Emotions CurrentPlayerGoalEnumValue => GameManager.Instance.ActivePlayer.PlayerEnumValue == Player.PlayerEnum.Player1 ? GameManager.Instance.Player1Goal : GameManager.Instance.Player2Goal;
    //TraitValue CurrentPlayerGoal => Emotion.EmotionValues[CurrentPlayerGoalEnumValue];

    public TraitValue CurrentPlayerGoal => GetGoal(GameManager.Instance.ActivePlayer.PlayerEnumValue);
    public Emotion.Emotions CurrentPlayerGoalEnumValue => GetGoalEnum(GameManager.Instance.ActivePlayer.PlayerEnumValue);

    public Emotion.Emotions GetGoalEnum(Player.PlayerEnum player)
    {
        switch (player)
        {
            case Player.PlayerEnum.Player1: return GameManager.Instance.Player1Goal;

            case Player.PlayerEnum.Player2: return GameManager.Instance.Player1Goal;

            default: throw new System.ArgumentException();
                
        }
    }

    public TraitValue GetGoal(Player.PlayerEnum player) => Emotion.EmotionValues[GetGoalEnum(player)];


   // public readonly Dictionary


    public Text goalDisplay;
    public TMP_Text UnpleasantPleasantDisplay;
    public TMP_Text PersonalSocialDisplay;
    public TMP_Text CalmEnergisedDisplay;


    //Dictionary<Player.PlayerEnum, > goalMood = new Dictionary<Player.PlayerEnum, MoodAttributes>();
    
    //Dictionary<Goal, MoodAttributes> allGoals;

    private void Awake()
    {
        //allGoals = new Dictionary<Emotion.Emotions, MoodAttributes>()
        //{
        //     {Goal.Proud,   new MoodAttributes( 2, -1, -1 ) }, // Unpleasant/Pleasant, Personal/Social, Calm/Energised
        //     {Goal.Anxious, new MoodAttributes(-1, -1, 2 ) },
        //     {Goal.Content, new MoodAttributes( 1, 1, 2 ) }
        //};
    }


    //private void GetCurrentGoals()
    //{
    //    var player1Goal = GameManager.Instance.CurrentGoal;
    //    var player2Goal = GameManager.Instance.AlternateGoal;

    //    goalMood[Player.PlayerEnum.Player0] = allGoals[player1Goal];
    //    goalMood[Player.PlayerEnum.Player1] = allGoals[player2Goal];


    //}


    void UpdateText()
    {
        goalDisplay.text = $"<b>Goal: {CurrentPlayerGoalEnumValue.ToString()}</b>";

        //UnpleasantPleasantDisplay.text = CurrentPlayerGoal.GetDisplayWithImage(MoodAttributes.Scales.Pleasance);
        //PersonalSocialDisplay.text = CurrentPlayerGoal.GetDisplayWithImage(MoodAttributes.Scales.Sociability);
        //CalmEnergisedDisplay.text = CurrentPlayerGoal.GetDisplayWithImage(MoodAttributes.Scales.Energy);
    }



    private void OnEnable()
    {
        EventsManager.BindEvent(EventsManager.EventType.NewTurnBegin, UpdateText);
      //  EventsManager.BindEvent(EventsManager.EventType.StartGame, GetCurrentGoals);
        EventsManager.BindEvent(EventsManager.EventType.UpdateScore, CheckIfWin);
    }

    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.NewTurnBegin, UpdateText);
       // EventsManager.UnbindEvent(EventsManager.EventType.StartGame, GetCurrentGoals);
        EventsManager.UnbindEvent(EventsManager.EventType.UpdateScore, CheckIfWin);
    }


    // Update is called once per frame
    void Update()
    {
     //   GetCurrentGoals();
        UpdateText();

        CheckIfWin();
    }

 

    void CheckIfWin()
    {
        if ((GoalMatches(Player.PlayerEnum.Player1) && GoalMatches(Player.PlayerEnum.Player2)) || Input.GetKeyDown(KeyCode.Space))
        {
            EventsManager.InvokeEvent(EventsManager.EventType.GameOver);
        }
    }
    


    bool GoalMatches(Player.PlayerEnum player)
    {
        var currentMoods = gardenScoreCalculator.GardenMoods;
        return currentMoods[player] == GetGoal(player); // this is an overloaded operator
    }
}
