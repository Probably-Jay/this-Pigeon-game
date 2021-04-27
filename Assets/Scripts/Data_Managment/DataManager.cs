using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    PlayerDataPacket playerData;
    GardenDataPacket gardenData;

    void Awake()
    {
        playerData = GetComponent<PlayerDataPacket>();
        gardenData = GetComponent<GardenDataPacket>();
    }

    /// <summary>
    ///  updates the turn couter
    /// </summary>
    public void incrementTurnCounter()
    {
        playerData.turnCounter++;
    }


// Player 1 data

    /// <summary>
    /// sets the ID for player 1 to the device number of player ones phone genrated by the api
    /// </summary>
   public void SetPlayer1ID(string newID)
   {
        playerData.player1ID = newID;
   }

    /// <summary>
    /// sets the goal mood that player 1 chose at the start of the game
    /// </summary>
    public void SetPlayer1GoalMood(int chosenMood)
    {
        switch (chosenMood)
        {
            case 1:
                playerData.player1ChosenMood = PlayerDataPacket.Moods.Mood1;
                break;
            case 2:
                playerData.player1ChosenMood = PlayerDataPacket.Moods.Mood2;
                break;
            case 3:
                playerData.player1ChosenMood = PlayerDataPacket.Moods.Mood3;
                break;
            case 4:
                playerData.player1ChosenMood = PlayerDataPacket.Moods.Mood4;
                break;
            default:
               
                break;
        }     
    }

    /// <summary>
    /// sets the mood achived bool for player 1 to the value of the bool passed in
    /// // Note if the player ends thier tun with the mood values of thier garden equal to their chosed mood this will be true. In not then false
    /// </summary>
    public void SetPlayer1AchivedMood(bool moodAchived)
    {
        playerData.player1MoodAchieved = moodAchived;
    }

    /// <summary>
    /// decreases the action points player 1 has for planting in thier own garden
    /// </summary>
    public void SpendPlayer1SelfAction()
    {
        playerData.player1SelfActions--;
    }

    /// <summary>
    /// decreases the action points player 1 has for planting in thier partners garden
    /// </summary>
    public void SpendPlayer1OtherAction()
    {
        playerData.player1OtherActions--;
    }

    /// <summary>
    /// Resets the both sets of action points for player 1 uses to plant in thier garden and their partners garden
    /// </summary>
    public void ResetPlayer1ActionPoints()
    {
        playerData.player1SelfActions = 1;
        playerData.player1SelfActions = 1;
    }


    // Player 2 data

    /// <summary>
    /// sets the ID for player 2 to the device number of player ones phone genrated by the api
    /// </summary>
    public void SetPlayer2ID(string newID)
    {
       playerData.player2ID = newID;
    }

    /// <summary>
    /// sets the goal mood that player 2 chose at the start of the game
    /// </summary>
    public void SetPlayer2GoalMood(int chosenMood)
    {
        switch (chosenMood)
        {
            case 1:
                playerData.player2ChosenMood = PlayerDataPacket.Moods.Mood1;
                break;
            case 2:
                playerData.player2ChosenMood = PlayerDataPacket.Moods.Mood2;
                break;
            case 3:
                playerData.player2ChosenMood = PlayerDataPacket.Moods.Mood3;
                break;
            case 4:
                playerData.player2ChosenMood = PlayerDataPacket.Moods.Mood4;
                break;
            default:

                break;
        }
    }

    /// <summary>
    /// sets the mood achived bool for player 2 to the value of the bool passed in
    /// // Note if the player ends thier tun with the mood values of thier garden equal to their chosed mood this will be true. In not then false
    /// </summary>
    public void SetPlayer2AchivedMood(bool moodAchived)
    {
        playerData.player2MoodAchieved = moodAchived;
    }

    /// <summary>
    /// decreases the action points player 2 has for planting in thier own garden
    /// </summary>
    public void SpendPlayer2SelfAction()
    {
        playerData.player2SelfActions--;
    }

    /// <summary>
    /// decreases the action points player 2 has for planting in thier partners garden
    /// </summary>
    public void SpendPlayer2OtherAction()
    {
        playerData.player2OtherActions--;
    }

    /// <summary>
    /// Resets the both sets of action points for player 2 uses to plant in thier garden and their partners garden
    /// </summary>
    public void ResetPlayer2ActionPoints()
    {
        playerData.player2SelfActions = 1;
        playerData.player2SelfActions = 1;
    }

    //Garden Data


    public void AddPlantToGarden1(
    int plantType,
    int slotNumber,
    int stage,
    bool action1,
    bool action2,
    bool action3,
    bool action4)
    {
        gardenData.newestGarden1.Add(new GardenDataPacket.Plant(plantType, slotNumber, stage, action1, action2, action3, action4));
    }

    public void removePlantFromGarden1(int slotNumber)
    {
        // cycle through garden1s list of plants till the plant with the correct slot number is found
        // then remove that plant from the list

        for (int i = 0; i < gardenData.newestGarden1.Count; i++)
        {
            if (gardenData.newestGarden1[i].slotNumber == slotNumber)
            {
                //gardenData.newestGarden1.RemoveAt(i);
            }
        }
    }

    public void AddPlantToGarden2(      
        int plantType,
        int slotNumber,
        int stage,
        bool action1,
        bool action2,
        bool action3,
        bool action4)
    {
        gardenData.newestGarden2.Add(new GardenDataPacket.Plant(plantType, slotNumber, stage, action1, action2, action3, action4));
    }

    public void removePlantFromGarden2(int slotNumber)
    {
        // cycle through garden2s list of plants till the plant with the correct slot number is found
        // then remve that plant from the list
    }

    public void AddTendingActionToPlant(int gardenNumber, int SlotNumber, int tendingAction)
    {
        switch (tendingAction)
        {
            case 1:
                if (gardenNumber == 1)
                {
                    // cycle through garden1s list of plants till the plant with the correct slot number is found and set the bool for action 1 to true
                    for (int i = 0; i < gardenData.newestGarden1.Count; i++)
                    {
                        if (gardenData.newestGarden1[i].slotNumber == SlotNumber)
                        {
                          //  gardenData.newestGarden1[i].plantType;

                            int plantType;
                            int slotNumber;
                            int stage;


                            bool action1;
                            bool action2;
                            bool action3;
                            bool action4;

                            // gardenData.newestGarden1[i].plantType = 1;
                        }
                    }
                }
                else
                {
                    // cycle through garden2s list of plants till the plant with the correct slot number is found and set the bool for action 1 to true
                    for (int i = 0; i < gardenData.newestGarden2.Count; i++)
                    {

                    }
                }
                break;
            case 2:
                if (gardenNumber == 1)
                {
                    // cycle through garden1s list of plants till the plant with the correct slot number is found and set the bool for action 2 to true
                    for (int i = 0; i < gardenData.newestGarden1.Count; i++)
                    {

                    }
                }
                else
                {
                    // cycle through garden2s list of plants till the plant with the correct slot number is found and set the bool for action 2 to true
                    for (int i = 0; i < gardenData.newestGarden2.Count; i++)
                    {

                    }
                }
                break;
            case 3:
                if (gardenNumber == 1)
                {
                    // cycle through garden1s list of plants till the plant with the correct slot number is found and set the bool for action 3 to true
                    for (int i = 0; i < gardenData.newestGarden1.Count; i++)
                    {

                    }
                }
                else
                {
                    // cycle through garden2s list of plants till the plant with the correct slot number is found and set the bool for action 3 to true
                    for (int i = 0; i < gardenData.newestGarden2.Count; i++)
                    {

                    }
                }
                break;
            case 4:
                if (gardenNumber == 1)
                {
                    // cycle through garden1s list of plants till the plant with the correct slot number is found and set the bool for action 4 to true
                    for (int i = 0; i < gardenData.newestGarden1.Count; i++)
                    {

                    }
                }
                else
                {
                    // cycle through garden2s list of plants till the plant with the correct slot number is found and set the bool for action 4 to true
                    for (int i = 0; i < gardenData.newestGarden2.Count; i++)
                    {

                    }
                }
                break;
            default:
               
                break;
        }
    }

    public void RemoveTendingActionFromPlant(int gardenNumber, int slotNumber, int tendingAction)
    {
        switch (tendingAction)
        {
            case 1:
                if (gardenNumber == 1)
                {
                    // cycle through garden1s list of plants till the plant with the correct slot number is found and set the bool for action 1 to false
                    for (int i = 0; i < gardenData.newestGarden1.Count; i++)
                    {

                    }
                }
                else
                {
                    // cycle through garden2s list of plants till the plant with the correct slot number is found and set the bool for action 1 to false
                    for (int i = 0; i < gardenData.newestGarden2.Count; i++)
                    {

                    }
                }
                break;
            case 2:
                if (gardenNumber == 1)
                {
                    // cycle through garden1s list of plants till the plant with the correct slot number is found and set the bool for action 2 to false
                    for (int i = 0; i < gardenData.newestGarden1.Count; i++)
                    {

                    }
                }
                else
                {
                    // cycle through garden2s list of plants till the plant with the correct slot number is found and set the bool for action 2 to false
                    for (int i = 0; i < gardenData.newestGarden2.Count; i++)
                    {

                    }
                }
                break;
            case 3:
                if (gardenNumber == 1)
                {
                    // cycle through garden1s list of plants till the plant with the correct slot number is found and set the bool for action 3 to false
                    for (int i = 0; i < gardenData.newestGarden1.Count; i++)
                    {

                    }
                }
                else
                {
                    // cycle through garden2s list of plants till the plant with the correct slot number is found and set the bool for action 3 to false
                    for (int i = 0; i < gardenData.newestGarden2.Count; i++)
                    {

                    }
                }
                break;
            case 4:
                if (gardenNumber == 1)
                {
                    // cycle through garden1s list of plants till the plant with the correct slot number is found and set the bool for action 4 to false
                    for (int i = 0; i < gardenData.newestGarden1.Count; i++)
                    {

                    }
                }
                else
                {
                    // cycle through garden2s list of plants till the plant with the correct slot number is found and set the bool for action 4 to false
                    for (int i = 0; i < gardenData.newestGarden2.Count; i++)
                    {

                    }
                }
                break;
            default:

                break;
        }
    }

    public void GrowPlant(int gardenNumber, int slotNumber)
    {
        if (gardenNumber == 1)
        {
            // cycle through garden1s list of plants till the plant with the correct slot number is found
            // then set the increment the stage of that plant
            for (int i = 0; i < gardenData.newestGarden1.Count; i++)
            {

            }
        }
        else
        {
            // cycle through garden2s list of plants till the plant with the correct slot number is found
            // then set the increment the stage of that plant
            for (int i = 0; i < gardenData.newestGarden2.Count; i++)
            {

            }
        }
    }

}
