using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mood;

// created jay
// altered jay 27/03

public class GoalStore : MonoBehaviour
{
    private const string goalKeyPlayer1 = "Goal_Player1";
    private const string goalKeyPlayer2 = "Goal_Player2";




    //[SerializeField] Dropdown dropdownPlayer1;
   // [SerializeField] Dropdown dropdownPlayer2;

    [SerializeField] Dropdown dropdown;
    [SerializeField] Player.PlayerEnum player;

    public void StoreGoal() => StoreGoal(player, (Emotion.Emotions)dropdown.value);


    //public void StoreGoal()
    //{
    //    if (dropdownPlayer1) { 
    //        var value1 = dropdownPlayer1.value;
    //        StoreGoal(Player.PlayerEnum.Player1,(Emotion.Emotions)value1);
    //    }
    //    if (dropdownPlayer2)
    //    {
    //        var value2 = dropdownPlayer2.value;
    //        StoreGoal(Player.PlayerEnum.Player2, (Emotion.Emotions)value2);
    //    }
    //}

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
