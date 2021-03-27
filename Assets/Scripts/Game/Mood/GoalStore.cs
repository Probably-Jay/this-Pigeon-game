using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mood;

public class GoalStore : MonoBehaviour
{
    private const string goalKeyPlayer1 = "Goal_Player1";
    private const string goalKeyPlayer2 = "Goal_Player2";




    [SerializeField] Dropdown dropdownPlayer1;
    [SerializeField] Dropdown dropdownPlayer2;
    private void OnEnable()
    {
        EventsManager.BindEvent(EventsManager.EventType.BeginSceneLoad, StoreGoal);
    }

    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.BeginSceneLoad, StoreGoal);
    }

    public void StoreGoal()
    {
        if (dropdownPlayer1) { 
            var value1 = dropdownPlayer1.value;
            StoreGoal(Player.PlayerEnum.Player1,(Emotion.Emotions)value1);
        }
        if (dropdownPlayer2)
        {
            var value2 = dropdownPlayer2.value;
            StoreGoal(Player.PlayerEnum.Player2, (Emotion.Emotions)value2);
        }
    }

    public static void StoreGoal(Player.PlayerEnum player, Emotion.Emotions goal)
    {
        switch (player)
        {
            case Player.PlayerEnum.Player1:
                PlayerPrefs.SetInt(goalKeyPlayer1, (int)goal);
                break;
            case Player.PlayerEnum.Player2:
                PlayerPrefs.SetInt(goalKeyPlayer2, (int)goal);
                break;
        }
        PlayerPrefs.Save();
    }

    public static Emotion.Emotions GetGoal()
    {
        return (Emotion.Emotions)PlayerPrefs.GetInt(goalKeyPlayer1);
    }

    public static Emotion.Emotions GetAltGoal()
    {
        return (Emotion.Emotions)PlayerPrefs.GetInt(goalKeyPlayer2);
    }


}
