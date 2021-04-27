using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataPacket : MonoBehaviour
{
    public enum Moods { Mood1, Mood2, Mood3, Mood4 };

    // game data
    public int turnCounter = 0;
    public string turnOwner = "NULL";
    public bool compleate = false;

    // player 1 data
    public string player1ID = "NULL";
    public Moods player1ChosenMood = Moods.Mood1;
    public bool player1MoodAchieved = false;

    public int player1SelfActions;
    public int player1OtherActions;


    // player 2 data
    public string player2ID = "NULL";
    public Moods player2ChosenMood = Moods.Mood1;
    public bool player2MoodAchieved = false;

    public int player2SelfActions;
    public int player2OtherActions;
}