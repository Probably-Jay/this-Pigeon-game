using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Jay 11/03

public class SaveManager : Singleton<SaveManager>
{

    public new static SaveManager Instance { get => Singleton<SaveManager>.Instance; }

    private readonly SaveRegistryManager saveRegistryManager = new SaveRegistryManager();
    private readonly SaveGameManager saveGameManager = new SaveGameManager();

   // public GameMetaData 



    public override void Initialise()
    {
        base.InitSingleton();
    }

    private void Awake()
    {
        saveRegistryManager.CreateRegistryFile();
    }

    

    public bool CreateNewGame()
    {
        GameMetaData newGame = CreateNewGameFile();

        bool sucess = true;
        sucess &= saveRegistryManager.AddNewGame(newGame);
        sucess &= saveGameManager.CreateNewSaveFile(newGame.gameID);

        return sucess;
    }

    private GameMetaData CreateNewGameFile()
    {
        throw new NotImplementedException();
    }

    public bool DeleteGame(string gameID) => DeleteGame(saveRegistryManager.GetGame(gameID));

    public bool DeleteGame(GameMetaData game)
    {
        bool sucess = true;
        sucess &= saveGameManager.DeleteGame(game);
        sucess &= saveRegistryManager.RemoveGame(game.gameID);
        return sucess;

      
    }

    private void SaveGame()
    {
        // saveGameManager.StageSaveData()
        saveGameManager.OverwriteSaveFile();
        throw new NotImplementedException();
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
       
    }

   

  

}
