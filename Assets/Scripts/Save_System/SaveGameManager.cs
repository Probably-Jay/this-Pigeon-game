using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameManager
{

    public GameMetaData OpenGameMetaData { get; private set; } = null;
    public SaveData OpenGameData { get; private set; } = null;

    public bool GameOpen => OpenGameData != null;

    /// <summary>
    /// Creates a new empty save file using the gameID. If a file already exists there this has no effect.
    /// </summary>
    /// <param name="newGameID">Newly generated unique game ID</param>
    public bool CreateNewSaveFile(string newGameID) => SaveDataSerialiser.CreateNewSaveFile(newGameID);


    /// <summary>
    /// Load in the contents of the save file to <see cref="OpenGameData"/>. The paramater <paramref name="game"/> must be gained from <see cref="SaveRegistryManager.GetAllStoredGames"/>. (open does not imply any kind of resource lock)
    /// </summary>
    /// <param name="game">Data class on a save file, gained from <see cref="SaveRegistryManager.GetAllStoredGames"/></param>
    /// <returns></returns>
    public bool OpenSave(GameMetaData game)
    {
        OpenGameData = SaveDataSerialiser.LoadGame(game.gameID);
        if (OpenGameData != null) // data exists
        {
            // could check hash here
            OpenGameMetaData = game;
            return true;
        }
        else return false;
    }




    /// <summary>
    /// This is the primary function in saving data. This will set the <see cref="OpenGameData"/> to the value of <paramref name="data"/>.
    /// *Warrning* This will not actually save the game data. You must call <see cref="OverwriteSaveFile"/>
    /// </summary>
    public bool StageSaveData(SaveData data)
    {
        if (!GameOpen || data == null) return false;

        OpenGameData = data;
        return true;

    }

    /// <summary>
    /// Overwrites the open save file with the value of <see cref="OpenGameData"/> and then nullifies the referance to this file
    /// </summary>
    public bool OverwriteSaveFileAndCloseGame()
    {
        if (!GameOpen) return false;

        bool sucess = OverwriteSaveFile();
        CloseGame();

        return sucess;
    }

    /// <summary>
    /// Overwrites the open save file with the value of <see cref="OpenGameData"/>
    /// </summary>
    public bool OverwriteSaveFile()
    {
        if (!GameOpen) return false;

        SaveDataSerialiser.SaveGame(OpenGameMetaData.gameID, OpenGameData);
        return true;
    }

    /// <summary>
    /// nullifies the referance to the open file (open does not imply a resources lock)
    /// </summary>
    public void CloseGame()
    {
        OpenGameData = null;
        OpenGameMetaData = null;
    }

    /// <summary>
    /// Delete a provided game if it exists. If the game provided is currenly open then it is closed
    /// </summary>
    public bool DeleteGame(GameMetaData game)
    {
        if (GameOpen && OpenGameMetaData.gameID == game.gameID) CloseGame();

        return SaveDataSerialiser.DeleteFile(game.gameID);
    }
}
