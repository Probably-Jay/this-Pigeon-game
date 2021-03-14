using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Security.Cryptography;
using System;

// jay 11/03
namespace SaveSystemInternal
{
    /// <summary>
    /// Static class responsible for serialising the <see cref="SaveGameRegistryData"/> data. Controlled by <see cref="SaveRegistryManager"/>. See also <see cref="SaveDataSerialiser"/>
    /// </summary>
    public static class SaveGameRegistrySerialiser
    {
        const string registryFile = "directoryFile.gameDirectorySave";
        /// <summary>
        /// The path to the folder where the registry file is stored
        /// </summary>
        public static string RegistrySavePath => Path.Combine(Application.persistentDataPath, "registry");
        private static string FilePath => Path.Combine(RegistrySavePath, registryFile);

        /// <summary>
        /// If the registry file exists
        /// </summary>
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

            SaveDataUtility.SetHash(saveGameRegistry);

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

            SaveDataUtility.SetHash(data);

            var jsonData = JsonUtility.ToJson(data);

            File.WriteAllText(FilePath, jsonData); // will create or overwrite file there and then closes file

            return true;
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

                if (!SaveDataUtility.ValidateHash(data))
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

       

    }
}