using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaveSystemInternal;



public class _SaveDemonstration : MonoBehaviour
{
    GameMetaData newGame;

    // Start is called before the first frame update
    void Start()
    {
        /// Creates a new game, opens a random existing game, reads a value from it, alters the value and saves that alteration,
        /// closes the game, opens it up again, reads the value again 
        newGame = CreateNewGame();
        Demonstration();

    }

    private void Demonstration()
    {
        

        OpenGameRandomExistingGame();

        DisplayCurrentGameValue();
        IncrimentValueAndSaveGame();
        string gameID = SaveManager.Instance.CurrentlyOpenGame.gameID;
        CloseGame();
        OpenGame(gameID);
        DisplayCurrentGameValue();

    }

    // delete the game
    private void OnApplicationQuit()
    {
        DeleteGame(newGame);
    }

    private void CloseGame()
    {
        Debug.Log($"Closing game {SaveManager.Instance.CurrentlyOpenGame.gameID}");
        SaveManager.Instance.CloseGame();
        Debug.Log($"Curently open game: {(SaveManager.Instance.CurrentlyOpenGame != null ?SaveManager.Instance.CurrentlyOpenGame.ToString() : "None")}");
    }

    private void IncrimentValueAndSaveGame()
    {
        if (!SaveManager.Instance.GameOpen)
        {
            Debug.LogError("Game is not open");
            return;
        }

        SaveManager.Instance.OpenSaveGameData.testValue++;
        if (!SaveManager.Instance.SaveGame())
        {
            Debug.LogError("Error saving game");
            return;
        }

        Debug.Log("Saved game sucessfully");

    }

    private void DisplayCurrentGameValue()
    {
        if (!SaveManager.Instance.GameOpen)
        {
            Debug.LogError("There is currently not a game open");
            return;
        }

        Debug.Log($"Value is currently {SaveManager.Instance.OpenSaveGameData.testValue}");


    }

    private void OpenGameRandomExistingGame()
    {
        var games = SaveManager.Instance.GetAllStoredGames();
        if (games == null)
        {
            Debug.LogError("Error collecting games");
            return;
        }

        if (games.Count == 0)
        {
            Debug.LogWarning("There does not seem to be any stored games");
            return;
        }

        int index = UnityEngine.Random.Range(0, games.Count);

        OpenGame(games[index]);

    }

    private void ListExistingGames()
    {
        var games = SaveManager.Instance.GetAllStoredGames();
        if(games == null)
        {
            Debug.LogError("Error collecting games");
            return;
        }

        if(games.Count == 0)
        {
            Debug.LogWarning("There does not seem to be any stored games");
            return;
        }

        foreach (var game in games)
        {
            Debug.Log($"Game: {game.gameID}");
        }

       
    }

    private GameMetaData CreateNewGame()
    {
        GameMetaData newGame = SaveManager.Instance.CreateNewGame();
        if (newGame == null)
        {
            Debug.LogError("Game not created");
            return null;

        }

        Debug.Log($"New game {newGame.gameID} created");

        return newGame;
    }

    private void OpenGame(GameMetaData game)
    {
        if (!SaveManager.Instance.OpenGame(game))
        {
            Debug.LogError("Game not opened");
            return;
        }

        Debug.Log($"Opened {game.gameID} Sucessfully");

  
    }
    private void OpenGame(string gameID)
    {
        if (!SaveManager.Instance.OpenGame(gameID))
        {
            Debug.LogError("Game not opened");
            return;
        }

        Debug.Log($"Opened {gameID} Sucessfully");

  
    }

    private void DeleteGame(GameMetaData game)
    {
        Debug.Log($"Deleting game {game.gameID}");
        SaveManager.Instance.DeleteGame(game);
    }

    private void DeleteGame(string gameID)
    { 
        Debug.Log($"Deleting game {gameID}");
        SaveManager.Instance.DeleteGame(gameID);
    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
