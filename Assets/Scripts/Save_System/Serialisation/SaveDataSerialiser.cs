using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;



    // Jay 11/03
namespace SaveSystemInternal
{
    /// <summary>
    /// Static class responsible for serialising the <see cref="SaveGameData"/> data. Controlled by <see cref="SaveGameManager"/>. See also <see cref="SaveGameRegistrySerialiser"/>
    /// </summary>
    public static class SaveDataSerialiser
    {
        const string extention = ".budSave";
        /// <summary>
        /// The path to the folder where save games are stored
        /// </summary>
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

      
        /// <summary>
        /// Creates a new empty save file using the gameID. If a file already exists there this has no effect.
        /// </summary>
        /// <param name="localGameID"></param>
        /// <returns>If a new save file was created</returns>
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


            SaveDataUtility.SetHash(newSave);

            var jsonData = JsonUtility.ToJson(newSave);

            File.WriteAllText(path, jsonData); // will overwrite file there and then closes it

            return true;
        }

        /// <summary>
        /// Save the data provided to the save file given by the <paramref name="localGameID"/>
        /// </summary>
        /// <param name="localGameID">The ID of the game file to be saved into</param>
        /// <param name="data">The data to save</param>
        /// <returns>If the saving was sucessful</returns>
        public static bool SaveGame(string localGameID, SaveGameData data)
        {
            string path = GetFilePath(localGameID);

            return Savedata(path, data);
        }

        /// <summary>
        /// Load game data from game file by the provided <paramref name="localGameID"/>
        /// </summary>
        /// <param name="localGameID">The ID of the game file to be loaded</param>
        /// <returns>The data from the file in a <see cref="SaveGameData"/> structure</returns>
        public static SaveGameData LoadGame(string localGameID)
        {
            string path = GetFilePath(localGameID);
            return LoadData(path);
        }




        private static bool Savedata(string path, SaveGameData data)
        {
            if (!FileExists(path)) return false;

            SaveDataUtility.SetHash(data);

            var jsonData = JsonUtility.ToJson(data);

            File.WriteAllText(path, jsonData); // will overwrite file there and then closes it

            return true;
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

                if (!SaveDataUtility.ValidateHash(data))
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
        /// Deletes a save file with the provied <paramref name="localGameID"/>
        /// </summary>
        /// <param name="localGameID">The ID of the save file to be deleted</param>
        /// <returns>If the file to be deleted previously existed (the deletion operation itself never fails)</returns>
        public static bool DeleteFile(string localGameID)
        {
            string path = GetFilePath(localGameID);
            if (!FileExists(path)) return false;


            File.Delete(path);
            return true;

        }



    }
}