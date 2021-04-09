using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NetSystem
{


    [RequireComponent(typeof(MatchMaker))]
    public class NetworkHandler : MonoBehaviour
    {
        [SerializeField] PlayerClient playerClient;
        MatchMaker matchMaker;

        private void Awake()
        {
            matchMaker = GetComponent<MatchMaker>();
            matchMaker.Init(playerClient);
        }

        public void MatchMake()
        {
            var callbacks = new APIOperationCallbacks<List<NetworkGame>>(OnGamesGatheredSucess, OnGamesGatherFailure);
            StartCoroutine(matchMaker.GetAllMyGames(callbacks));

          //  while(res)

        }

        private void OnGamesGatheredSucess(List<NetworkGame> games)
        {
            Debug.Log("Overall sucess");
            foreach (var game in games)
            {
                Debug.Log(game.GroupName);
                Debug.Log($"Open: {game.MetaData.}");

            }
        }

        private void OnGamesGatherFailure()
        {
            throw new NotImplementedException();
        }

    }
    public class NetworkGame
    {
        public PlayFab.CloudScriptModels.EntityKey GroupEntityKey => new PlayFab.CloudScriptModels.EntityKey() { Id = group.Group.Id,Type = group.Group.Type };
        public string GroupName => group.GroupName;
        public NetworkGameMetadata MetaData  => metaData;

        readonly PlayFab.GroupsModels.GroupWithRoles group;
        readonly NetworkGame.NetworkGameMetadata metaData;

        public NetworkGame(PlayFab.GroupsModels.GroupWithRoles group, NetworkGame.NetworkGameMetadata gameMetaData)
        {
            this.group = group;
            this.metaData = gameMetaData;
        }


        public class NetworkGameMetadata
        {
            public readonly bool Open;
            public readonly PlayFab.CloudScriptModels.EntityKey Player1;
            public readonly PlayFab.CloudScriptModels.EntityKey Player2;
        }

    }

   




}