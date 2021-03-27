using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using Plants;

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

            newData.p1gardenGoals = liveData.gardenGoals[Player.PlayerEnum.Player1].DeepCopyValues();
            newData.p2gardenGoals = liveData.gardenGoals[Player.PlayerEnum.Player2].DeepCopyValues();

            newData.p1actionPoints = new int[TurnPoints.NumberOfPointTypes];
            newData.p2actionPoints = new int[TurnPoints.NumberOfPointTypes];
            foreach (TurnPoints.PointType type in System.Enum.GetValues(typeof(TurnPoints.PointType)))
            {
                newData.p1actionPoints[(int)type] = liveData.actionPoints[Player.PlayerEnum.Player1][type];
                newData.p2actionPoints[(int)type] = liveData.actionPoints[Player.PlayerEnum.Player2][type];
            }

            newData.plants = new SerialisedPlantData[liveData.plants.Count];
            for (int i = 0; i < liveData.plants.Count; i++)
            {
                Plant plant = liveData.plants[i];
                newData.plants[i] = GetSerialisablePlant(plant);
            }

            newData.turnNumber = liveData.turnNumber;


            return newData;
        }

        private static SerialisedPlantData GetSerialisablePlant(Plant plant)
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

            newData.gardenGoals = new Dictionary<Player.PlayerEnum, MoodAttributes>()
            {
                {Player.PlayerEnum.Player1, new MoodAttributes(saveData.p1gardenGoals[0],saveData.p1gardenGoals[1],saveData.p1gardenGoals[2]) },
                {Player.PlayerEnum.Player2, new MoodAttributes(saveData.p2gardenGoals[0],saveData.p2gardenGoals[1],saveData.p2gardenGoals[2]) }
            };

            newData.actionPoints = new Dictionary<Player.PlayerEnum, Dictionary<TurnPoints.PointType, int>>()
            {
                {
                    Player.PlayerEnum.Player1, new Dictionary<TurnPoints.PointType, int>()
                    {
                        { TurnPoints.PointType.SelfObjectPlace,     saveData.p1actionPoints[0] },
                        { TurnPoints.PointType.OtherObjectPlace,    saveData.p1actionPoints[1] }
                    }
                },
                {
                    Player.PlayerEnum.Player2, new Dictionary<TurnPoints.PointType, int>()
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


        /// <summary>
        /// Validates if the checksum hash based on the current data matches the one stored with this data when it was last serialised.
        /// This should remain the same if calculated on the same data and so is used to make sure file has not been altered / corrupted.
        /// This will only be meaningful just after a strucutre is deserialised from a file. <paramref name="data"/>'s <see cref="SaveGameData.hash"/> is unaltered by this function.
        /// </summary>
        /// <param name="data">The structure to be validated</param>
        /// <returns>If the hashes match (the data is the same and not-corrupted)</returns>
        public static bool ValidateHash(SaveGameData data)
        {
            byte[] previousHash = (byte[])data.hash.Clone(); // deep copy
            SetHash(data);

            for (int i = 0; i < previousHash.Length; i++)
            {
                if (previousHash[i] != data.hash[i])
                {
                    data.hash = previousHash; // keep this unchanged
                    return false;
                }
            }
            data.hash = previousHash; // keep this unchanged
            return true;
        }

        /// <summary>
        /// Overload for <see cref="SaveGameRegistryData"/>. Validates if the checksum hash based on the current data matches the one stored with this data when it was last serialised.
        /// This should remain the same if calculated on the same data and so is used to make sure file has not been altered / corrupted.
        /// This will only be meaningful just after a strucutre is deserialised from a file. <paramref name="data"/>'s <see cref="SaveGameData.hash"/> is unaltered by this function.
        /// </summary>
        /// <param name="data">The structure to be validated</param>
        /// <returns>If the hashes match (the data is the same and not-corrupted)</returns>
        public static bool ValidateHash(SaveGameRegistryData data)
        {
            byte[] previousHash = (byte[])data.hash.Clone();
            SetHash(data);

            for (int i = 0; i < previousHash.Length; i++)
            {
                if (previousHash[i] != data.hash[i])
                {
                    data.hash = previousHash; // keep this unchanged
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Set a checksum hash based on the current data, this should remain the same if calculated on the same data, and so can be used to make sure file has not been altered / corrupted
        /// </summary>
        public static void SetHash(SaveGameData data)
        {
            data.hash = null; // reset hash
            var jsonData = JsonUtility.ToJson(data); // get the json of the data without the hash

            using (HashAlgorithm algorithm = SHA256.Create())
                data.hash = algorithm.ComputeHash(System.Text.Encoding.UTF8.GetBytes(jsonData + "salt")); // use the json to set the hash
        }

        /// <summary>
        /// Overload for <see cref="SaveGameRegistryData"/>. Set a checksum hash based on the current data, this should remain the same if calculated on the same data, and so can be used to make sure file has not been altered
        /// </summary>
        public static void SetHash(SaveGameRegistryData data)
        {
            data.hash = null; // reset hash
            var jsonData = JsonUtility.ToJson(data); // get the json of the data without the hash

            using (HashAlgorithm algorithm = SHA256.Create())
                data.hash = algorithm.ComputeHash(System.Text.Encoding.UTF8.GetBytes(jsonData + "salt")); // use the json to set the hash
        }
    }
}