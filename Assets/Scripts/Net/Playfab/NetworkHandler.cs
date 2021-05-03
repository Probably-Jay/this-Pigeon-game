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
            ClearMemberGamesCache();

        }

        public void ClearMemberGamesCache() => RemoteMemberGamesList.ClearCache();

        private void UpdateActiveGame(NetworkGame obj)
        {
            NetGame.SetActiveNetorkGame(obj);
        }


        /// <summary>
        /// Gathers all games this client is a member of, including unstarted open games. This data is cached.
        /// <para/> Upon completion will invoke one of the following callbacks :
        /// <para><see cref="APIOperationCallbacks{List{PlayFab.GroupsModels.GroupWithRoles}}.OnSucess"/>: The cache was hit, or the open games were sucessfully gathered along with their metadata and cached</para>
        /// <para><see cref="APIOperationCallbacks{List{PlayFab.GroupsModels.GroupWithRoles}}.OnFailure"/>: The call failed due to a networking error, or for one of the following reasons (retuned in callback): 
        ///     <list type="bullet">
        ///         <item>
        ///         <term><see cref="FailureReason.PlayerIsMemberOfNoGames"/></term>
        ///         <description>The player is not a member of any games. This is returned as an explicit error case to be handled</description>
        ///         </item> 
        ///     </list>
        /// </para>
        /// </summary>
        /// <param name="resultsCallback">Callbakcs for the sucess or failure of this action</param>
        public void GatherAllMemberGames(APIOperationCallbacks<ReadOnlyCollection<NetworkGame>> callbacks)
        {
            StartCoroutine(RemoteMemberGamesList.GetAllMyGames(callbacks));
        }        
        
        //public void GatherAllOpenGames(Action<List<GroupWithRoles>> onGamesGatheredSucess, Action<FailureReason> onGamesGatherFailure)
        //{
        //    var callbacks = new APIOperationCallbacks<List<GroupWithRoles>>(onSucess: onGamesGatheredSucess, onfailure: onGamesGatherFailure);
        //    StartCoroutine(RemoteOpenGamesList.GetOpenGameGroups(callbacks)); ;
        //}

        //private void OnGamesGatheredSucess(ReadOnlyCollection<NetworkGame> games)
        //{
        //    // todo - list these to the player
        //    throw new NotImplementedException();
        //}

        //private void OnGamesGatherFailure(FailureReason reason)
        //{
        //    throw new NotImplementedException();
        //}

        //public void LoadActiveMemberGame(NetworkGame game)
        //{
        //    throw new NotImplementedException();
        //}

        public void ResumeMemberGame(NetworkGame game, APIOperationCallbacks<NetworkGame> parentCallbacks)
        {
            
            var callbacks = new APIOperationCallbacks<NetworkGame>(
                onSucess: (joinedGame) => 
                {
                    OnResumeGameSucess(joinedGame);
                    parentCallbacks.OnSucess(joinedGame);
                }, 
                onfailure: parentCallbacks.OnFailure
            );

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


        /// <summary>
        /// Enters new game. Will join an open game from the server if one is available, or will create a new game and publish it to the server.
        /// <para/> Upon completion will invoke one of the following callbacks :
        /// <para><see cref="APIOperationCallbacks{List{PlayFab.GroupsModels.GroupWithRoles}}.OnSucess"/>: A game was sucesfully joined. Will return the <see cref="NetworkGame"/> that was joined</para>
        /// <para><see cref="APIOperationCallbacks{List{PlayFab.GroupsModels.GroupWithRoles}}.OnFailure"/>: The call failed due to a networking error, or for one of the following reasons (retuned in callback): 
        ///     <list type="bullet">
        ///         <item>
        ///         <term><see cref="FailureReason.TooManyActiveGames"/></term>
        ///         <description>The user is a member of too many games and is not allowed to enter a new one</description>
        ///         </item> 
        ///         <item>        
        ///         <term><see cref="FailureReason.AboveOpenGamesLimit"/></term>
        ///         <description>There are no open games on the server to join, but the user cannot create a new game as they own too many open games already</description>
        ///         </item>
        ///     </list>
        /// </para>
        /// </summary>
        /// <param name="resultsCallback">Callbakcs for the sucess or failure of this action</param>
        /// <remarks><seealso cref="SelectGroupToJoin"/> for the method a open game is selected</remarks>
        public void EnterNewGame(APIOperationCallbacks<NetworkGame> parentCallbacks)
        {
            var callbacks = new APIOperationCallbacks<List<PlayFab.GroupsModels.GroupWithRoles>>(
                onSucess: (groups) => { OnNewGameGotOpenGameGroupsSucess(groups, parentCallbacks); },
                onfailure: (reason) => { OnNewGameGetOpenGamefailure(reason, parentCallbacks); });

            StartCoroutine(RemoteOpenGamesList.GetOpenGameGroups(callbacks));
        }


        //public void GetOpenGameGroups(APIOperationCallbacks<List<NetworkGame>> parentCallbacks)
        //{
        //    var callbacks = new APIOperationCallbacks<List<PlayFab.GroupsModels.GroupWithRoles>>(
        //       onSucess: (groups) => { OnGotOpenGameGroupsSucess(groups, parentCallbacks); },
        //       onfailure: (reason) => { OnGetOpenGamefailure(reason, parentCallbacks); });

        //    StartCoroutine(RemoteOpenGamesList.GetOpenGameGroups(callbacks));
        //}

        //private void OnGotOpenGameGroupsSucess(List<GroupWithRoles> groups, APIOperationCallbacks<List<NetworkGame>> parentCallbacks)
        //{
            
        //}

        //private void OnGetOpenGamefailure(FailureReason reason, APIOperationCallbacks<List<NetworkGame>> parentCallbacks)
        //{
        //    throw new NotImplementedException();
        //}




        /// <summary>
        /// Calls <see cref="ActiveGame.JoinOpenGameGroup"/>
        /// <para>Callbacks to: <see cref="OnJoinedGameSucess"/> or <see cref="OnJoinedGameFailure"/></para>
        /// </summary>
        private void OnNewGameGotOpenGameGroupsSucess(List<PlayFab.GroupsModels.GroupWithRoles> groups, APIOperationCallbacks<NetworkGame> parentCallbacks )
        {

            var groupToJoin = SelectGroupToJoin(groups); // select game from list at random, could change this to oldest game in list

            Debug.Log($"Open game found {groupToJoin.Group.Id}");

            var callbacks = new APIOperationCallbacks<NetworkGame>(
                onSucess: (game) => { OnJoinedGameSucess(game, parentCallbacks); },
                onfailure: (reason) => { OnJoinedGameFailure(reason, parentCallbacks); });

            StartCoroutine(NetGame.JoinOpenGameGroup(groupToJoin,callbacks));
        }

        /// <summary>
        /// Returns a selected game from <paramref name="groups"/>
        /// </summary>
        private GroupWithRoles SelectGroupToJoin(List<GroupWithRoles> groups)
        {
            return groups[UnityEngine.Random.Range(0, groups.Count)]; // for now select a game at random
        }

        /// <summary>
        /// Calls <see cref="StartNewGame"/> if there were no open games, else terminates terminates callback chain with <see cref="APIOperationCallbacks{NetworkGame}.OnFailure"/>
        /// </summary>
        private void OnNewGameGetOpenGamefailure(FailureReason reason, APIOperationCallbacks<NetworkGame> parentCallbacks)
        {
            if (reason == FailureReason.NoOpenGamesAvailable) // this is not a problem, just start a new game
            { 
                Debug.Log("No open games, starting new game");
                StartNewGame(parentCallbacks);
                return; 
            }
              
            Debug.LogError($"Error getting open games: {reason}");
            parentCallbacks.OnFailure(reason);
        }

        /// <summary>
        /// Updates <see cref="NetGame"/>, terminates callback chain with <see cref="APIOperationCallbacks{NetworkGame}.OnSucess"/>
        /// </summary>
        private void OnJoinedGameSucess(NetworkGame game, APIOperationCallbacks<NetworkGame> parentCallbacks)
        {
            UpdateActiveGame(game);
            Debug.Log($"Joined game {NetGame.CurrentNetworkGame.GroupName}");
            parentCallbacks.OnSucess(game);
        }


        /// <summary>
        /// Terminates callback chain with <see cref="APIOperationCallbacks{NetworkGame}.OnFailure"/>
        /// </summary>
        private void OnJoinedGameFailure(FailureReason reason, APIOperationCallbacks<NetworkGame> parentCallbacks)
        {
            Debug.LogError(reason);
            parentCallbacks.OnFailure(reason);
        }

        /// <summary>
        /// Calls <see cref="MatchMaker.CreateGame"/>
        /// <para>Callbacks to: <see cref="OnCreateGameSucess"/> or <see cref="OnCreateGameFailure"/></para>
        /// </summary>
        private void StartNewGame(APIOperationCallbacks<NetworkGame> parentCallbacks)
        {
            var callbacks = new APIOperationCallbacks<NetworkGame>(
                onSucess: (game) => { OnCreateGameSucess(game, parentCallbacks); },
                onfailure: (reason) => { OnCreateGameFailure(reason, parentCallbacks); });

            StartCoroutine(matchMaker.CreateGame(callbacks));
        }

        /// <summary>
        /// Updates <see cref="NetGame"/>, terminates callback chain with <see cref="APIOperationCallbacks{NetworkGame}.OnSucess"/>
        /// </summary>
        private void OnCreateGameSucess(NetworkGame game, APIOperationCallbacks<NetworkGame> parentCallbacks)
        {
            UpdateActiveGame(game);
            Debug.Log($"Created game {NetGame.CurrentNetworkGame.GroupName}");
            parentCallbacks.OnSucess(game);
           
        }

        /// <summary>
        /// Terminates callback chain with <see cref="APIOperationCallbacks{NetworkGame}.OnFailure"/>
        /// </summary>
        private void OnCreateGameFailure(FailureReason error, APIOperationCallbacks<NetworkGame> parentCallbacks)
        {
            Debug.LogError(error);
            parentCallbacks.OnFailure(error);
        }


        /// <summary>
        /// Gets the data of the provided <paramref name="game"/>, or of the current game if provided <paramref name="game"/> is null
        /// <para/> Upon completion will invoke one of the following callbacks :
        /// <para><see cref="APIOperationCallbacks{List{PlayFab.GroupsModels.GroupWithRoles}}.OnSucess"/>: The game data was sucessfully obtained</para>
        /// <para><see cref="APIOperationCallbacks{List{PlayFab.GroupsModels.GroupWithRoles}}.OnFailure"/>: The call failed due to a networking error (returned in callback)</para>
        /// </summary>
        /// <param name="resultsCallback">Callbakcs for the sucess or failure of this action</param>
        public void ReceiveData(APIOperationCallbacks<NetworkGame.UsableData> parentCallbacks, NetworkGame game = null)
        {
            var callbacks = new APIOperationCallbacks<NetworkGame.RawData>(
                onSucess: (rawData) => OnReceiveDataSucess(parentCallbacks, game, rawData),
                onfailure: (e) => OnReceiveDataFailure(parentCallbacks, e)
                );

            StartCoroutine(gameDataHandler.GetDataFromTheServer(callbacks, game));
        }

        private void OnReceiveDataSucess(APIOperationCallbacks<NetworkGame.UsableData> parentCallbacks, NetworkGame game, NetworkGame.RawData data)
        {
            if(game == null)
            {
                game = NetGame.CurrentNetworkGame;
            }

            game.rawData = data;

            var sucess = game.DeserilaiseRawData(data);

            if (!sucess)
            {
                OnReceiveDataFailure(parentCallbacks, FailureReason.InternalError);
                return;
            }

            parentCallbacks.OnSucess(game.UsableGameData);
        }

        private void OnReceiveDataFailure(APIOperationCallbacks<NetworkGame.UsableData> parentCallbacks, FailureReason e)
        {
            parentCallbacks.OnFailure(e);
        }

        //private void OnReceiveDataSucess(NetworkGame.RawData data)
        //{
        //    Debug.Log($"Data received: {nameof(data.gardenA)}: \"{data.gardenA}\", {nameof(data.gardenB)}: \"{data.gardenB}\"");
        //}

        //private void OnReceiveDataFailure(FailureReason obj)
        //{
        //    throw new NotImplementedException();
        //}

        /// <summary>
        /// Sends the data provided to the server, overwiting the current save data
        /// <para/> Upon completion will invoke one of the following callbacks :
        /// <para><see cref="APIOperationCallbacks{List{PlayFab.GroupsModels.GroupWithRoles}}.OnSucess"/>: The game data was sucessfully sent</para>
        /// <para><see cref="APIOperationCallbacks{List{PlayFab.GroupsModels.GroupWithRoles}}.OnFailure"/>: The call failed due to a networking error (returned in callback)</para>
        /// </summary>
        /// <param name="resultsCallback">Callbakcs for the sucess or failure of this action</param>
        public void SendData(APIOperationCallbacks callbacks, NetworkGame.RawData data)
        {
            StartCoroutine(gameDataHandler.SaveDataToServer(data, callbacks));
        }

        //private void OnSendDataSucess()
        //{
        //    Debug.Log("Data sent");
        //}

        //private void OnSendDataFailure(FailureReason obj)
        //{
        //    throw new NotImplementedException();
        //}

        public bool ContinueToGame(SceneChangeController.Scenes game)
        {
            if(PlayerClient == null || !PlayerClient.IsLoggedIn)
            {
                return false;
            }

            if(NetGame == null || !NetGame.IsInGame)
            {
                return true;
            }

            SceneChangeController.Instance.ChangeScene(game);
            return true;
        }

        //private void UnexpectedPlayfabError<T>(FailureReason reason, APIOperationCallbacks<T> parentCallbacks)
        //{
        //    Debug.LogError("There was an unexpected playfab error");
        //    parentCallbacks.OnFailure(reason);
        //}

        //private void UnknownError<T>(FailureReason reason, APIOperationCallbacks<T> parentCallbacks)
        //{
        //    Debug.LogError($"There was an unexpected error: {reason}");
        //    parentCallbacks.OnFailure(reason);
        //}



    }
   

   




}