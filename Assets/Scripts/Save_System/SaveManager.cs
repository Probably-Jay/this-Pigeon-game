using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Jay 11/03

/// <summary>
/// Layer of abstraction between the game data and the data sent to files. Manages <see cref="SaveGameManager"/> and <see cref="SaveRegistryManager"/>
/// </summary>
public class SaveManager : Singleton<SaveManager>
{

    public new static SaveManager Instance { get => Singleton<SaveManager>.Instance; }


    /// <summary>
    /// A copy of the data in the currently open game file. Edit this <see cref="SaveData"/> and call <see cref="SaveGame"/> to save it.
    /// </summary>
    public SaveData GameData { get; private set; } = null;


    private readonly SaveRegistryManager saveRegistryManager = new SaveRegistryManager();
    private readonly SaveGameManager saveGameManager = new SaveGameManager();




    public override void Initialise()
    {
        base.InitSingleton();

        if(saveRegistryManager.CreateRegistryFile()) // Attempt to create the registry file{
        {
            Debug.LogWarning($"Registry file not found, creating new registry file at {saveRegistryManager.RegistryFilePath}");
        }
        else
        {
            Debug.Log("Registry file found");
        }
    }



    
    /// <summary>
    /// Create a new game file and add it to the registry. This will not open the game.
    /// </summary>
    /// <returns></returns>
    public GameMetaData CreateNewGame()
    {
        GameMetaData newGame = CreateNewGameData();

        // return null if any step fails
        if (saveRegistryManager.AddNewGame(newGame))
        {
            if (saveGameManager.CreateNewSaveFile(newGame))
            {
                return newGame;
            }
            else // file was not created
            {
                saveRegistryManager.RemoveDeletedGame(newGame);
                return null;
            }
        }
        else return null; // file data could not be added to registry, don't create it
    }


    /// <summary>
    /// Overwrites the currently open game with the value of <see cref="GameData"/>
    /// </summary>
    public bool SaveGame()
    {
        if (!saveGameManager.GameOpen) return false;

        if (saveGameManager.StageSaveData(GameData))
        {
            saveGameManager.OverwriteSaveFile();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Opens a game and stores the game data in <see cref="GameData"/>
    /// </summary>
    /// <param name="game">The metadata of the game to be opened</param>
    public bool OpenGame(GameMetaData game)
    {
        bool sucess = saveGameManager.OpenSave(game);

        GameData = sucess ? new SaveData(saveGameManager.OpenGameData) : null; // deep-copy so can be altered safely (only if opened)

        return sucess;
    }

    /// <summary>
    /// Get the list of saved games that are in the registry file. Will return null if registry file does not exist
    /// </summary>
    /// <returns>Will return null if registry empty or cannot be read</returns>
    public List<GameMetaData> GetAllStoredGames() => saveRegistryManager.GetAllStoredGames();

    /// <summary>
    /// Delete a save game and remove it from the registry
    /// </summary>
    /// <param name="gameID">The ID of the game to remove</param>
    public void DeleteGame(string gameID) => DeleteGame(saveRegistryManager.GetGame(gameID));

    /// <summary>
    /// Delete a save game and remove it from the registry
    /// </summary>
    /// <param name="gameID">The game to remove</param>
    public void DeleteGame(GameMetaData game)
    {
        saveRegistryManager.RemoveDeletedGame(game.gameID);
        saveGameManager.DeleteGame(game);
    }

    private GameMetaData CreateNewGameData()
    {
        GameMetaData newGame = new GameMetaData();
        string TimeID = DateTime.Now.Ticks.ToString();
        string UserID = SystemInfo.deviceUniqueIdentifier;
        newGame.gameID = UserID + TimeID;
        return newGame;
    }


    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
       
    }

   

  

}
