using System;
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
        readonly NetworkGameMetadata metaData;

        public RawData rawData;
        public UsableData usableData;

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
            public string gameStartedBy;
            public string turnBelongsTo;
            public string turnComplete;

            public string time;

            public string gardenData;
            public string playerData;            
        }

        public class UsableData
        {
            public bool gameBegun;
            public string gameStartedBy;
            public string turnBelongsTo;

            public bool turnComplete;

            public string time;

            public GardenDataPacket gardenData;
            public PlayerDataPacket playerData;

         
        }


        public bool DeserilaiseRawData(RawData rawData)
        {
            usableData = new UsableData();

            {
                bool? begun = ParseBool(rawData.gameBegun);
                if (!begun.HasValue)
                {
                    return false;
                }
                usableData.gameBegun = begun.Value;
            }

            {
                var turnComplete = ParseBool(rawData.turnComplete);
                if (!turnComplete.HasValue)
                {
                    return false;
                }
                usableData.gameBegun = turnComplete.Value;
            }

            usableData.gameStartedBy = rawData.gameStartedBy;
            usableData.turnBelongsTo = rawData.turnBelongsTo;

            

            try
            {
                usableData.gardenData = JsonUtility.FromJson<GardenDataPacket>(rawData.gardenData);
                usableData.playerData = JsonUtility.FromJson<PlayerDataPacket>(rawData.playerData);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private static bool? ParseBool(string data)
        {
            if(data == "true")
            {
                return true;
            }
            if(data == "false")
            {
                return false;
            }
            return null;
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