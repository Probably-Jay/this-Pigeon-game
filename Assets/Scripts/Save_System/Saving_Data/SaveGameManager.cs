using NetSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// jay 11/03
namespace SaveSystemInternal
{
    /// <summary>
    /// Class which handles the saveing ig game files. Controlls <see cref="SaveDataSerialiser"/>.
    /// Controlled by <see cref="SaveManager"/>. See also <see cref="SaveRegistryManager"/>
    /// </summary>
    public class SaveGameManager
    {
        /// <summary>
        /// The <see cref="GameMetaData"/> structure of the currently open game. <c>null</c> if there is no game open
        /// </summary>
        public GameMetaData OpenGameMetaData { get; private set; } = null;
        /// <summary>
        /// The <see cref="SaveGameData"/> structure of the currently open game, holding the data from the file or and staged saves (see <see cref="StageSaveData(SaveGameData)"/>). 
        /// <c>null</c> if there is no game open
        /// </summary>
        public SaveGameData OpenGameData { get; private set; } = null;
        /// <summary>
        /// If there is currently a game open
        /// </summary>
        public bool GameOpen => OpenGameData != null;

        /// <summary>
        /// Creates a new empty save file using the gameID. If a file already exists there this has no effect. This will not open the game
        /// </summary>
        /// <param name="newGame">Newly generated unique game ID</param>
        /// <returns>If a new file was created</returns>
        public bool CreateNewSaveFile(GameMetaData newGame) => SaveDataSerialiser.CreateNewSaveFile(newGame.gameID);

        /// <summary>
        /// Load in the contents of the save file to <see cref="OpenGameData"/>. The paramater <paramref name="game"/> must be gained from <see cref="SaveRegistryManager.GetAllStoredGames"/>. ("open"/"close" does not imply any kind of resource lock)
        /// </summary>
        /// <param name="game">Data class on a save file, gained from <see cref="SaveRegistryManager.GetAllStoredGames"/></param>
        /// <returns>If the save was opened</returns>
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
        /// This is the primary function in saving data. This will set the <see cref="OpenGameData"/> to the value of <paramref name="data"/> (if <paramref name="data"/> is not <c>null</c>).
        /// *Warrning* This will not actually save the game data. You must call then <see cref="OverwriteSaveFile"/>
        /// </summary>
        public bool StageSaveData(SaveGameData data)
        {
            if (!GameOpen || data == null) return false;

            OpenGameData = data;
            return true;

        }

        /// <summary>
        /// Overwrites the open save file with the value of <see cref="OpenGameData"/> and then nullifies the referance to this file
        /// </summary>
        /// <returns>If the operation was sucessful</returns>
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
        /// <returns>If the operation was sucessful</returns>
        public bool OverwriteSaveFile()
        {
            if (!GameOpen) return false;

            return SaveDataSerialiser.SaveGame(OpenGameMetaData.gameID, OpenGameData);

        }

        public bool OverwriteSaveFile(NetworkGame.RawData data)
        {
            if (!GameOpen) return false;

            return SaveDataSerialiser.SaveGame(OpenGameMetaData.gameID, data);
        }

        /// <summary>
        /// Nullifies the referance to the open file ("open"/"close" does not imply a resources lock)
        /// </summary>
        public void CloseGame()
        {
            OpenGameData = null;
            OpenGameMetaData = null;
        }

        /// <summary>
        /// Delete a provided game if it exists. If the game provided is currenly open then it is closed. Will not fail.
        /// </summary>
        /// <returns>If a game previously existed (the deletion operation itself never fails)</returns>
        public bool DeleteGame(GameMetaData game)
        {
            if (GameOpen && OpenGameMetaData.gameID == game.gameID) CloseGame();

            return SaveDataSerialiser.DeleteFile(game.gameID);
        }

       

        /// <summary>
        /// Verify if a <see cref="GameMetaData"/> from the registry file corresponds to a game file that exists
        /// </summary>
        /// <param name="game">The game in question</param>
        /// <returns>If there is a file on disk with the same ID as <paramref name="game"/></returns>
        public static bool GameExists(GameMetaData game) => SaveDataSerialiser.GameExists(game.gameID);

    }
}