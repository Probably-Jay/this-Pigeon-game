using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TurnTracker
{
    public int Turn { get; private set; } = 0;

    public bool TurnActive { get; private set; }
    /// <summary>
    /// The current player's turn (player 0 goes first)
    /// </summary>
    public Player.PlayerEnum PlayerTurn { get => Turn % 2 != 0 ? Player.PlayerEnum.Player0 : Player.PlayerEnum.Player1 ; }

    /// <summary>
    /// Increment the turn
    /// </summary>
    public void ProgressTurn() { Turn++; TurnActive = true; }
    public void EndTurn() => TurnActive = false;
}
