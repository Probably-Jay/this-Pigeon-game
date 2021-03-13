﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Security.Cryptography;
using System;

// jay 11/03
namespace SaveSystemInternal
{
    public static class SaveGameRegistrySerialiser
    {
        const string registryFile = "directoryFile.gameDirectorySave";
        public static string RegistrySavePath => Path.Combine(Application.persistentDataPath, "registry");
        private static string FilePath => Path.Combine(RegistrySavePath, registryFile);

        public static bool RegistryFileExists => Directory.Exists(RegistrySavePath) && File.Exists(FilePath);




        /// <summary>
        /// Attempt to create the registry file, if it already exists this function has no effect
        /// </summary>
        public static bool CreateRegistryFile()
        {
            if (!Directory.Exists(RegistrySavePath))
            {
                Directory.CreateDirectory(RegistrySavePath);
            }

            if (RegistryFileExists) return false;

            SaveGameRegistryData saveGameRegistry = new SaveGameRegistryData();

            SetHash(saveGameRegistry);

            var jsonData = JsonUtility.ToJson(saveGameRegistry);

            File.WriteAllText(FilePath, jsonData); // will create or overwrite file there and then closes file

            return true;
        }

        /// <summary>
        /// Overwrite the registry file with the provided data
        /// </summary>
        public static bool SaveGameRegistry(SaveGameRegistryData data)
        {
            if (!RegistryFileExists) return false;

            SetHash(data);

            var jsonData = JsonUtility.ToJson(data);

            File.WriteAllText(FilePath, jsonData); // will create or overwrite file there and then closes file

            return true;
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


        /// <summary>
        /// Load the registry file. Will return null if file is not available
        /// </summary>
        /// <returns></returns>
        public static SaveGameRegistryData LoadRegistry()
        {
            if (!RegistryFileExists)
            {
                Debug.LogError($"Registry file does not exist");
                return null;
            }

            try
            {
                var jsonData = File.ReadAllText(FilePath);

                SaveGameRegistryData data = JsonUtility.FromJson<SaveGameRegistryData>(jsonData);

                if (!ValidateHash(data))
                {
                    Debug.LogError("Registry file is corrupt");
                    // this maybe should do nothing
                }

                return data;
            }
            catch
            {
                Debug.LogError($"Error loading registry");
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
}