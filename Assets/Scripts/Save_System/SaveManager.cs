using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Jay 11/03

public class SaveManager : Singleton<SaveManager>
{

    public new static SaveManager Instance { get => Singleton<SaveManager>.Instance; }

    private SaveRegistryManager saveRegistryManager = new SaveRegistryManager();


    /// <summary>
    /// The current save data exists and is initialised
    /// </summary>
    public static bool SaveValid => SaveExists && Data.HashIsValid;

    /// <summary>
    /// The current save data exists
    /// </summary>
    public static bool SaveExists => Data != null && Data != new SaveData();


    public override void Initialise()
    {
        base.InitSingleton();
    }

    private void Awake()
    {
        Data = new .aveData();
    }



    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
       
    }

    private SaveData saveData;
    public static SaveData Data { get => Instance.saveData; private set => Instance.saveData = value; }

    public static int SaveNumber { get; set; } = 0;

    /// <summary>
    /// Save game from the current <see cref="Data"/>
    /// </summary>
    public static void SaveExplicit() => SaveDataSerialiser.SaveGame(SaveNumber, Data);

    /// <summary>
    /// Load the save file into <see cref="Data"/>
    /// </summary>
    public static void Load()
    {
        SaveData _data = SaveDataSerialiser.LoadGame(SaveNumber);
        if (_data != null) Data = _data;
        else { Data.initialised = false; Debug.LogError("Save data does not exist"); }
    }

    //public static void DeleteTemp() => JsonSerialiser.WipeTemp(SaveNumber);

    public static void ClearData() => Data = new SaveData();
}
