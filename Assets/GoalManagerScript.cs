﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// created scott 05/03
// altered jay 05/03

public class GoalManagerScript : MonoBehaviour
{
    // Vars ---------------------
    public enum Goal { Proud, Anxious, Content }

    [SerializeField] DisplayManager gardenScoreCalculator;

    [SerializeField] GardenEmotionIndicatorControls plasant;
    [SerializeField] GardenEmotionIndicatorControls social;
    [SerializeField] GardenEmotionIndicatorControls energy;


    Goal CurentPlayerGoal => GameManager.Instance.ActivePlayer.PlayerEnumValue == Player.PlayerEnum.Player0 ? GameManager.Instance.CurrentGoal : GameManager.Instance.AlternateGoal;
    int[] CurentPlayerGoalValues => allGoals[CurentPlayerGoal];



    int g1v;
    int g2v;

    public Text goalDisplay;

    Dictionary<Player.PlayerEnum, int[]> goalMood = new Dictionary<Player.PlayerEnum, int[]>();
    /// int[,] curMood = new int[2, 3];


    Dictionary<Goal, int[]> allGoals;

    private void Awake()
    {
        allGoals = new Dictionary<Goal, int[]>()
        {
             {Goal.Proud,   new int[3]{2,-1,-1 } } // pleasant, energy, social
            ,{Goal.Anxious, new int[3]{-1,2,-1 } }
            ,{Goal.Content, new int[3]{1,2,1 } }
        };
    }

    private void GeCurrentGoals()
    {
        var player1Goal = GameManager.Instance.CurrentGoal;
        var player2Goal = GameManager.Instance.AlternateGoal;

        goalMood[Player.PlayerEnum.Player0] = allGoals[player1Goal];
        goalMood[Player.PlayerEnum.Player1] = allGoals[player2Goal];

        plasant.UpdateIndicator(CurentPlayerGoalValues[0]);
        social.UpdateIndicator(CurentPlayerGoalValues[1]);
        energy.UpdateIndicator(CurentPlayerGoalValues[2]);

    }


    [System.Obsolete("Replaced by " + nameof(GeCurrentGoals), true)]
    void GetGoals() // BUG  what's going on here?  why is goalMood[1,...] never used? 
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

        var goal = CurentPlayerGoalValues;
        goalDisplay.text = $"<b>Goal: {CurentPlayerGoal.ToString()}</b>\n{GetDisplayText("Pleasant","Unpleasant",goal[0])},      {GetDisplayText("Personal","Social" ,goal[1])},      {GetDisplayText("Calm","Energised",goal[2])}\n";

    }

    string GetDisplayText(string srt1, string str2, int value)
    {
        if(value > 0)
        {
            return $"<b>{srt1}</b>/{str2} {Mathf.Abs(value).ToString()}";
        }
        else if(value < 0)
        {
            return $"{srt1}/<b>{str2}</b> {Mathf.Abs(value).ToString()}";
        }
        else
        {
            return $"{srt1}/{str2} {Mathf.Abs(value).ToString()}";
        }
    }


    private string GetDisplayValue(int value)
    {
        return Mathf.Abs(value).ToString();
    }

    private void OnEnable()
    {
        EventsManager.BindEvent(EventsManager.EventType.NewTurnBegin, UpdateText);
        EventsManager.BindEvent(EventsManager.EventType.StartGame, GeCurrentGoals);
        EventsManager.BindEvent(EventsManager.EventType.UpdateScore, CheckIfWin);
    }

    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.NewTurnBegin, UpdateText);
        EventsManager.UnbindEvent(EventsManager.EventType.StartGame, GeCurrentGoals);
        EventsManager.UnbindEvent(EventsManager.EventType.UpdateScore, CheckIfWin);
    }


    // Update is called once per frame
    void Update()
    {
        // Move to Events for later build
       // GetGoals();
        GeCurrentGoals();
        UpdateText();

       // CheckIfWin();
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
                if(curmood[player][i] != CurentPlayerGoalValues[i])
                {
                    win = false;
                }
            }
        }

        return false; // if reached here then no match
    }
}
