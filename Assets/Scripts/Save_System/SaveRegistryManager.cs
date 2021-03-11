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
    /// Get the list of saved games that are in the registry file
    /// </summary>
    /// <returns>Will return null if registry empty or cannot be read</returns>
    public List<GameMetaData> GetStoredGames()
    {
        if (!RegistryValid) return null;

        return new List<GameMetaData>(Registry.games);

    }

    /// <summary>
    /// Delete a game from the registry. *Warning* This will not delete the actual game save file, only the referance to it.
    /// Calling this before deleting the game file will result in a resource leak
    /// </summary>
    /// <param name="gameID">The ID of the game to be deleted</param>
    /// <returns>If the action was sucessful</returns>
    public bool RemoveGame(string gameID)
    {
        if (!RegistryValid) return false;

        for (int i = 0; i < Registry.games.Length; i++)
        {
            GameMetaData game = Registry.games[i];
            if (game.gameID == gameID)
            {
                RemoveGameAt(i);
                return OverwiteRegistry();
            }
        }
        return false;
    }

    /// <summary>
    /// Add a new game file to the registry. *Warning* failure to call this function after creating a new save file will make the file inaccessible
    /// which will result in a resource leak
    /// </summary>
    /// <param name="game">The game to be stored</param>
    /// <returns>If the action was sucessful</returns>
    public bool AddNewGame(GameMetaData game)
    {
        if (!RegistryValid) return false;

        AddGame(game);
        return OverwiteRegistry();
    }


    private void AddGame(GameMetaData game)
    {
        var listOfGames = new List<GameMetaData>(Registry.games);
        listOfGames.Add(game);
        Registry.games = listOfGames.ToArray();
    }

    private void RemoveGameAt(int index)
    {
        var listOfGames = new List<GameMetaData>(Registry.games);
        listOfGames.RemoveAt(index);
        Registry.games = listOfGames.ToArray();
    }


    private bool RegistryValid => Registry != null && Registry.games.Length > 0;

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
        if (_registryCache != null && _registryCache.initialised != false) // data exists and is not empty
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
