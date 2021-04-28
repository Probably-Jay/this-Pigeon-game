using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerDataPacket
{
 

    // game data
    public int turnCounter = 0;
    public string turnOwner = "";
    public bool turnComplete = false;

    // player 1 data
    public string player1ID = "";
    public int player1ChosenMood;
    public bool player1MoodAchieved = false;

    public int player1SelfActions;
    public int player1OtherActions;


    // player 2 data
    public string player2ID = "";
    public int player2ChosenMood;
    public bool player2MoodAchieved = false;

    public int player2SelfActions;
    public int player2OtherActions;
}