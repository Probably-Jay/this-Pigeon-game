using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerDataPacket
{
 
    public PlayerDataPacket()
    {

    }

    public PlayerDataPacket(PlayerDataPacket other)
    {
        this.turnCounter = other.turnCounter;
        this.turnOwner = other.turnOwner;
        this.turnComplete = other.turnComplete;
        
        this.player1ID = other.player1ID;
        this.player1ChosenMood = other.player1ChosenMood;
        this.player1MoodAchieved = other.player1MoodAchieved;
        this.player1SelfActions = other.player1SelfActions;
        this.player1OtherActions = other.player1OtherActions;
         
        this.player2ID = other.player2ID;
        this.player2ChosenMood = other.player2ChosenMood;
        this.player2MoodAchieved = other.player2MoodAchieved;
        this.player2SelfActions = other.player2SelfActions;
        this.player2OtherActions = other.player2OtherActions;



    }


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