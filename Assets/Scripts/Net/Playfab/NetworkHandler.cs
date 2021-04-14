﻿using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab.GroupsModels;
using UnityEngine;


namespace NetSystem
{


    [RequireComponent(typeof(MatchMaker))]
    [RequireComponent(typeof(ServerGameDataHandler))]
    public class NetworkHandler : MonoBehaviour
    {
        [SerializeField] PlayerClient playerClient;
        NetworkGame currentNetworkGame = null;


        MatchMaker matchMaker;
        ServerGameDataHandler gameDataHandler;



        private void Awake()
        {
            matchMaker = GetComponent<MatchMaker>();
            matchMaker.Init(playerClient);

            gameDataHandler = GetComponent<ServerGameDataHandler>();
            gameDataHandler.Init(playerClient, currentNetworkGame);
        }

        private void UpdateGame(NetworkGame obj)
        {
            currentNetworkGame = obj;
            gameDataHandler.networkGame = obj;
        }

        public void GatherAllMemberGames()
        {
            var callbacks = new APIOperationCallbacks<List<NetworkGame>>(onSucess: OnGamesGatheredSucess, onfailure: OnGamesGatherFailure);
            StartCoroutine(matchMaker.GetAllMyGames(callbacks));
        }

        private void OnGamesGatheredSucess(List<NetworkGame> games)
        {
            // todo - list these to the player
            throw new NotImplementedException();
        }

        private void OnGamesGatherFailure(FailureReason reason)
        {
            throw new NotImplementedException();
        }

        public void LoadActiveMemberGame(NetworkGame game)
        {
            throw new NotImplementedException();
        }

        public void EnterNewGame()
        {
            var callbacks = new APIOperationCallbacks<List<PlayFab.GroupsModels.GroupWithRoles>>(onSucess: OnGotOpenGameGroupsSucess, onfailure: OnGetOpenGamefailure);
            StartCoroutine(matchMaker.GetOpenGameGroups(callbacks));
        }

        private void OnGotOpenGameGroupsSucess(List<PlayFab.GroupsModels.GroupWithRoles> groups)
        {

            var groupToJoin = SelectGroupToJoin(groups);
            Debug.Log($"Open game found {groupToJoin.Group.Id}");
            var callbacks = new APIOperationCallbacks<NetworkGame>(onSucess: OnJoinedGameSucess, onfailure: OnJoinedGameFailure);
            StartCoroutine(matchMaker.JoinOpenGameGroup(groupToJoin,callbacks));
        }

        private GroupWithRoles SelectGroupToJoin(List<GroupWithRoles> groups)
        {
            return groups[UnityEngine.Random.Range(0, groups.Count)]; // for now select a game at random
        }

        private void OnGetOpenGamefailure(FailureReason reason)
        {
            switch (reason)
            {
                case FailureReason.PlayFabError:
                    UnexpectedPlayfabError();
                    return;
                case FailureReason.TooManyActiveGames:
                    PlayerHasTooManyActiveGames();
                    return;
                case FailureReason.NoOpenGamesAvailable: // this is not a problem, just start a new game
                    Debug.Log("No open games, starting new game");
                    StartNewGame();
                    return;
                default:
                    UnknownError();
                    return;
            }
            
        }

        private void OnJoinedGameSucess(NetworkGame obj)
        {
            UpdateGame(obj);
            Debug.Log($"Joined game {currentNetworkGame.GroupName}");
        }

        

        private void OnJoinedGameFailure(FailureReason obj)
        {
            throw new NotImplementedException();
        }


        private void StartNewGame()
        {
            var callbacks = new APIOperationCallbacks<NetworkGame>(OnCreateGameSucess, OnCreateGameFailure);
            StartCoroutine(matchMaker.CreateGame(callbacks));
        }

        private void OnCreateGameSucess(NetworkGame obj)
        {
            UpdateGame(obj);
            Debug.Log($"Created game {currentNetworkGame.GroupName}");
        }

        private void OnCreateGameFailure(FailureReason error)
        {
            Debug.LogError(error);
            throw new NotImplementedException();
        }

        private void PlayerHasTooManyActiveGames()
        {
            Debug.Log("Too many active games");
            throw new NotImplementedException();
        }





        public void SendData()
        {
            var callbacks = new APIOperationCallbacks(onSucess: OnSendDataSucess, onfailure: OnSendDataFailure);
            StartCoroutine(gameDataHandler.SaveDataToServer("Please", callbacks));
        }

        private void OnSendDataSucess()
        {
            Debug.Log("Data sent");
        }

        private void OnSendDataFailure(FailureReason obj)
        {
            throw new NotImplementedException();
        }

        private void UnexpectedPlayfabError()
        {
            throw new NotImplementedException();
        }

        private void UnknownError()
        {
            throw new NotImplementedException();
        }
    }
   

   




}