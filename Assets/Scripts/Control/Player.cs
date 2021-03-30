using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// created jay 12/02
/// <summary>
/// Class holding all of the data about a player
/// </summary>
[RequireComponent(typeof(TurnPoints))]
public class Player : MonoBehaviour
{

    public const int NumberOfPlayers = 2;
    public enum PlayerEnum { Player1 = 0, Player2 = 1 };

    public TurnPoints TurnPoints { get; private set; }
    public PlayerEnum PlayerEnumValue { get; set; }
    public bool HasAcheivedGoal { get; private set; } = false;


    private void OnEnable()
    {
        EventsManager.BindEvent(EventsManager.ParameterEventType.AcheivedGoal, CheckWin);
    }

    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.ParameterEventType.AcheivedGoal, CheckWin);
    }

    private void CheckWin(EventsManager.EventParams eventParams)
    {
        if((PlayerEnum)eventParams.EnumData == PlayerEnumValue)
        {
            HasAcheivedGoal = true;
        }
    }

    public void StartTurn()
    {
        TurnPoints.StartTurn();
    }

    public void Init(PlayerEnum player)
    {
        PlayerEnumValue = player;
        TurnPoints = GetComponent<TurnPoints>();
    }

}
