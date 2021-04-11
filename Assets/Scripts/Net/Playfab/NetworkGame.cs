using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetSystem
{
    public class NetworkGame
    {
        public PlayFab.CloudScriptModels.EntityKey GroupEntityKey => new PlayFab.CloudScriptModels.EntityKey() { Id = group.Group.Id, Type = group.Group.Type };
        public string GroupName => group.GroupName;
        public bool GameOpenToJoin => metaData.Open;

        public List<PlayFab.CloudScriptModels.EntityKey> playerEntities => new List<PlayFab.CloudScriptModels.EntityKey>() { metaData.Player1, metaData.Player2 };

        readonly PlayFab.GroupsModels.GroupWithRoles group;
        readonly NetworkGame.NetworkGameMetadata metaData;

        public NetworkGame(PlayFab.GroupsModels.GroupWithRoles group, NetworkGame.NetworkGameMetadata gameMetaData)
        {
            this.group = group;
            this.metaData = gameMetaData;
        }


        public class NetworkGameMetadata
        {
            public bool Open;
            public PlayFab.CloudScriptModels.EntityKey Player1;
            public PlayFab.CloudScriptModels.EntityKey Player2;
            public string SharedDataID;
        }

    }
}