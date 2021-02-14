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

    public enum PlayerEnum { Player0 = 0, Player1 = 1 };

    public TurnPoints TurnPoints { get; private set; }
    public PlayerEnum PlayerEnumValue { get; set; }


    // Update is called once per frame
    void Update()
    {

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
