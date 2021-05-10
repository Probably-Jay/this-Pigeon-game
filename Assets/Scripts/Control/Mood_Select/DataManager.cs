using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script created by Alexander Purvis 26/04/2021
namespace NetSystem
{

    public class DataManager 
    {
        int plantPositionInList = 0;

        public PlayerDataPacket PlayerData { get; private set; }
        public GardenDataPacket GardenData { get; private set; }

        //int plantType = 0;
        //int stage = 0;

        //bool action1 = false;
        //bool action2 = false;
        //bool action3 = false;
        //bool action4 = false;


        public DataManager()
        {
            PlayerData = new PlayerDataPacket();
            GardenData = new GardenDataPacket();
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
            PlayerData.turnCounter = -1;
        }

        public void SetTurnCounter(int value)
        {
            PlayerData.turnCounter = value;
        }

        /// <summary>
        ///  updates the turn couter
        /// </summary>
        public void IncrementTurnCounter()
        {
            PlayerData.turnCounter++;
        }

        /// <summary>
        /// sets turn owner
        /// </summary>
        public void SetTheTurnsOwner(string newTurnOwner)
        {
            PlayerData.turnOwner = newTurnOwner;
        }

        /// <summary>
        /// set the state of the turn complete variable 
        /// </summary>
        public void SetStateOfTurnComplete(bool currentTurnComplete)
        {
            PlayerData.turnComplete = currentTurnComplete;
        }

        // Player 1 data

        /// <summary>
        /// sets the ID for player 1 to the device number of player ones phone genrated by the api
        /// </summary>
        public void SetPlayer1ID(string newID)
        {
            PlayerData.player1ID = newID;
        }


        /// <summary>
        /// sets the goal mood that player 1 chose at the start of the game
        /// </summary>
        public void SetPlayer1GoalMood(int chosenMood)
        {
            PlayerData.player1ChosenMood = chosenMood;
        }

        public int GetPlayer1GoalMood()
        {
            return PlayerData.player1ChosenMood;
        }

        /// <summary>
        /// sets the mood achived bool for player 1 to the value of the bool passed in
        /// // Note if the player ends thier tun with the mood values of thier garden equal to their chosed mood this will be true. In not then false
        /// </summary>
        public void SetPlayer1AchivedMood(bool moodAchived)
        {
            PlayerData.player1MoodAchieved = moodAchived;
        }

        /// <summary>
        /// decreases the action points player 1 has for planting in thier own garden
        /// </summary>
        public void SpendPlayer1SelfAction()
        {
            PlayerData.player1SelfActions--;
        }

        /// <summary>
        /// decreases the action points player 1 has for planting in thier partners garden
        /// </summary>
        public void SpendPlayer1OtherAction()
        {
            PlayerData.player1OtherActions--;
        }

        /// <summary>
        /// Resets the both sets of action points for player 1 uses to plant in thier garden and their partners garden
        /// </summary>
        public void ResetPlayer1ActionPoints()
        {
            PlayerData.player1SelfActions = 1;
            PlayerData.player1OtherActions = 1;
        }

        public void SetPlayer1ActionPoints(int self, int other)
        {
            PlayerData.player1SelfActions = 1;
            PlayerData.player1OtherActions = 1;
        }


        // Player 2 data

        /// <summary>
        /// sets the ID for player 2 to the device number of player ones phone genrated by the api
        /// </summary>
        public void SetPlayer2ID(string newID)
        {
            PlayerData.player2ID = newID;
        }

        /// <summary>
        /// sets the goal mood that player 2 chose at the start of the game
        /// </summary>
        public void SetPlayer2GoalMood(int chosenMood)
        {
            PlayerData.player2ChosenMood = chosenMood;
        }

        public int GetPlayer2GoalMood()
        {
            return PlayerData.player2ChosenMood;

        }
        /// <summary>
        /// sets the mood achived bool for player 2 to the value of the bool passed in
        /// // Note if the player ends thier tun with the mood values of thier garden equal to their chosed mood this will be true. In not then false
        /// </summary>
        public void SetPlayer2AchivedMood(bool moodAchived)
        {
            PlayerData.player2MoodAchieved = moodAchived;
        }

