using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Security.Cryptography;
using System;

public class SaveGameRegistrySerialiser
{
    const string registryFile = "directoryFile.gameDirectorySave";
    public static string RegistrySavePath => Path.Combine(Application.persistentDataPath, "registry");
    private static string FilePath => Path.Combine(RegistrySavePath, registryFile);


    public static void SaveGameRegistry(SaveGameRegistryData data) => Savedata(data, FilePath);


    public static SaveGameRegistryData LoadRegistry() => LoadData(FilePath);



    private static void Savedata(SaveGameRegistryData data, string path)
    {
        if (!Directory.Exists(RegistrySavePath))
        {
            Directory.CreateDirectory(RegistrySavePath);
        }

        SetHash(data);

        var jsonData = JsonUtility.ToJson(data);

        File.WriteAllText(path, jsonData); // will create or overwrite file there and then closes file
    }

    /// <summary>
    /// Set a checksum hash based on the current data, this should remain the same if calculated on the same data, and so can be used to make sure file has not been altered
    /// </summary>
    private static void SetHash(SaveGameRegistryData data)
    {
        data.hash = null; // reset hash
        var jsonData = JsonUtility.ToJson(data); // get the json of the data without the hash

        using (HashAlgorithm algorithm = SHA256.Create())
            data.hash = algorithm.ComputeHash(System.Text.Encoding.UTF8.GetBytes(jsonData + "salt")); // use the json to set the hash
    }

    private static SaveGameRegistryData LoadData(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogError($"Save file at {path} does not exist");
            return null;
        }

        try
        {
            var jsonData = File.ReadAllText(path);

            SaveGameRegistryData data = JsonUtility.FromJson<SaveGameRegistryData>(jsonData);

            if (data == new SaveGameRegistryData())
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

    public static bool ValidateHash(SaveGameRegistryData data)
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

}
