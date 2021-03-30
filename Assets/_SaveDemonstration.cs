using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaveSystemInternal;
using Plants;


/// <summary>
/// Demonstration of how to use the save system in any part of the project
/// </summary>
public class _SaveDemonstration : MonoBehaviour
{
    GameMetaData newGame;

    // Start is called before the first frame update
    void Start()
    {
        /// create a new game
        newGame = CreateNewGame();
        /// Opens a random existing game (right now there is only one - the one just created), attempts to read value from it (which will all be defaults),
        /// adds a plant and saves the game, closes the game, opens it up again, reads the plant again
        Demonstration();

    }

    private void Demonstration()
    {
        

        OpenGameRandomExistingGame();

        DisplayCurrentPlants();
        AddPlantAndSaveGame();
        string gameID = SaveManager.Instance.CurrentlyOpenGame.gameID;
        CloseGame();
        OpenGame(gameID);
        DisplayCurrentPlants();

    }

    /// delete the game just created when stop playing
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

    private void AddPlantAndSaveGame()
    {
        if (!SaveManager.Instance.GameOpen)
        {
            Debug.LogError("Game is not open");
            return;
        }

        var go1 = new GameObject();
        var go2 = new GameObject();
        var plant = go1.AddComponent<Plant>();

        var player = go2.AddComponent<Player>();
        player.PlayerEnumValue = Player.PlayerEnum.Player2;

        go1.transform.position = new Vector3(1, 2, 3);
        plant.PlantOwner = player;
        plant.plantname = Plant.PlantName.Phess;


        SaveManager.Instance.GameData.plants.Add(plant);


        if (!SaveManager.Instance.SaveGame())
        {
            Debug.LogError("Error saving game");
            return;
        }

        Debug.Log("Saved game sucessfully");

    }

    private void DisplayCurrentPlants()
    {
        if (!SaveManager.Instance.GameOpen)
        {
            Debug.LogError("There is currently not a game open");
            return;
        }

        foreach (var plantData in SaveManager.Instance.GameData.plantsToAdd)
        {
            Debug.Log($"Plant: {((Plant.PlantName)plantData.plantNameType).ToString()}");
            Debug.Log($"Position: {(new Vector3(plantData.position[0],plantData.position[1],plantData.position[2])).ToString()}");
            Debug.Log($"Owner: {((Player.PlayerEnum)plantData.owner).ToString()}");
        }
         
   

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
    private void OpenGame(string gameID) // same as above but this is how to do it with a string ID
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

    private void DeleteGame(string gameID) // same as above but this is how to do it with a string ID
    { 
        Debug.Log($"Deleting game {gameID}");
        SaveManager.Instance.DeleteGame(gameID);
    }


}
