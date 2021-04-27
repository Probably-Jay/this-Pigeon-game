using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    PlayerDataPacket playerData;
    GardenDataPacket gardenData;

    int plantPositionInList = 0;

    //int plantType = 0;
    //int stage = 0;

    //bool action1 = false;
    //bool action2 = false;
    //bool action3 = false;
    //bool action4 = false;


    void Awake()
    {
        playerData = GetComponent<PlayerDataPacket>();
        gardenData = GetComponent<GardenDataPacket>();
    }

    private void OnEnable()
    {
        EventsManager.BindEvent(EventsManager.ParameterEventType.OnPlantPlanted, AddNewPlant);
    }
    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.ParameterEventType.OnPlantPlanted, AddNewPlant);

    }

    private void AddNewPlant(EventsManager.EventParams eventParams)
    {

        switch ((Player.PlayerEnum)eventParams.EnumData2)
        {
            case Player.PlayerEnum.Player1:
                AddPlantToGarden1(
                    plantType: (int)(Plants.Plant.PlantName)eventParams.EnumData1,
                    slotNumber: eventParams.IntData1,
                    stage: eventParams.IntData2,
                    watering: eventParams.Bool1,
                    spraying: eventParams.Bool2,
                    trimming: eventParams.Bool3
                    );
                break;
            case Player.PlayerEnum.Player2:
                AddPlantToGarden2(
                    plantType: (int)(Plants.Plant.PlantName)eventParams.EnumData1,
                    slotNumber: eventParams.IntData1,
                    stage: eventParams.IntData2,
                    watering: eventParams.Bool1,
                    spraying: eventParams.Bool2,
                    trimming: eventParams.Bool3
                    );
                break;
        }
    }
    

    /// <summary>
    /// Set the turn couter to 0
    /// </summary>
    public void InitialiseTurnCounter()
    {
        playerData.turnCounter =0;
    }
    
    /// <summary>
    ///  updates the turn couter
    /// </summary>
    public void IncrementTurnCounter()
    {
        playerData.turnCounter++;
    }

    /// <summary>
    /// sets turn owner
    /// </summary>
    public void SetTheTurnsOwner(string newTurnOwner)
    {
        playerData.turnOwner = newTurnOwner;
    }

    /// <summary>
    /// set the state of the turn complete variable 
    /// </summary>
    public void SetStateOfTurnComplete(bool currentTurnComplete)
    {
        playerData.turnComplete = currentTurnComplete;
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

    /// <summary>
    ///  Adds a new Plant to player ones garden
    /// </summary>
    public void AddPlantToGarden1(
    int plantType,
    int slotNumber,
    int stage,
    bool watering,
    bool spraying,
    bool trimming)
    {
        //gardenData.newestGarden1.Add(new GardenDataPacket.Plant(plantType, slotNumber, stage, action1, action2, action3, action4));
        for (int i = 0; i < gardenData.newestGarden1.Length; i++)
        {
            if(gardenData.newestGarden1[i] == null)
            {
                gardenData.newestGarden1[i] = new GardenDataPacket.Plant(plantType, slotNumber, stage, watering, spraying, trimming);
                return;
            }
        }
        Debug.LogError("Over 10 plants");
    }

    /// <summary>
    ///  Removes a new Plant from player ones garden
    /// </summary>
    public void RemovePlantFromGarden1(int slotNumber)
    {
        // cycle through garden1s list of plants till the plant with the correct slot number is found
        // then remove that plant from the list

        for (int i = 0; i < gardenData.newestGarden1.Length; i++)
        {
            if(gardenData.newestGarden1[i] == null)
            {
                continue;
            }

            if (gardenData.newestGarden1[i].slotNumber == slotNumber)
            {
                gardenData.newestGarden1[i] = null;
            }
        }
    }
    /// <summary>
    ///  Adds a new Plant to player twos garden
    /// </summary>
    public void AddPlantToGarden2(      
        int plantType,
        int slotNumber,
        int stage,
        bool watering,
        bool spraying,
        bool trimming)
    {
        for (int i = 0; i < gardenData.newestGarden2.Length; i++)
        {
            if (gardenData.newestGarden2[i] == null)
            {
                gardenData.newestGarden2[i] = new GardenDataPacket.Plant(plantType, slotNumber, stage, watering, spraying, trimming);
                return;
            }
        }
        Debug.LogError("Over 10 plants");
    }

    /// <summary>
    ///  Removes a new Plant from player twos garden
    /// </summary>
    public void removePlantFromGarden2(int slotNumber)
    {
        // cycle through garden2s list of plants till the plant with the correct slot number is found
        // then remve that plant from the list
        for (int i = 0; i < gardenData.newestGarden2.Length; i++)
        {
            if (gardenData.newestGarden2[i] == null)
            {
                continue;
            }

            if (gardenData.newestGarden2[i].slotNumber == slotNumber)
            {
                gardenData.newestGarden2[i] = null;
            }
        }
    }

    /// <summary>
    ///  Searches the appropriate garden for the appropriate Plant then copies that plants data 
    /// </summary>
    void FindPlantInList(int gardenNumber, int slotNumber)
    {
        if (gardenNumber == 1)
        {
            // cycle through garden1s list of plants till the plant with the correct slot number
            for (int i = 0; i < gardenData.newestGarden1.Length; i++)
            {
                if (gardenData.newestGarden1[i] == null)
                {
                    continue;
                }

                if (gardenData.newestGarden1[i].slotNumber == slotNumber)
                {
                    // when the plant is found store the position in the list and all the data in that plant
                    plantPositionInList = i;

                    //plantType = gardenData.newestGarden1[i].plantType;                
                    //stage = gardenData.newestGarden1[i].stage;

                    //action1 = gardenData.newestGarden1[i].action1;
                    //action2 = gardenData.newestGarden1[i].action2;
                    //action3 = gardenData.newestGarden1[i].action3;
                    //action4 = gardenData.newestGarden1[i].action4;
                }
            }
        }
        else
        {
            // cycle through garden2s list of plants till the plant with the correct slot number is found 
            for (int i = 0; i < gardenData.newestGarden2.Length; i++)
            {
                if (gardenData.newestGarden2[i].slotNumber == slotNumber)
                {
                    if (gardenData.newestGarden2[i] == null)
                    {
                        continue;
                    }

                    // when the plant is found store the position in the list and all the data in that plant
                    plantPositionInList = i;

                    //plantType = gardenData.newestGarden2[i].plantType;
                    //stage = gardenData.newestGarden2[i].stage;

                    //action1 = gardenData.newestGarden2[i].action1;
                    //action2 = gardenData.newestGarden2[i].action2;
                    //action3 = gardenData.newestGarden2[i].action3;
                    //action4 = gardenData.newestGarden2[i].action4;
                }
            }
        }
    }



    public void AddTendingActionToPlant(int gardenNumber, int slotNumber, Plants.PlantActions.TendingActions tendingAction)
    {

        FindPlantInList(gardenNumber, slotNumber);



        // override the data for the desired tending action
        switch (tendingAction)
        {
            case Plants.PlantActions.TendingActions.Watering:
                if (gardenNumber == 1)
                {
                    gardenData.newestGarden1[plantPositionInList].watering = true;
                }
                else
                {
                    gardenData.newestGarden2[plantPositionInList].watering = true;
                }
                break;
            case Plants.PlantActions.TendingActions.Spraying:
                if (gardenNumber == 1)
                {
                    gardenData.newestGarden1[plantPositionInList].spraying = true;
                }
                else
                {
                    gardenData.newestGarden2[plantPositionInList].spraying = true;
                }
                break;
            case Plants.PlantActions.TendingActions.Trimming:
                if (gardenNumber == 1)
                {
                    gardenData.newestGarden1[plantPositionInList].trimming = true;
                }
                else
                {
                    gardenData.newestGarden2[plantPositionInList].trimming = true;
                }
                break;
            //case 4:
            //    if (gardenNumber == 1)
            //    {
            //        gardenData.newestGarden1[plantPositionInList].action4 = true;
            //    }
            //    else
            //    {
            //        gardenData.newestGarden2[plantPositionInList].action4 = true;
            //    }
            //    break;
        }

    }

    public void RemoveTendingActionFromPlant(int gardenNumber, int slotNumber, Plants.PlantActions.TendingActions tendingAction)
    {

        FindPlantInList(gardenNumber, slotNumber);

        // override the data for the desired tending action
        switch (tendingAction)
        {
            case Plants.PlantActions.TendingActions.Watering:
                if (gardenNumber == 1)
                {
                    gardenData.newestGarden1[plantPositionInList].watering = false;
                }
                else
                {
                    gardenData.newestGarden2[plantPositionInList].watering = false;
                }
                break;
            case Plants.PlantActions.TendingActions.Spraying:
                if (gardenNumber == 1)
                {
                    gardenData.newestGarden1[plantPositionInList].spraying = false;
                }
                else
                {
                    gardenData.newestGarden2[plantPositionInList].spraying = false;
                }
                break;
            case Plants.PlantActions.TendingActions.Trimming:
                if (gardenNumber == 1)
                {
                    gardenData.newestGarden1[plantPositionInList].trimming = false;
                }
                else
                {
                    gardenData.newestGarden2[plantPositionInList].trimming = false;
                }
                break;
            //default:
            //    if (gardenNumber == 1)
            //    {
            //        gardenData.newestGarden1[plantPositionInList].action4 = false;
            //    }
            //    else
            //    {
            //        gardenData.newestGarden2[plantPositionInList].action4 = false;
            //    }
            //    break;
        }
    }

    public void GrowPlant(int gardenNumber, int slotNumber)
    {
        FindPlantInList(gardenNumber, slotNumber);

        if (gardenNumber == 1)
        {
            gardenData.newestGarden1[plantPositionInList].stage++;  
        }
        else
        {
            gardenData.newestGarden2[plantPositionInList].stage++;
        }
    }
}
