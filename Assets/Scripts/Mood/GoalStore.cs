using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalStore : MonoBehaviour
{
    private const string goalKeyPlayer1 = "Goal_Player1";
    private const string goalKeyPlayer2 = "Goal_Player2";



    //public override void Awake()
    //{
    //    InitSingleton();
    //}

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
            StoreGoal(Player.PlayerEnum.Player1,(GoalManagerScript.Goal)value1);
        }
        if (dropdownPlayer2)
        {
            var value2 = dropdownPlayer2.value;
            StoreGoal(Player.PlayerEnum.Player2, (GoalManagerScript.Goal)value2);
        }
    }

    public static void StoreGoal(Player.PlayerEnum player, GoalManagerScript.Goal goal)
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

    public static GoalManagerScript.Goal GetGoal()
    {
        return (GoalManagerScript.Goal)PlayerPrefs.GetInt(goalKeyPlayer1);
    }

    public static GoalManagerScript.Goal GetAltGoal()
    {
        return (GoalManagerScript.Goal)PlayerPrefs.GetInt(goalKeyPlayer2);
    }


}
