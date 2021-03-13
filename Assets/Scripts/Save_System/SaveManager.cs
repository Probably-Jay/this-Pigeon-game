using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;

using SaveSystemInternal;

// Jay 11/03

/// <summary>
/// Layer of abstraction between the game data and the data sent to files. Manages <see cref="SaveGameManager"/> and <see cref="SaveRegistryManager"/>
/// </summary>
public class SaveManager : Singleton<SaveManager>
{

    /// <summary>
    /// Singleton referance to this object
    /// </summary>
    public new static SaveManager Instance { get => Singleton<SaveManager>.Instance; }


    /// <summary>
    /// A copy of the data in the currently open game file. Edit this <see cref="SaveData"/> and call <see cref="SaveGame"/> to save it.
    /// </summary>
    public SaveData GameData { get; private set; } = null;
    /// <summary>
    /// If there is currently a <see cref="SaveData"/> object available to read/write to
    /// </summary>
    public bool GameOpen => saveGameManager.GameOpen;

    /// <summary>
    /// The <see cref="SaveData"/> object available to read/write to. Will be null if <see cref="GameData"/> is false
    /// </summary>
    public GameMetaData CurrentlyOpenGame => saveGameManager.OpenGameMetaData;


    private readonly SaveRegistryManager saveRegistryManager = new SaveRegistryManager();
    private readonly SaveGameManager saveGameManager = new SaveGameManager();



    /// <summary>
    /// Initialise the singleton as required by <see cref="Singleton{T}"/>. Also crates a registry file if one does not exist.
    /// </summary>
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
    /// <returns>A <see cref="GameMetaData"/> object of the newly created game. Returns null if this fails.</returns>
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
    /// <returns>Bool value if this was sucessful or not</returns>
    public bool SaveGame()
    {
        if (!GameOpen) return false;

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
    /// <returns>Bool value if this was sucessful or not</returns>
    public bool OpenGame(GameMetaData game)
    {

        if (GameOpen)
        {
            Debug.LogWarning($"Closing game {CurrentlyOpenGame.gameID} in order to open game {game.gameID}");
        }

        bool sucess = saveGameManager.OpenSave(game);

        if (sucess)
        {
            GameData = new SaveData(saveGameManager.OpenGameData);  // deep-copy so can be altered safely
            return true;
        }
        else
        {
            Debug.LogWarning($"Game {game.gameID} cannot be opened or does not exist, deleteing game");
            DeleteGame(game);
        }


        return sucess;
    }

    // overload
    /// <summary>
    /// Opens a game and stores the game data in <see cref="GameData"/>
    /// </summary>
    /// <param name="game">The ID of the game to be opened</param>
    /// <returns>Bool value if this was sucessful or not</returns>
    public bool OpenGame(string gameID)
    {
        GameMetaData game = saveRegistryManager.GetGame(gameID);

        if(game == null)
        {
            Debug.LogError($"Game with ID {gameID} could not be located");
            return false;
        }

        return OpenGame(game);
    }

    /// <summary>
    /// Get the list of saved games that are in the registry file. Will return null if registry file does not exist
    /// </summary>
    /// <returns>Will return null if registry empty or cannot be read</returns>
    public List<GameMetaData> GetAllStoredGames() => saveRegistryManager.GetAllStoredGames();

    /// <summary>
    /// Nullifies the referances to the open game. This does not need to be called and does not release any locked resources
    /// </summary>
    public void CloseGame()
    {
        GameData = null;
        saveGameManager.CloseGame();
    }

    /// <summary>
    /// Delete a save game and remove it from the registry
    /// </summary>
    /// <param name="gameID">The ID of the game to remove</param>
    public void DeleteGame(string gameID) => DeleteGame(saveRegistryManager.GetGame(gameID));

    //overload
    /// <summary>
    /// Delete a save game and remove it from the registry
    /// </summary>
    /// <param name="gameID">The game to remove</param>
    public void DeleteGame(GameMetaData game)
    {
        saveGameManager.DeleteGame(game);
        saveRegistryManager.RemoveDeletedGame(game.gameID);
    }

    #region Internal Functions

    private GameMetaData CreateNewGameData()
    {
        GameMetaData newGame = new GameMetaData();
        string TimeID = DateTime.Now.Ticks.ToString();
        string UserID = SystemInfo.deviceUniqueIdentifier != SystemInfo.unsupportedIdentifier ? SystemInfo.deviceUniqueIdentifier : GenerateQuasiUniqueIdentifier();
        newGame.gameID = UserID + "-"+ TimeID;
        return newGame;
    }

    private string GenerateQuasiUniqueIdentifier() // fallback function, hopefully never used
    {
        Debug.LogError($"System does not support {nameof(SystemInfo.deviceUniqueIdentifier)}.");
        string unhashedID = SystemInfo.deviceName + SystemInfo.deviceType.ToString() + SystemInfo.deviceModel; // hack
        byte[] UserIDbytes;
        using (HashAlgorithm algorithm = SHA256.Create())
            UserIDbytes = algorithm.ComputeHash(System.Text.Encoding.UTF8.GetBytes(unhashedID + "salt"));
        return System.Text.Encoding.UTF8.GetString(UserIDbytes);
    }

    #endregion




}
