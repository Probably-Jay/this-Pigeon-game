using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Security.Cryptography;
using System;


    // Jay 11/03
namespace SaveSystemInternal
{
    /// <summary>
    /// Static class responsible for serialising the <see cref="SaveGameData"/> data. Controlled by <see cref="SaveGameManager"/>. See also <see cref="SaveGameRegistrySerialiser"/>
    /// </summary>
    public static class SaveDataSerialiser
    {
        const string extention = ".budSave";
        public static string SavePath => Path.Combine(Application.persistentDataPath, "gameSaves");

        /// <summary>
        /// If a game file exists
        /// </summary>
        /// <param name="localGameID">The ID of the game in question</param>
        /// <returns>If a game exists under that ID</returns>
        public static bool GameExists(string localGameID) => FileExists(GetFilePath(localGameID));


        /// <summary>
        /// Get the path to a specific save file
        /// </summary>
        /// <param name="localGameID"></param>
        private static string GetFilePath(string localGameID) => Path.Combine(SavePath, GetFileName(localGameID));

        /// <summary>
        /// Get the filename that this save file will use
        /// </summary>
        private static string GetFileName(string localGameID, string modifier = "-main") => $"{localGameID}{modifier}{extention}";

        private static bool FileExists(string path) => Directory.Exists(SavePath) && File.Exists(path);

        public static bool SaveGame(string localGameID, SaveGameData data)
        {
            string path = GetFilePath(localGameID);

            return Savedata(path, data);
        }


        /// <summary>
        /// Creates a new empty save file using the gameID. If a file already exists there this has no effect.
        /// </summary>
        /// <param name="localGameID"></param>
        public static bool CreateNewSaveFile(string localGameID)
        {
            // if folder does not exist, create it
            if (!Directory.Exists(SavePath))
            {
                Directory.CreateDirectory(SavePath);
            }

            string path = GetFilePath(localGameID);

            if (File.Exists(path)) return false;

            SaveGameData newSave = new SaveGameData(localGameID);


            SetHash(newSave);

            var jsonData = JsonUtility.ToJson(newSave);

            File.WriteAllText(path, jsonData); // will overwrite file there and then closes it

            return true;
        }

        public static SaveGameData LoadGame(string localGameID)
        {
            string path = GetFilePath(localGameID);
            return LoadData(path);
        }




        private static bool Savedata(string path, SaveGameData data)
        {
            if (!FileExists(path)) return false;

            SetHash(data);

            var jsonData = JsonUtility.ToJson(data);

            File.WriteAllText(path, jsonData); // will overwrite file there and then closes it

            return true;
        }



        /// <summary>
        /// Set a checksum hash based on the current data, this should remain the same if calculated on the same data, and so can be used to make sure file has not been altered / corrupted
        /// </summary>
        private static void SetHash(SaveGameData data)
        {
            data.hash = null; // reset hash
            var jsonData = JsonUtility.ToJson(data); // get the json of the data without the hash

            using (HashAlgorithm algorithm = SHA256.Create())
                data.hash = algorithm.ComputeHash(System.Text.Encoding.UTF8.GetBytes(jsonData + "salt")); // use the json to set the hash
        }

        private static SaveGameData LoadData(string path)
        {
            if (!FileExists(path))
            {
                Debug.LogError($"Save file at {path} does not exist");
                return null;
            }

            try
            {
                var jsonData = File.ReadAllText(path);

                SaveGameData data = JsonUtility.FromJson<SaveGameData>(jsonData);

                //if (data == new SaveData())
                //{
                //    Debug.LogError("Save is empty");
                //    throw new Exception(); // this will move to catch
                //}

                if (!ValidateHash(data))
                {
                    Debug.LogError("Save file is corrupt");
                    // this maybe should do nothing
                }

                return data;
            }
            catch
            {
                Debug.LogError($"Error loading save at {path}");
                return null;
            }
        }

        /// <summary>
        /// Validates if the checksum hash based on the current data matches the one stored with this data when it was last serialised.
        /// This should remain the same if calculated on the same data and so is used to make sure file has not been altered / corrupted.
        /// This will only be meaningful just after a strucutre is deserialised from a file. <see cref="SaveGameData.hash"/> is unaltered by this function.
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

        public static bool DeleteFile(string localGameID)
        {
            string path = GetFilePath(localGameID);
            if (!FileExists(path)) return false;


            File.Delete(path);
            return true;

        }



    }
}