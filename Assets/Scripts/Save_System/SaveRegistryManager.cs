using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// created Jay 11/03

/// <summary>
/// Class which handles the save registry file, responsible for storing metadata about saved games including thier ID and filepath
/// </summary>
public class SaveRegistryManager
{
    /// <summary>
    /// The location the registry file should be stored
    /// </summary>
    public string RegistryFilePath => SaveGameRegistrySerialiser.RegistrySavePath;

    /// <summary>
    /// Creates the registry file if it does not exist, else it does nothing
    /// </summary>
    public bool CreateRegistryFile() => SaveGameRegistrySerialiser.CreateRegistryFile();



    /// <summary>
    /// Get the list of saved games that are in the registry file
    /// </summary>
    /// <returns>Will return null if registry empty or cannot be read</returns>
    public List<GameMetaData> GetAllStoredGames()
    {
        if (!RegistryExists) return null;

        return new List<GameMetaData>(Registry.games); // return a copy of it
    }


    /// <summary>
    /// Get a saved game from the registry file by game ID
    /// </summary>
    /// <returns>Will return null if registry empty or cannot be read</returns>
    public GameMetaData GetGame(string gameID)
    {
        if (!RegistryNotEmpty) return null;

        for (int i = 0; i < Registry.games.Length; i++)
        {
            GameMetaData game = Registry.games[i];
            if (game.gameID == gameID)
            {
                return game;
            }
        }
        return null;
    }





    /// <summary>
    /// Remove a game from the registry. *Warning* This will not delete the actual game save file, only the referance to it.
    /// Calling this before deleting the game file will result in a resource leak
    /// </summary>
    /// <param name="gameID">The ID of the game to be deleted</param>
    /// <returns>If the action was sucessful</returns>
    public bool RemoveDeletedGame(string gameID)
    {
        if (!RegistryNotEmpty) return false;


        GameMetaData game = GetGame(gameID);
        return RemoveDeletedGame(game);
    }

    /// <summary>
    /// Remove a game from the registry. *Warning* This will not delete the actual game save file, only the referance to it.
    /// Calling this before deleting the game file will result in a resource leak
    /// </summary>
    /// <param name="gameID">The ID of the game to be deleted</param>
    /// <returns>If the action was sucessful</returns>
    public bool RemoveDeletedGame(GameMetaData game)
    {
        bool sucess = true;
        sucess &= RemoveGame(game);
        sucess &= OverwiteRegistry();

        return sucess;
    }

    /// <summary>
    /// Add a new game file to the registry. This function does not create a save game. *Warning* Failure to call this function after creating a new save file will make the file inaccessible
    /// which will result in a resource leak
    /// </summary>
    /// <param name="game">The game to be stored</param>
    /// <returns>If the action was sucessful</returns>
    public bool AddNewGame(GameMetaData game)
    {
        if (!RegistryExists) return false;

        AddGame(game);
        return OverwiteRegistry();
    }


    private void AddGame(GameMetaData game)
    {
        var listOfGames = new List<GameMetaData>(Registry.games);
        listOfGames.Add(game);
        Registry.games = listOfGames.ToArray();
    }

    private bool RemoveGame(GameMetaData game)
    {
        var listOfGames = new List<GameMetaData>(Registry.games);
        bool sucess = listOfGames.Remove(game);
        Registry.games = listOfGames.ToArray();
        return sucess;
    }


    private bool RegistryExists => Registry != null;
    private bool RegistryNotEmpty => RegistryExists && Registry.games.Length > 0;

    private SaveGameRegistryData _registryCache = null; 

    /// <summary>
    /// The Registy data, returns cache or loads from file
    /// </summary>
    private SaveGameRegistryData Registry
    {
        get
        {
            if (_registryCache != null)
                return _registryCache;
            else
                return LoadRegistry();
        }
    }

    /// <summary>
    /// Load and cache the registry
    /// </summary>
    private SaveGameRegistryData LoadRegistry()
    {
        _registryCache = SaveGameRegistrySerialiser.LoadRegistry();
        if (_registryCache != null ) // data exists
        {
            // could check hash here
            return _registryCache;
        }
        else return null;
    }

    /// <summary>
    /// Overwrite the file data with the current cache of the data, if cache is null, does nothing
    /// </summary>
    /// <returns>If overwrite was sucessfull</returns>
    private bool OverwiteRegistry()
    {
        if (_registryCache == null) return false;

        SaveGameRegistrySerialiser.SaveGameRegistry(_registryCache);
        return true;
    }



}
