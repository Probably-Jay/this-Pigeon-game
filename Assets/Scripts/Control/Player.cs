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
    public static readonly PlayerEnum[] PlayerEnumValueList = (PlayerEnum[])System.Enum.GetValues(typeof(PlayerEnum));

    public NetSystem.PlayerClient PlayerClient { get; private set; }

    public TurnPoints TurnPoints { get; private set; }
    public PlayerEnum EnumID { get; set; }
    public bool HasAcheivedGoal { get; private set; } = false;

    public static PlayerEnum OtherPlayer(PlayerEnum player) => player == PlayerEnum.Player1 ? PlayerEnum.Player2 : PlayerEnum.Player1;


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
        if((PlayerEnum)eventParams.EnumData1 == EnumID)
        {
            HasAcheivedGoal = true;
        }
    }

    public void StartTurn()
    {
        TurnPoints.Initialise();
    }

    public void Init(NetSystem.PlayerClient netPlayer, Player.PlayerEnum playerWeAre)
    {
        PlayerClient = netPlayer;
        EnumID = playerWeAre;
        PlayerClient.PlayerGameEnumValue = playerWeAre;
        TurnPoints = GetComponent<TurnPoints>();
        
    }

}
