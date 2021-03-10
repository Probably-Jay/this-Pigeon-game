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

    [SerializeField] DisplayManager gardenScoreCalculator;

   // [SerializeField] GardenEmotionIndicatorControls pleasant;
    //[SerializeField] GardenEmotionIndicatorControls social;
    //[SerializeField] GardenEmotionIndicatorControls energy;
   



    Goal CurrentPlayerGoalName => GameManager.Instance.ActivePlayer.PlayerEnumValue == Player.PlayerEnum.Player0 ? GameManager.Instance.CurrentGoal : GameManager.Instance.AlternateGoal;
    MoodAtributes CurrentPlayerGoal => allGoals[CurrentPlayerGoalName];




    public Text goalDisplay;
    public TMP_Text UnpleasantPleasantDisplay;
    public TMP_Text PersonalSocialDisplay;
    public TMP_Text CalmEnergisedDisplay;


    Dictionary<Player.PlayerEnum, MoodAtributes> goalMood = new Dictionary<Player.PlayerEnum, MoodAtributes>();
    
    Dictionary<Goal, MoodAtributes> allGoals;

    private void Awake()
    {
        allGoals = new Dictionary<Goal, MoodAtributes>()
        {
             {Goal.Proud,   new MoodAtributes( 2, -1, -1 ) }, // Unpleasant/Pleasant, Personal/Social, Calm/Energised
             {Goal.Anxious, new MoodAtributes(-1, -1, 2 ) },
             {Goal.Content, new MoodAtributes( 1, 1, 2 ) }
        };
    }


    private void GetCurrentGoals()
    {
        var player1Goal = GameManager.Instance.CurrentGoal;
        var player2Goal = GameManager.Instance.AlternateGoal;

        goalMood[Player.PlayerEnum.Player0] = allGoals[player1Goal];
        goalMood[Player.PlayerEnum.Player1] = allGoals[player2Goal];

        pleasant.UpdateIndicator(CurrentPlayerGoal.Pleasance);
        social.UpdateIndicator(CurrentPlayerGoal.Sociability);
        energy.UpdateIndicator(CurrentPlayerGoal.Energy);
       
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

        /// Mood Index Sprites
        /// 0 = Energetic
        /// 1 = NeutPleasant
        /// 2 = Pleasant
        /// 3 = Sad (TEMP, ONLY FOR TESTING)
        /// 4 = NeutSocial
        /// 5 = Social
        /// 6 = Calm
        /// 7 = Unpleasant
        /// 8 = Personal
        /// 9 = NeutEnergy
        
        // set Unpleasant/Pleasant text
        string UnpleasantPleasant_Text = " ";
        string UnpleasantPleasant_Text = MoodAtributes.GetName(MoodAtributes.Scales.Pleasance, CurrentPlayerGoal.Pleasance);
        string PersonalSocial_Text = MoodAtributes.GetName(MoodAtributes.Scales.Sociability, CurrentPlayerGoal.Sociability);
        string CalmEnergised_Text = MoodAtributes.GetName(MoodAtributes.Scales.Energy, CurrentPlayerGoal.Energy);

        if (goal[0] < 0)
        {
            UnpleasantPleasant_Text = "Unpleasant: <sprite=3>";
        }
        else if (goal[0] > 0)
        {
            UnpleasantPleasant_Text = "Pleasant: <sprite=2>";
        }
        else if (goal[0] == 0)
        {
            UnpleasantPleasant_Text = "Neutral: <sprite=1>";
        }

        goalDisplay.text = $"<b>Goal: {CurrentPlayerGoalName.ToString()}</b>";

        UnpleasantPleasantDisplay.text = GetDisplayText(UnpleasantPleasant_Text, CurrentPlayerGoal.Pleasance);
        PersonalSocialDisplay.text = GetDisplayText(PersonalSocial_Text, CurrentPlayerGoal.Sociability);
        CalmEnergisedDisplay.text =  GetDisplayText(CalmEnergised_Text, CurrentPlayerGoal.Energy);
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
                if(curmood[player][i] != CurrentPlayerGoal[i])
                {
                    win = false;
                }
            }
        }

        return false; // if reached here then no match
    }
}