        /// <summary>
        /// decreases the action points player 2 has for planting in thier own garden
        /// </summary>
        public void SpendPlayer2SelfAction()
        {
            PlayerData.player2SelfActions--;
        }

        /// <summary>
        /// decreases the action points player 2 has for planting in thier partners garden
        /// </summary>
        public void SpendPlayer2OtherAction()
        {
            PlayerData.player2OtherActions--;
        }

        /// <summary>
        /// Resets the both sets of action points for player 2 uses to plant in thier garden and their partners garden
        /// </summary>
        public void ResetPlayer2ActionPoints()
        {
            PlayerData.player2SelfActions = 1;
            PlayerData.player2OtherActions = 1;
        }

        public void SetPlayer2ActionPoints(int self, int other)
        {
            PlayerData.player2SelfActions = 1;
            PlayerData.player2OtherActions = 1;
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
            for (int i = 0; i < GardenData.newestGarden1.Length; i++)
            {
                if (!GardenData.newestGarden1[i].Initilised)
                {
                    GardenData.newestGarden1[i] = new GardenDataPacket.Plant(plantType, slotNumber, stage, watering, spraying, trimming);
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

            for (int i = 0; i < GardenData.newestGarden1.Length; i++)
            {
                if (!GardenData.newestGarden1[i].Initilised)
                {
                    continue;
                }

                if (GardenData.newestGarden1[i].slotNumber == slotNumber)
                {
                    GardenData.newestGarden1[i].Uninitialise();
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
            for (int i = 0; i < GardenData.newestGarden2.Length; i++)
            {
                if (!GardenData.newestGarden2[i].Initilised)
                {
                    GardenData.newestGarden2[i] = new GardenDataPacket.Plant(plantType, slotNumber, stage, watering, spraying, trimming);
                    return;
                }
            }
            Debug.LogError("Over 10 plants");
        }


        public void UpdatePlantState(Player.PlayerEnum gardenNumber, 
            int slotNumber,
            int stage,
            bool watering,
            bool spraying,
            bool trimming)
        {
            if (!FindPlantInList(gardenNumber, slotNumber))
            {
                Debug.LogError("Cannot find plant");
                return;
            }

            if (gardenNumber == Player.PlayerEnum.Player1)
            {
                GardenData.newestGarden1[plantPositionInList].stage = stage;
                GardenData.newestGarden1[plantPositionInList].watering = watering;
                GardenData.newestGarden1[plantPositionInList].spraying = spraying;
                GardenData.newestGarden1[plantPositionInList].trimming = trimming;
            }
            else
            {
                GardenData.newestGarden2[plantPositionInList].stage = stage;
                GardenData.newestGarden2[plantPositionInList].watering = watering;
                GardenData.newestGarden2[plantPositionInList].spraying = spraying;
                GardenData.newestGarden2[plantPositionInList].trimming = trimming;
            }
        }

        /// <summary>
        ///  Removes a new Plant from player twos garden
        /// </summary>
        public void RemovePlantFromGarden2(int slotNumber)
        {
            // cycle through garden2s list of plants till the plant with the correct slot number is found
            // then remve that plant from the list
            for (int i = 0; i < GardenData.newestGarden2.Length; i++)
            {
                if (!GardenData.newestGarden2[i].Initilised)
                {
                    continue;
                }

                if (GardenData.newestGarden2[i].slotNumber == slotNumber)
                {
                    GardenData.newestGarden2[i] = null;
                }
            }
        }

        /// <summary>
        ///  Searches the appropriate garden for the appropriate Plant then copies that plants data 
        /// </summary>
        bool FindPlantInList(Player.PlayerEnum gardenNumber, int slotNumber)
        {

            switch (gardenNumber)
            {
                case Player.PlayerEnum.Player1:
                    // cycle through garden1s list of plants till the plant with the correct slot number
                    for (int i = 0; i < GardenData.newestGarden1.Length; i++)
                    {
                        if (!GardenData.newestGarden1[i].Initilised)
                        {
                            continue;
                        }

                        if (GardenData.newestGarden1[i].slotNumber == slotNumber)
                        {
                            // when the plant is found store the position in the list and all the data in that plant
                            plantPositionInList = i;

                            return true;

                        }
                    }
                    break;
                case Player.PlayerEnum.Player2:
                    // cycle through garden2s list of plants till the plant with the correct slot number is found 
                    for (int i = 0; i < GardenData.newestGarden2.Length; i++)
                    {
                        if (!GardenData.newestGarden2[i].Initilised)
                        {
                            continue;
                        }

                        if (GardenData.newestGarden2[i].slotNumber == slotNumber)
                        {


                            // when the plant is found store the position in the list and all the data in that plant
                            plantPositionInList = i;

                            return true;

                        }
                    }
                    break;
            }


            return false;
        }



        /*
        public void AddTendingActionToPlant(int gardenNumber, int slotNumber, Plants.PlantActions.TendingActions tendingAction)
        {

            if (!FindPlantInList(gardenNumber, slotNumber))
            {
                Debug.LogError("Cannot find plant");
                return;
            }



            // override the data for the desired tending action
            switch (tendingAction)
            {
                case Plants.PlantActions.TendingActions.Watering:
                    if (gardenNumber == 1)
                    {
                        GardenData.newestGarden1[plantPositionInList].watering = true;
                    }
                    else
                    {
                        GardenData.newestGarden2[plantPositionInList].watering = true;
                    }
                    break;
                case Plants.PlantActions.TendingActions.Spraying:
                    if (gardenNumber == 1)
                    {
                        GardenData.newestGarden1[plantPositionInList].spraying = true;
                    }
                    else
                    {
                        GardenData.newestGarden2[plantPositionInList].spraying = true;
                    }
                    break;
                case Plants.PlantActions.TendingActions.Trimming:
                    if (gardenNumber == 1)
                    {
                        GardenData.newestGarden1[plantPositionInList].trimming = true;
                    }
                    else
                    {
                        GardenData.newestGarden2[plantPositionInList].trimming = true;
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

            if (!FindPlantInList(gardenNumber, slotNumber))
            {
                Debug.LogError("Cannot find plant");
                return;
            }

            // override the data for the desired tending action
            switch (tendingAction)
            {
                case Plants.PlantActions.TendingActions.Watering:
                    if (gardenNumber == 1)
                    {
                        GardenData.newestGarden1[plantPositionInList].watering = false;
                    }
                    else
                    {
                        GardenData.newestGarden2[plantPositionInList].watering = false;
                    }
                    break;
                case Plants.PlantActions.TendingActions.Spraying:
                    if (gardenNumber == 1)
                    {
                        GardenData.newestGarden1[plantPositionInList].spraying = false;
                    }
                    else
                    {
                        GardenData.newestGarden2[plantPositionInList].spraying = false;
                    }
                    break;
                case Plants.PlantActions.TendingActions.Trimming:
                    if (gardenNumber == 1)
                    {
                        GardenData.newestGarden1[plantPositionInList].trimming = false;
                    }
                    else
                    {
                        GardenData.newestGarden2[plantPositionInList].trimming = false;
                    }
                    break;
            }
        }
        */

        public void UpdatePlantTendingActions(Plants.Plant plant)
        {
            if (!FindPlantInList(plant.StoredInGarden, plant.StoredInSlot))
            {
                Debug.LogError("Cannot find plant");
                return;
            }

            switch (plant.StoredInGarden)
            {
                case Player.PlayerEnum.Player1:
                    GardenData.newestGarden1[plantPositionInList].watering = plant.RequiresAction(Plants.PlantActions.TendingActions.Watering);
                    GardenData.newestGarden1[plantPositionInList].spraying = plant.RequiresAction(Plants.PlantActions.TendingActions.Spraying);
                    GardenData.newestGarden1[plantPositionInList].trimming = plant.RequiresAction(Plants.PlantActions.TendingActions.Trimming);
                    break;
                case Player.PlayerEnum.Player2:
                    GardenData.newestGarden2[plantPositionInList].watering = plant.RequiresAction(Plants.PlantActions.TendingActions.Watering);
                    GardenData.newestGarden2[plantPositionInList].spraying = plant.RequiresAction(Plants.PlantActions.TendingActions.Spraying);
                    GardenData.newestGarden2[plantPositionInList].trimming = plant.RequiresAction(Plants.PlantActions.TendingActions.Trimming);
                    break;
            }

        }

    }
}