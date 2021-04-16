using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PlayFab.GroupsModels;
using UnityEngine;


namespace NetSystem
{


    [RequireComponent(typeof(ActiveGame))]
    [RequireComponent(typeof(MatchMaker))]
    [RequireComponent(typeof(ServerGameDataHandler))]
    [RequireComponent(typeof(PlayerClient))]
    [RequireComponent(typeof(MemberGamesList))]
    [RequireComponent(typeof(OpenGamesList))]
    public class NetworkHandler : Singleton<NetworkHandler>
    {
        new public static NetworkHandler Instance { get => Singleton<NetworkHandler>.Instance; }
        public PlayFab.CloudScriptModels.EntityKey ClientEntity => PlayerClient.ClientEntityKey;

        public bool LoggedIn => PlayerClient.IsLoggedIn;
        public bool InGame => LoggedIn && NetGame.IsInGame;


        public ActiveGame NetGame { get; private set; } = null;
        public PlayerClient PlayerClient { get; private set; } 


        MatchMaker matchMaker;
        ServerGameDataHandler gameDataHandler;

        public MemberGamesList RemoteMemberGamesList { get; private set; }
        public OpenGamesList RemoteOpenGamesList { get; private set; }

        public override void Initialise()
        {
            base.InitSingleton();

            RemoteMemberGamesList = GetComponent<MemberGamesList>();
            RemoteOpenGamesList = GetComponent<OpenGamesList>();

            NetGame = GetComponent<ActiveGame>();

            matchMaker = GetComponent<MatchMaker>();
          

            gameDataHandler = GetComponent<ServerGameDataHandler>();
          

            PlayerClient = GetComponent<PlayerClient>();

           // AnonymousLogin();
        }

        public void AnonymousLoginPlayer() => PlayerClient.AnonymousLogin();

        public void AnonymousLoginDebugPlayer() => PlayerClient.DebugWindowsAnonymousLogin();

        private void UpdateGame(NetworkGame obj)
        {
            NetGame.SetActiveNetorkGame(obj);
        }

        public void GatherAllMemberGames(Action<ReadOnlyCollection<NetworkGame>> onGamesGatheredSucess, Action<FailureReason> onGamesGatherFailure)
        {
            var callbacks = new APIOperationCallbacks<ReadOnlyCollection<NetworkGame>>(onSucess: onGamesGatheredSucess, onfailure: onGamesGatherFailure);
            StartCoroutine(RemoteMemberGamesList.GetAllMyGames(callbacks));
        }        
        
        public void GatherAllOpenGames(Action<List<GroupWithRoles>> onGamesGatheredSucess, Action<FailureReason> onGamesGatherFailure)
        {
            var callbacks = new APIOperationCallbacks<List<GroupWithRoles>>(onSucess: onGamesGatheredSucess, onfailure: onGamesGatherFailure);
            StartCoroutine(RemoteOpenGamesList.GetOpenGameGroups(callbacks)); ;
        }

        private void OnGamesGatheredSucess(ReadOnlyCollection<NetworkGame> games)
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

        public void ResumeMemberGame(NetworkGame game)
        {
            var callbacks = new APIOperationCallbacks<NetworkGame>(onSucess: OnResumeGameSucess, onfailure: OnResumeGamefailure);
            StartCoroutine(NetGame.ResumeMemberGame(game, callbacks));
        }

        private void OnResumeGameSucess(NetworkGame obj)
        {
            NetGame.SetActiveNetorkGame(obj);
        }

        private void OnResumeGamefailure(FailureReason obj)
        {
            throw new NotImplementedException();
        }

        public void EnterNewGame()
        {
            var callbacks = new APIOperationCallbacks<List<PlayFab.GroupsModels.GroupWithRoles>>(onSucess: OnGotOpenGameGroupsSucess, onfailure: OnGetOpenGamefailure);
            StartCoroutine(RemoteOpenGamesList.GetOpenGameGroups(callbacks));
        }

        private void OnGotOpenGameGroupsSucess(List<PlayFab.GroupsModels.GroupWithRoles> groups)
        {

            var groupToJoin = SelectGroupToJoin(groups);
            Debug.Log($"Open game found {groupToJoin.Group.Id}");
            var callbacks = new APIOperationCallbacks<NetworkGame>(onSucess: OnJoinedGameSucess, onfailure: OnJoinedGameFailure);
            StartCoroutine(NetGame.JoinOpenGameGroup(groupToJoin,callbacks));
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
            Debug.Log($"Joined game {NetGame.CurrentNetworkGame.GroupName}");
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
            Debug.Log($"Created game {NetGame.CurrentNetworkGame.GroupName}");
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


        public void ReceiveData()
        {
            var callbacks = new APIOperationCallbacks<string>(onSucess: OnReceiveDataSucess, onfailure: OnReceiveDataFailure);
            StartCoroutine(gameDataHandler.GetDataFromTheServer(callbacks));
        }

        private void OnReceiveDataSucess(string data)
        {
            throw new NotImplementedException();
        }

        private void OnReceiveDataFailure(FailureReason obj)
        {
            throw new NotImplementedException();
        }

        public void SendData()
        {
            var data = "Data to be sent";
            var callbacks = new APIOperationCallbacks(onSucess: OnSendDataSucess, onfailure: OnSendDataFailure);
            StartCoroutine(gameDataHandler.SaveDataToServer(data, callbacks));
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