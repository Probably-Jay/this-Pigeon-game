using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TurnTracker
{
    public int Turn { get; private set; } = 0;
    /// <summary>
    /// If the turn is currently active and started ( there is a breif period after end turn button is pressed where this is false) 
    /// </summary>
    public bool TurnActive { get; private set; }
    /// <summary>
    /// The current player's turn (player 0 goes first)
    /// </summary>
    public Player.PlayerEnum PlayerTurn { get => Turn % 2 != 0 ? Player.PlayerEnum.Player1 : Player.PlayerEnum.Player2 ; }

  

    /// <summary>
    /// Increment the turn
    /// </summary>
    public void ProgressTurn() { Turn++; TurnActive = true; }

    //public void EndTurn() => TurnActive = false;

    public void EndTurn() { 
        TurnActive = false;
    }

}
