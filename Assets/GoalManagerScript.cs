using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalManagerScript : MonoBehaviour
{
    // Vars ---------------------
    enum Goal { Proud, Anxious, Content }


    Goal goalG1;
    Goal goalG2;

    int g1v;
    int g2v;

    public Text goalDisplay;

    int[,] goalMood = new int[2, 3];
    int[,] curMood = new int[2, 3];

    // Start is called before the first frame update
    void Start()
    {
    }

    void GetGoals() // BUG  what's going on here?  why is goalMood[1,...] never used? 
    {
        g1v = (int)GameManager.Instance.CurrentGoal;
        g2v = (int)GameManager.Instance.AlternateGoal;

        goalG1 = (Goal)g1v;
        goalG2 = (Goal)g2v;

        switch (goalG1)
        {
            case Goal.Proud:                       // Proud
                goalMood[0, 0] = 1;
                goalMood[0, 1] = 3;
                goalMood[0, 2] = 0;
                break;

            case Goal.Anxious:                     // Anxious
                goalMood[0, 0] = 1;
                goalMood[0, 1] = 0;
                goalMood[0, 2] = 3;
                break;

            case Goal.Content:                     // Content
                goalMood[0, 0] = 0;
                goalMood[0, 1] = 2;
                goalMood[0, 2] = 2;
                break;            
        }

        switch (goalG2)
        {
            case Goal.Proud:                     // Proud
                goalMood[1, 0] = 1;
                goalMood[1, 1] = 3;
                goalMood[1, 2] = 0;
                break;

            case Goal.Anxious:                     // Anxious
                goalMood[1, 0] = 1;
                goalMood[1, 1] = 0;
                goalMood[1, 2] = 3;
                break;

            case Goal.Content:                     // Content
                goalMood[1, 0] = 0;
                goalMood[1, 1] = 2;
                goalMood[1, 2] = 2;
                break;
        }
    }

    void UpdateText()
    {
        Goal turnGoal;
       // GameManager.Instance.ActivePlayer.PlayerEnumValue;
        if (GameManager.Instance.ActivePlayer.PlayerEnumValue == Player.PlayerEnum.Player0)
        {
            turnGoal = goalG1;
        }
        else
        {
            turnGoal = goalG2;
        }
        if (goalDisplay)
        {
            goalDisplay.text = $"Mood Goal:\n{turnGoal.ToString()}";
        }
    }

    private void OnEnable()
    {
        EventsManager.BindEvent(EventsManager.EventType.NewTurnBegin, UpdateText);
        EventsManager.BindEvent(EventsManager.EventType.StartGame, GetGoals);
    }

    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.NewTurnBegin, UpdateText);
        EventsManager.UnbindEvent(EventsManager.EventType.StartGame, GetGoals);
    }


    // Update is called once per frame
    void Update()
    {
        // Move to Events for later build
        GetGoals();
        UpdateText();
        CheckIfWin();
    }

    void CheckIfWin()
    {
        if (curMood == goalMood || Input.GetKeyDown("space"))
        {
            EventsManager.InvokeEvent(EventsManager.EventType.GameOver);
        }
    }
}
