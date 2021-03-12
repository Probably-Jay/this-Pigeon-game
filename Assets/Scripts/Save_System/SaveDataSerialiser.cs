using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Security.Cryptography;
using System;

// Jay 11/03

public static class SaveDataSerialiser
{
    const string extention = ".budSave";
    public static string SavePath => Path.Combine(Application.persistentDataPath, "gameSaves");

    /// <summary>
    /// Get the path to a specific save file
    /// </summary>
    /// <param name="localGameID"></param>
    private static string GetFilePath(string localGameID) => Path.Combine(SavePath, GetFileName(localGameID));

    /// <summary>
    /// Get the filename that this save file will use
    /// </summary>
    private static string GetFileName(string localGameID, string modifier = "") => $"Save_{localGameID}{modifier}{extention}";


    private static bool FileExists(string path) => Directory.Exists(SavePath) && File.Exists(path);

    public static bool SaveGame(string localGameID, SaveData data)
    {
        string path = GetFilePath(localGameID);

        return Savedata(path, data);
    }


    /// <summary>
    /// Creates a new empty save file using the gameID. If a file already exists there this has no effect.
    /// </summary>
    /// <param name="localGameID"></param>
    public static void CreateNewSaveFile(string localGameID)
    {
        if (!Directory.Exists(SavePath))
        {
            Directory.CreateDirectory(SavePath);
        }

        string path = GetFilePath(localGameID);

        if (File.Exists(path)) return;

        SaveGame(path, default);
    }

    public static SaveData LoadGame(string localGameID)
    {
        string path = GetFilePath(localGameID);
        return LoadData(path);
    }




    private static bool Savedata(string path, SaveData data)
    {
        if (!FileExists(path)) return false;

        SetHash(data);

        var jsonData = JsonUtility.ToJson(data);

        File.WriteAllText(path, jsonData); // will overwrite file there and then closes it

        return true;
    }



    /// <summary>
    /// Set a checksum hash based on the current data, this should remain the same if calculated on the same data, and so can be used to make sure file has not been altered
    /// </summary>
    private static void SetHash(SaveData data)
    {
        data.hash = null; // reset hash
        var jsonData = JsonUtility.ToJson(data); // get the json of the data without the hash

        using (HashAlgorithm algorithm = SHA256.Create())
            data.hash = algorithm.ComputeHash(System.Text.Encoding.UTF8.GetBytes(jsonData + "salt")); // use the json to set the hash
    }

    private static SaveData LoadData(string path)
    {
        if (!FileExists(path))
        {
            Debug.LogError($"Save file at {path} does not exist");
            return null;
        }

        try
        {
            var jsonData = File.ReadAllText(path);

            SaveData data = JsonUtility.FromJson<SaveData>(jsonData);

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


    public static bool ValidateHash(SaveData data)
    {
        var hashFromFile = data.hash;
        SetHash(data);

        for (int i = 0; i < hashFromFile.Length; i++)
        {
            if (hashFromFile[i] != data.hash[i])
            {
                data.hash = hashFromFile; // keep this unchanged
                return false;
            }
        }
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
