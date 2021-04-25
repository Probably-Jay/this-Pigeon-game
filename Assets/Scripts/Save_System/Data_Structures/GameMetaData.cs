using System;
using UnityEngine;

//Jay 11/03

namespace SaveSystemInternal
{
    /// <summary>
    /// A Datastructure that represents a game file, a list of these is stored on the registry file
    /// </summary>
    [Serializable]
    public class GameMetaData
    {
        /// <summary>
        /// The unique ID of a game. Made up of <see cref="SystemInfo.deviceUniqueIdentifier"/> and the number of ticks given by <see cref="DateTime.Now"/>.
        /// The filepath of a game is given by passing the value of <see cref="gameID"/> to <see cref="SaveDataSerialiser.GetFilePath(string)"/>
        /// </summary>
        public string gameID;
        //public string gameName;


        public GameMetaData(NetSystem.NetworkGame netGame)
        {
            gameID = netGame.GroupEntityKey.Id;
        }

    }
}