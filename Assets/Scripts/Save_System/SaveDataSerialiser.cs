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
    //private static string TempSavePath => Path.Combine(Application.temporaryCachePath, "saveData");


    public static void SaveGame(string localGameID, SaveData data)
    {
        string path = GetFilePath(localGameID);

        Savedata(data, path);
    }

    //public static void SaveGameTemp(int localGameID, SaveData data)
    //{
    //    string path = GetTempFilePath(localGameID);

    //    Savedata(data, path);
    //}

 

    public static SaveData LoadGame(string localGameID)
    {
        string path = GetFilePath(localGameID);
        return LoadData(path);
    }

    //public static SaveData LoadGameTemp(int localGameID)
    //{
    //    string path = GetTempFilePath(localGameID);
    //    return LoadData(path);
    //}



    private static void Savedata(SaveData data, string path)
    {
        if (!Directory.Exists(SavePath))
        {
            Directory.CreateDirectory(SavePath);
        }

        SetHash(data);

        var jsonData = JsonUtility.ToJson(data);


        File.WriteAllText(path, jsonData); // will create or overwrite file there and then closes file

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
        if (!File.Exists(path))
        {
            Debug.LogError($"Save file at {path} does not exist");
            return null;
        }

        try
        {
            var jsonData = File.ReadAllText(path);

            SaveData data = JsonUtility.FromJson<SaveData>(jsonData);

            if (data == new SaveData())
            {
                Debug.LogError("Save is empty");
                throw new Exception(); // this will move to catch
            }

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

    public static void Wipe(string localGameID)
    {
        string path = GetFilePath(localGameID);

        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
    //public static void WipeTemp(int localGameID)
    //{
    //    string path = GetTempFilePath(localGameID);

    //    if (File.Exists(path))
    //    {
    //        File.Delete(path);
    //    }
    //}

    private static string GetFilePath(string localGameID)
    {
        return Path.Combine(SavePath, GetFileName(localGameID));
    }

    //private static string GetTempFilePath(int localGameID)
    //{
    //    //return Path.Combine(SavePath, GetFileName(localGameID, "_temp"));
    //     return Path.Combine(TempSavePath, GetFileName(localGameID, "_temp")); 
    //}

    /// <summary>
    /// Get the filename that this file will use
    /// </summary>
    /// <param name="localGameID"></param>
    /// <param name="modifier"></param>
    /// <returns></returns>
    private static string GetFileName(string localGameID, string modifier = "") => $"Save_{localGameID}{modifier}{extention}";

}
