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
    public bool Player1Initilised => player1ID != null && player1ChosenMood != -1;
    public string player1ID = "";
    public int player1ChosenMood = -1;
    public bool player1MoodAchieved = false;

    public int player1SelfActions;
    public int player1OtherActions;


    // player 2 data
    public bool Player2Initilised => player2ID != null && player2ChosenMood != -1;
    public string player2ID = "";
    public int player2ChosenMood = -1;
    public bool player2MoodAchieved = false;

    public int player2SelfActions;
    public int player2OtherActions;
}