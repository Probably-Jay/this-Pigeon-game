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

        public bool _useDebugAcountLogin = false;

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

        public void AnonymousLoginPlayer(APIOperationCallbacks callbacks)
        {
            PlayerClient.AnonymousLogin(callbacks);
        }

        public void AnonymousLoginDebugPlayer(APIOperationCallbacks callbacks)
        {
            PlayerClient.AnonymousLogin(callbacks,true);
        }

        public void LogoutPlayer()
        {
            NetGame.ExitGame();
            PlayerClient.Logout();
        }

        private void UpdateActiveGame(NetworkGame obj)
        {
            NetGame.SetActiveNetorkGame(obj);
        }

        public void GatherAllMemberGames(APIOperationCallbacks<ReadOnlyCollection<NetworkGame>> callbacks)
        {
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

        public void EnterNewGame(APIOperationCallbacks<NetworkGame> parentCallbacks)
        {
            var callbacks = new APIOperationCallbacks<List<PlayFab.GroupsModels.GroupWithRoles>>(
                onSucess: (groups) => { OnGotOpenGameGroupsSucess(groups, parentCallbacks); },
                onfailure: (reason) => { OnGetOpenGamefailure(reason, parentCallbacks); });

            StartCoroutine(RemoteOpenGamesList.GetOpenGameGroups(callbacks));
        }

        private void OnGotOpenGameGroupsSucess(List<PlayFab.GroupsModels.GroupWithRoles> groups, APIOperationCallbacks<NetworkGame> parentCallbacks )
        {

            var groupToJoin = SelectGroupToJoin(groups);
            Debug.Log($"Open game found {groupToJoin.Group.Id}");

            var callbacks = new APIOperationCallbacks<NetworkGame>(
                onSucess: (game) => { OnJoinedGameSucess(game, parentCallbacks); },
                onfailure: (reason) => { OnJoinedGameFailure(reason, parentCallbacks); });

            StartCoroutine(NetGame.JoinOpenGameGroup(groupToJoin,callbacks));
        }

        private GroupWithRoles SelectGroupToJoin(List<GroupWithRoles> groups)
        {
            return groups[UnityEngine.Random.Range(0, groups.Count)]; // for now select a game at random
        }

        private void OnGetOpenGamefailure(FailureReason reason, APIOperationCallbacks<NetworkGame> parentCallbacks)
        {
            switch (reason)
            {
                case FailureReason.PlayFabError:
                    UnexpectedPlayfabError(reason, parentCallbacks);
                    return;
                case FailureReason.TooManyActiveGames:
                    PlayerHasTooManyActiveGames(reason, parentCallbacks);
                    return;
                case FailureReason.NoOpenGamesAvailable: // this is not a problem, just start a new game
                    Debug.Log("No open games, starting new game");
                    StartNewGame(parentCallbacks);
                    return;
                default:
                    UnknownError(reason,parentCallbacks);
                    return;
            }
            
        }

        private void OnJoinedGameSucess(NetworkGame game, APIOperationCallbacks<NetworkGame> parentCallbacks)
        {
            UpdateActiveGame(game);
            Debug.Log($"Joined game {NetGame.CurrentNetworkGame.GroupName}");
            parentCallbacks.OnSucess(game);
        }

        

        private void OnJoinedGameFailure(FailureReason reason, APIOperationCallbacks<NetworkGame> parentCallbacks)
        {
            Debug.LogError(reason);
            parentCallbacks.OnFailure(reason);
        }


        private void StartNewGame(APIOperationCallbacks<NetworkGame> parentCallbacks)
        {
            var callbacks = new APIOperationCallbacks<NetworkGame>(
                onSucess: (game) => { OnCreateGameSucess(game, parentCallbacks); },
                onfailure: (reason) => { OnCreateGameFailure(reason, parentCallbacks); });
            StartCoroutine(matchMaker.CreateGame(callbacks));
        }

        private void OnCreateGameSucess(NetworkGame game, APIOperationCallbacks<NetworkGame> parentCallbacks)
        {
            UpdateActiveGame(game);
            Debug.Log($"Created game {NetGame.CurrentNetworkGame.GroupName}");
            parentCallbacks.OnSucess(game);
        }

        private void OnCreateGameFailure(FailureReason error, APIOperationCallbacks<NetworkGame> parentCallbacks)
        {
            Debug.LogError(error);
            parentCallbacks.OnFailure(error);
        }

        private void PlayerHasTooManyActiveGames(FailureReason reason, APIOperationCallbacks<NetworkGame> parentCallbacks)
        {
            Debug.Log("Too many active games");
            parentCallbacks.OnFailure(reason);
        }


        public void ReceiveData()
        {
            var callbacks = new APIOperationCallbacks<NetworkGame.RawData>(onSucess: OnReceiveDataSucess, onfailure: OnReceiveDataFailure);
            StartCoroutine(gameDataHandler.GetDataFromTheServer(callbacks));
        }

        private void OnReceiveDataSucess(NetworkGame.RawData data)
        {
            Debug.Log($"Data received: {nameof(data.gardenA)}: \"{data.gardenA}\", {nameof(data.gardenB)}: \"{data.gardenB}\"");
        }

        private void OnReceiveDataFailure(FailureReason obj)
        {
            throw new NotImplementedException();
        }

        public void SendData(NetworkGame.RawData data)
        {
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

        private void UnexpectedPlayfabError<T>(FailureReason reason, APIOperationCallbacks<T> parentCallbacks)
        {
            Debug.LogError("There was an unexpected playfab error");
            parentCallbacks.OnFailure(reason);
        }

        private void UnknownError<T>(FailureReason reason, APIOperationCallbacks<T> parentCallbacks)
        {
            Debug.LogError($"There was an unexpected error: {reason}");
            parentCallbacks.OnFailure(reason);
        }
    }
   

   




}