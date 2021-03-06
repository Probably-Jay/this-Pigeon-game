﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// created scott 05/03
// altered jay 05/03
// altered Alexander Purvis 06/03


public class GoalManagerScript : MonoBehaviour
{
    // Vars ---------------------
    public enum Goal { Proud, Anxious, Content }

    [SerializeField] DisplayManager gardenScoreCalculator;

    [SerializeField] GardenEmotionIndicatorControls pleasant;
    [SerializeField] GardenEmotionIndicatorControls social;
    [SerializeField] GardenEmotionIndicatorControls energy;
   



    Goal CurrentPlayerGoal => GameManager.Instance.ActivePlayer.PlayerEnumValue == Player.PlayerEnum.Player0 ? GameManager.Instance.CurrentGoal : GameManager.Instance.AlternateGoal;
    int[] CurrentPlayerGoalValues => allGoals[CurrentPlayerGoal];


    int g1v;
    int g2v;

    public Text goalDisplay;
    public Text UnpleasantPleasantDisplay;
    public Text PersonalSocialDisplay;
    public Text CalmEnergisedDisplay;


    Dictionary<Player.PlayerEnum, int[]> goalMood = new Dictionary<Player.PlayerEnum, int[]>();
    
    Dictionary<Goal, int[]> allGoals;

    private void Awake()
    {
        allGoals = new Dictionary<Goal, int[]>()
        {
             {Goal.Proud,   new int[3]{ 2, -1, -1 } }, // Unpleasant/Pleasant, Personal/Social, Calm/Energised
             {Goal.Anxious, new int[3]{-1, -1, 2 } },
             {Goal.Content, new int[3]{ 1, 1, 2 } }
        };
    }


    private void GetCurrentGoals()
    {
        var player1Goal = GameManager.Instance.CurrentGoal;
        var player2Goal = GameManager.Instance.AlternateGoal;

        goalMood[Player.PlayerEnum.Player0] = allGoals[player1Goal];
        goalMood[Player.PlayerEnum.Player1] = allGoals[player2Goal];


        // Unpleasant/Pleasant
        if (CurrentPlayerGoalValues[0] < 0)
        {
            pleasant.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.LeftOfScale);
        }
        else if (CurrentPlayerGoalValues[0] > 0)
        {
            pleasant.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.RightOfScale);
        }
        else if (CurrentPlayerGoalValues[0] == 0)
        {
            pleasant.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.Neutral);
        }


        // Personal/Social
        if (CurrentPlayerGoalValues[1] < 0)
        {
            social.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.LeftOfScale);
        }
        else if (CurrentPlayerGoalValues[1] > 0)
        {
            social.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.RightOfScale);
        }
        else if (CurrentPlayerGoalValues[1] == 0)
        {
            social.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.Neutral);
        }


        // Calm/Energised
        if (CurrentPlayerGoalValues[2] < 0)
        {
            energy.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.LeftOfScale);
        }
        else if (CurrentPlayerGoalValues[2] > 0)
        {
            energy.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.RightOfScale);
        }
        else if (CurrentPlayerGoalValues[2] == 0)
        {
            energy.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.Neutral);
        }
    }


    [System.Obsolete("Replaced by " + nameof(GetCurrentGoals), true)]
    void GetGoals()  
    {
        //g1v = (int)GameManager.Instance.CurrentGoal;
        //g2v = (int)GameManager.Instance.AlternateGoal;

        //goalG1 = (Goal)g1v;
        //goalG2 = (Goal)g2v;

        //switch (goalG1)
        //{
        //    case Goal.Proud:                       // Proud
        //        goalMood[0, 0] = 1;
        //        goalMood[0, 1] = 3;
        //        goalMood[0, 2] = 0;
        //        break;

        //    case Goal.Anxious:                     // Anxious
        //        goalMood[0, 0] = 1;
        //        goalMood[0, 1] = 0;
        //        goalMood[0, 2] = 3;
        //        break;

        //    case Goal.Content:                     // Content
        //        goalMood[0, 0] = 0;
        //        goalMood[0, 1] = 2;
        //        goalMood[0, 2] = 2;
        //        break;            
        //}

        //switch (goalG2)
        //{
        //    case Goal.Proud:                     // Proud
        //        goalMood[1, 0] = 1;
        //        goalMood[1, 1] = 3;
        //        goalMood[1, 2] = 0;
        //        break;

        //    case Goal.Anxious:                     // Anxious
        //        goalMood[1, 0] = 1;
        //        goalMood[1, 1] = 0;
        //        goalMood[1, 2] = 3;
        //        break;

        //    case Goal.Content:                     // Content
        //        goalMood[1, 0] = 0;
        //        goalMood[1, 1] = 2;
        //        goalMood[1, 2] = 2;
        //        break;
        //}
    }

    void UpdateText()
    {
        var goal = CurrentPlayerGoalValues;



        // set Unpleasant/Pleasant text
        string UnpleasantPleasant_Text = "      ";

        if (goal[0] < 0)
        {
            UnpleasantPleasant_Text = "Unpleasant";
        }
        else if (goal[0] > 0)
        {
            UnpleasantPleasant_Text = "Pleasant";
        }
        else if (goal[0] == 0)
        {
            UnpleasantPleasant_Text = "Neutral";
        }

        // set Personal/Social text

        string PersonalSocial_Text = " "; 

        if (goal[1] < 0)
        {
            PersonalSocial_Text = "Personal";
        }
        else if (goal[1] > 0)
        {
            PersonalSocial_Text = "Social";
        }
        else if (goal[1] == 0)
        {
            PersonalSocial_Text = "Neutral";
        }

        // set Calm/Energised text

        string CalmEnergised_Text = " "; 

        if (goal[2] < 0)
        {
            CalmEnergised_Text = "Calm";
        }
        else if (goal[2] > 0)
        {
            CalmEnergised_Text = "Energised";
        }
        else if (goal[2] == 0)
        {
            CalmEnergised_Text = "Neutral";
        }

        goalDisplay.text = $"<b>Goal: {CurrentPlayerGoal.ToString()}</b>";

        UnpleasantPleasantDisplay.text = GetDisplayText(UnpleasantPleasant_Text, goal[0]);
        PersonalSocialDisplay.text = GetDisplayText(PersonalSocial_Text, goal[1]);
        CalmEnergisedDisplay.text =  GetDisplayText(CalmEnergised_Text, goal[2]);
    }




    string GetDisplayText(string srt1, int value)
    {
        
            return $" <b>{srt1}</b> {Mathf.Abs(value).ToString()}";    
    }


    private string GetDisplayValue(int value)
    {
        return Mathf.Abs(value).ToString();
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
        // Move to Events for later build
       // GetGoals();
        GetCurrentGoals();
        UpdateText();

        CheckIfWin();
    }

 

    void CheckIfWin()
    {
        if (MoodMatches() || Input.GetKeyDown(KeyCode.Space))
        {
            EventsManager.InvokeEvent(EventsManager.EventType.GameOver);
        }
    }


    bool MoodMatches()
    {
        if(goalMood.Count == 0) return false;
        var curmood = gardenScoreCalculator.GetMoodValuesGardens();
        bool win = false;
        foreach (Player.PlayerEnum player in System.Enum.GetValues(typeof(Player.PlayerEnum)))
        {
            if (win) return true;
            win = true;
            for (int i = 0; i < goalMood[player].Length; i++)
            { 
                if(curmood[player][i] != CurrentPlayerGoalValues[i])
                {
                    win = false;
                }
            }
        }

        return false; // if reached here then no match
    }
}
