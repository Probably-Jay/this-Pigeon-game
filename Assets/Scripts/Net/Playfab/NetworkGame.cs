using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace NetSystem
{
    public class NetworkGame
    {
        public PlayFab.CloudScriptModels.EntityKey GroupEntityKey => new PlayFab.CloudScriptModels.EntityKey() { Id = group.Group.Id, Type = group.Group.Type };
        public string GroupName => group.GroupName;
        public bool GameOpenToJoin => metaData.Open;

        public bool NewGameJustCreated { get; private set; }

        public List<PlayFab.CloudScriptModels.EntityKey> playerEntities => new List<PlayFab.CloudScriptModels.EntityKey>() { metaData.Player1, metaData.Player2 };


        readonly PlayFab.GroupsModels.GroupWithRoles group;
        readonly NetworkGame.NetworkGameMetadata metaData;

        public RawData rawData;

        public NetworkGame(PlayFab.GroupsModels.GroupWithRoles group, NetworkGame.NetworkGameMetadata gameMetaData, bool newGame = false)
        {
            this.group = group;
            this.metaData = gameMetaData;
            NewGameJustCreated = newGame;
        }


        public class NetworkGameMetadata
        {
            public bool Open;
            public PlayFab.CloudScriptModels.EntityKey Player1;
            public PlayFab.CloudScriptModels.EntityKey Player2;
          //  public string SharedDataID;
        }

        public class GameSharedData 
        {
            public string Value;
            public string LastUpdated;

        }

        public class RawData
        {
            public string gameBegun;
            public string turnBelongsTo;
            public string turnComplete;

            public string time;

            public string gardenA;
            public string gardenB;            
            
            public string prevGardenA;
            public string prevGardenB;

        }

        /// <summary>
        /// Takes a collection of games and sorts them into active ongoing games and open unstarted games
        /// </summary>
        /// <param name="games"></param>
        /// <param name="openGames"></param>
        /// <param name="activeGames"></param>
        public static void SeperateOpenAndClosedGames(ReadOnlyCollection<NetworkGame> games, out List<NetworkGame> openGames, out List<NetworkGame> activeGames)
        {
            openGames = new List<NetworkGame>();
            activeGames = new List<NetworkGame>();
            foreach (var game in games)
            {
                if (game.GameOpenToJoin)
                {
                    openGames.Add(game);
                }
                else
                {
                    activeGames.Add(game);
                }
            }
        }

    }
}