using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// jay 13/03

namespace SaveSystemInternal
{
    /// <summary>
    /// Static class of helper functions for manipulating the usable <see cref="LiveGameData"/> and the serialisable <see cref="SaveGameData"/>
    /// </summary>
    public static class SaveDataUtility
    {
        /// <summary>
        /// Returns a deep copy of the usable <see cref="LiveGameData"/> in the serialisable form of <see cref="SaveGameData"/>
        /// </summary>
        /// <param name="liveData">The data from the currently active game to be transformed into a serialisable format</param>
        /// <returns>A new strutcure which can be saved into a file</returns>
        public static SaveGameData GetSerialisableSaveGameStruct(LiveGameData liveData)
        {
            SaveGameData newData = new SaveGameData(liveData.gameID);

            newData.p1gardenGoals = liveData.gardenGoals[Player.PlayerEnum.Player0].DeepCopyValues();
            newData.p2gardenGoals = liveData.gardenGoals[Player.PlayerEnum.Player1].DeepCopyValues();

            newData.p1actionPoints = new int[TurnPoints.NumberOfPointTypes];
            newData.p2actionPoints = new int[TurnPoints.NumberOfPointTypes];
            foreach (TurnPoints.PointType type in System.Enum.GetValues(typeof(TurnPoints.PointType)))
            {
                newData.p1actionPoints[(int)type] = liveData.actionPoints[Player.PlayerEnum.Player0][type];
                newData.p2actionPoints[(int)type] = liveData.actionPoints[Player.PlayerEnum.Player1][type];
            }

            newData.plants = new SerialisedPlantData[liveData.plants.Count];
            for (int i = 0; i < liveData.plants.Count; i++)
            {
                PlantItem plant = liveData.plants[i];
                newData.plants[i] = GetSerialisablePlant(plant);
            }

            newData.turnNumber = liveData.turnNumber;


            return newData;
        }

        private static SerialisedPlantData GetSerialisablePlant(PlantItem plant)
        {
            SerialisedPlantData serialisedPlant = new SerialisedPlantData();

            serialisedPlant.plantNameType = (int)plant.plantname;

            var plantPos = plant.transform.position;
            serialisedPlant.position = new float[3] { plantPos.x, plantPos.y, plantPos.z };

            serialisedPlant.owner = (int)plant.plantOwner.PlayerEnumValue;

            return serialisedPlant;
        }

        /// <summary>
        /// Returns a deep copy of the serialisable of <see cref="SaveGameData"/> in the usable form of <see cref="LiveGameData"/>
        /// </summary>
        /// <param name="saveData">The data from a just opened file to be transformed into a usable format</param>
        /// <returns>A new strutcure which can be read into the game</returns>
        public static LiveGameData GetUsableLiveGameStruct(SaveGameData saveData)
        {
            LiveGameData newData = new LiveGameData((SerialisedPlantData[])saveData.plants.Clone());
            newData.gameID = saveData.localGameID;

            newData.gardenGoals = new Dictionary<Player.PlayerEnum, MoodAtributes>()
            {
                {Player.PlayerEnum.Player0, new MoodAtributes(saveData.p1gardenGoals[0],saveData.p1gardenGoals[1],saveData.p1gardenGoals[2]) },
                {Player.PlayerEnum.Player1, new MoodAtributes(saveData.p2gardenGoals[0],saveData.p2gardenGoals[1],saveData.p2gardenGoals[2]) }
            };

            newData.actionPoints = new Dictionary<Player.PlayerEnum, Dictionary<TurnPoints.PointType, int>>()
            {
                {
                    Player.PlayerEnum.Player0, new Dictionary<TurnPoints.PointType, int>()
                    {
                        { TurnPoints.PointType.SelfObjectPlace,     saveData.p1actionPoints[0] },
                        { TurnPoints.PointType.OtherObjectPlace,    saveData.p1actionPoints[1] }
                    }
                },
                {
                    Player.PlayerEnum.Player1, new Dictionary<TurnPoints.PointType, int>()
                    {
                        { TurnPoints.PointType.SelfObjectPlace,     saveData.p2actionPoints[0] },
                        { TurnPoints.PointType.OtherObjectPlace,    saveData.p2actionPoints[1] }
                    }
                }

            };


           // newData.plants is not set, newData.plantsToAdd was set in ctor


            newData.turnNumber = saveData.turnNumber;

            return newData;

        }
    }
}