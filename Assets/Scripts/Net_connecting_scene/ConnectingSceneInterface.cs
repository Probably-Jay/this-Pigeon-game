﻿using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using NetSystem;
using SaveSystem;
using System;

// jay

namespace SceneInterface
{

    public class ConnectingSceneInterface : MonoBehaviour
    {
        [SerializeField] ActiveGamesListPopulator gamesList; 

        [SerializeField] TMP_Text messagText;
        [SerializeField] Image loadingImage;
        [SerializeField] Button backButton;
        [SerializeField] Button newGameButton;
        [SerializeField] GameObject enterGamePannel;
        [SerializeField] TMP_Text enterGameText;
        [SerializeField] Image enterGameLoadingImage;

       // Coroutine logginInCoroutine;

        const float rotationSpeed = 50;

        Coroutine refreshCoroutine;

        private void Start()
        {
            StartCoroutine(ServerOperations());
        }


        void OnDestroy()
        {
            StopAllCoroutines();
        }

        IEnumerator ServerOperations()
        {
            DisableNewGameButton();

            DisableBackButton();

            // log in
            {
                CallResponse loginResponse = Login();
                yield return new WaitUntil(() => loginResponse.status.Complete);

                if (loginResponse.status.Error)
                {
                    yield break;
                }
            }

            yield return StartCoroutine(RefreshMemberGamesList());


            EnableBackButton();

            EnableNewGameButton();

            refreshCoroutine = StartCoroutine(PeriodicRefresh());

        }

        private IEnumerator PeriodicRefresh()
        {
            while (true)
            {
                yield return new WaitForSeconds(15);
                yield return StartCoroutine(RefreshMemberGamesList());
            }
        }

        private IEnumerator RefreshMemberGamesList()
        {
            NetworkHandler.Instance.ClearMemberGamesCache();

            // get all member games
            ReadOnlyCollection<NetworkGame> memberGames = null;
            {
                CallResponse<ReadOnlyCollection<NetworkGame>> getGamesResponse = GetGames();
                yield return new WaitUntil(() => getGamesResponse.status.Complete);

                if (getGamesResponse.status.Error && getGamesResponse.status.ErrorData != FailureReason.PlayerIsMemberOfNoGames)
                {
                    yield break;
                }
                memberGames = getGamesResponse.returnData;
            }

            if (memberGames == null)
            {
                GamesGatheredFailure(FailureReason.InternalError);
                yield break;
            }

            // get the games data
            {
                var rawDataResponses = new List<CallResponse<NetworkGame.UsableData>>();
                foreach (var game in memberGames)
                {
                    CallResponse<NetworkGame.UsableData> getGameDataResponse = GetGameDataCall(game); // this call is async, we can bunch a load of them up
                    rawDataResponses.Add(getGameDataResponse);
                }

                for (int i = 0; i < rawDataResponses.Count; i++)
                {
                    var call = rawDataResponses[i];
                    var game = memberGames[i];

                    yield return new WaitUntil(() => call.status.Complete); // wait for them to complete

                    if (call.status.Error)
                    {
                        Debug.LogError(call.status.ErrorData);
                        yield break;
                    }

                    NetworkGame.UsableData data = call.returnData;
                }
            }

            // update the list of local saves of these games
            {
                SaveManager.Instance.UpdateSavedGameListAndRegistyFile(memberGames);
            }

            gamesList.Populate(memberGames);
        }

        private CallResponse Login()
        {
            var response = new CallResponse();

            StartCoroutine(ShowWaitingDisplay("Logging in...", response)); // display

            var loginCallbacks = new APIOperationCallbacks
                (
                onSucess: () =>
                {
                    response.status.SetSucess();
                    LoginSucess();
                },
                onfailure: (errorReason) =>
                {
                    LoginFailure(errorReason);
                    response.status.SetError(errorReason);
                }
                );

            if (!NetworkHandler.Instance._useDebugAcountLogin) // this can be removed after login
            {
                NetworkHandler.Instance.AnonymousLoginPlayer(loginCallbacks);
            }
            else
            {
                NetworkHandler.Instance.AnonymousLoginDebugPlayer(loginCallbacks);
            }

            return response;
        }


        IEnumerator ShowWaitingDisplay(string message, CallResponse callResponse)
        {
            messagText.enabled = true;
            loadingImage.enabled = true;
            messagText.text = message;
            while (!callResponse.status.Complete)
            {
                loadingImage.transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime); // spinny wheel
                yield return null;
            }
        }

        private void LoginSucess()
        {
            messagText.enabled = true;
            messagText.text = "Logged in";
            loadingImage.enabled = false;
        }

        private void LoginFailure(FailureReason errorReason)
        {
            messagText.enabled = true;
            messagText.text = "Unable to log in... Please check your internet and try again later";
            loadingImage.enabled = false;
        }

        private CallResponse<ReadOnlyCollection<NetworkGame>> GetGames()
        {
            var response = new CallResponse<ReadOnlyCollection<NetworkGame>>();

            StartCoroutine(ShowWaitingDisplay("Gathering games...", response)); // display

            var getGamesCallback = new APIOperationCallbacks<ReadOnlyCollection<NetworkGame>>
                (
                onSucess: (games) =>
                {
                    GamesGatheredSucess(games);
                    response.returnData = games;
                    response.status.SetSucess();
                },
                onfailure: (errorReason) =>
                {
                    if(errorReason == FailureReason.PlayerIsMemberOfNoGames)
                    {
                        GamesGatheredButMemberOfNoGames();
                        response.returnData = new ReadOnlyCollection<NetworkGame>(new List<NetworkGame>());
                        response.status.SetSucess();
                        return;
                    }
                    GamesGatheredFailure(errorReason);
                    response.status.SetError(errorReason);
                }
                );

            NetworkHandler.Instance.GatherAllMemberGames(getGamesCallback);

            return response;
        }

       

        private void GamesGatheredSucess(ReadOnlyCollection<NetworkGame> games)
        {
            messagText.enabled = true;
            loadingImage.enabled = false;

            List<NetworkGame> openGames, activeGames;
            NetworkGame.SeperateOpenAndClosedGames(games, out openGames, out activeGames);

            messagText.text = $"Member of {activeGames.Count} active games and {openGames.Count} open games";
        }

        //private void PopulateListDisplay(List<NetworkGame> openGames, List<NetworkGame> activeGames)
        //{
        //    List<NetworkGame> sortedGames = new List<NetworkGame>();
        //    sortedGames.AddRange(activeGames);
        //    sortedGames.AddRange(openGames);
        //    gamesList.Populate(sortedGames);
        //}

        private void GamesGatheredButMemberOfNoGames()
        {
            messagText.enabled = true;
            messagText.text = "You are not in any games, why not start a new one by pressing \"New Game\"";
            loadingImage.enabled = false;
        }

        private void GamesGatheredFailure(FailureReason errorReason)
        {
            messagText.enabled = true;
            messagText.text = "Unable to gather games... Please check your internet and try again later";
            loadingImage.enabled = false;
        }

        // called by button press
        public void EnterNewGame()
        {
            StartCoroutine(NewGame());
        }

        private IEnumerator NewGame()
        {
            EnableEnterGamePannel();

            // join the new game
            NetworkGame game;
            {
                CallResponse<NetworkGame> newGameResponse = JoinNewGameCall();
                yield return new WaitUntil(() => newGameResponse.status.Complete);

                if (newGameResponse.status.Error) // do not continue in the case of error
                {
                    yield break;
                }

                game = newGameResponse.returnData;
            }


            if (game == null)
            {
                NewGameJoinedFailure(FailureReason.UnknownError);
                yield break;
            }

            //get the game data
            {
                if (!game.NewGameJustCreated || true) // -- get the data from a brand new game
                {
                    CallResponse<NetworkGame.UsableData> getGameDataResponse = GetGameDataCall(game);

                    yield return new WaitUntil(() => getGameDataResponse.status.Complete);

                    if (getGameDataResponse.status.Error)
                    {
                        yield break;
                    }

                }
            }


            // create a local save of the game
            {
                SaveSystemInternal.GameMetaData localMetaGameData = SaveManager.Instance.CreateNewGame(new SaveSystemInternal.GameMetaData(game));
                if (localMetaGameData == null) // was unsucessful
                {
                    NewGameJoinedFailure(FailureReason.LocalSaveSystemError);
                    NetworkHandler.Instance.LogoutPlayer();
                    yield break;
                }

                // set this save to be active
                if (!SaveManager.Instance.OpenGame(localMetaGameData))
                {
                    NewGameJoinedFailure(FailureReason.LocalSaveSystemError);
                    NetworkHandler.Instance.LogoutPlayer();
                    yield break;
                }
            }

            EnterGame(game);

        }

        private void EnterGame(NetworkGame game)
        {

            if (NetworkHandler.Instance.PlayerClient == null || !NetworkHandler.Instance.PlayerClient.IsLoggedIn)
            {
                NewGameJoinedFailure(FailureReason.UnknownError);
                return;
            }

            if (NetworkHandler.Instance.NetGame == null || !NetworkHandler.Instance.NetGame.IsInGame)
            {
                NewGameJoinedFailure(FailureReason.UnknownError);
                return;
            }

            game.DetermineGameContext();

            if(game.EnteredGameContext == null)
            {
                NewGameJoinedFailure(FailureReason.UnknownError);
                return;
            }

            if (!game.EnteredGameContext.AllowedIntoGame)
            {
                NewGameJoinedFailure(FailureReason.GameNotBegun);
                return;
            }

            var nextScene = game.EnteredGameContext.SceneEntering;

            
            SceneChangeController.Instance.ChangeScene(nextScene);
        }


        private CallResponse<NetworkGame> JoinNewGameCall()
        {
            var response = new CallResponse<NetworkGame>();

            StartCoroutine(ShowEnterGameDisplay("Finding new game...",response));

            var enterGamesCallback = new APIOperationCallbacks<NetworkGame>
               (
               onSucess: (game) =>
               {
                   response.returnData = game;
                   response.status.SetSucess();
                   NewGameJoinedSucess(game);
               },
               onfailure: (errorReason) =>
               {
                   NewGameJoinedFailure(errorReason);
                   response.status.SetError(errorReason);
               }
               );

            NetworkHandler.Instance.EnterNewGame(enterGamesCallback);

            return response;

        }

        private IEnumerator ShowEnterGameDisplay(string message, CallResponse callResponse)
        {
            enterGameText.enabled = true;
            enterGameLoadingImage.enabled = true;
            enterGameText.text = message;
            while (!callResponse.status.Complete)
            {
                enterGameLoadingImage.transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime); // spinny wheel
                yield return null;
            }
        }

        private void NewGameJoinedSucess(NetworkGame game)
        {
            if (!NetworkHandler.Instance.InGame)
            {
                NewGameJoinedFailure(FailureReason.UnknownError);
                return;
            }

            enterGameText.enabled = true;
            enterGameText.text = "Game found";
            enterGameLoadingImage.enabled = false;

        }


        private void NewGameJoinedFailure(FailureReason errorReason)
        {
            string message;
            switch (errorReason)
            {
                case FailureReason.TooManyActiveGames:
                    message = $"You have too many ongoing games!\nFor this prototype there is a {NetComponent.maximumActiveGames} games limit.\n" +
                        $"Consider finishing up an existing game before starting a new one.";
                    break;
                case FailureReason.AboveOpenGamesLimit:
                    message = $"There are no new open games available.\nYou currenlty are hosting {NetComponent.maximumOpenGames} open game{(NetComponent.maximumOpenGames != 1 ? "s" : "")}, which is the current limit.\n" +
                        $"Check back later and somone might have joined {(NetComponent.maximumOpenGames != 1 ? "one of your games" : "your game")}!";
                    break;
                case FailureReason.ItIsTheOtherPlayersTurn:
                    message = $"Game joined sucessfully, but the other player has not taken their turn yet.\n" +
                        $"Why not come back in a bit?";
                    break;
                case FailureReason.GameNotBegun:
                    message = $"Joined a game!\nThe game is still being set up by the other player, please check back in a moment";
                    break;
                default:
                    message = $"Unable find new game: {errorReason}";
                    break;
            }

            enterGameText.enabled = true;
            enterGameText.text = message;
            enterGameLoadingImage.enabled = false;
        }


    
        private CallResponse<NetworkGame.UsableData> GetGameDataCall(NetworkGame game)
        {
            var response = new CallResponse<NetworkGame.UsableData>();

            StartCoroutine(ShowEnterGameDisplay("Getting Games...", response)); // display

            var callbacks = new APIOperationCallbacks<NetworkGame.UsableData>(
                onSucess: (data) => { 
                    GetGameDataSucess(data, game);
                    response.returnData = data;
                    response.status.SetSucess();
                },

                onfailure: (e) =>
                {
                    GetGameDataFailure(e);
                    response.status.SetError(e);
                });

            NetworkHandler.Instance.ReceiveData(callbacks, game);


            return response;

        }

        private void GetGameDataFailure(FailureReason e)
        {
            var message = $"The game data could not be accessed: {e}";

            enterGameText.enabled = true;
            enterGameText.text = message;
            enterGameLoadingImage.enabled = false;
        }

        private void GetGameDataSucess(NetworkGame.UsableData data, NetworkGame game)
        {
            game.SetGameData(data);
        }

        public void ResumeGame(NetworkGame game)
        {
            APIOperationCallbacks<NetworkGame> callbacks = new APIOperationCallbacks<NetworkGame>(ResumedGameSucess,ResumedGameFailure);
            NetworkHandler.Instance.ResumeMemberGame(game, callbacks);
        }

        private void ResumedGameSucess(NetworkGame game)
        {
            if (game.CurrentGameData == null)
            {
                NewGameJoinedFailure(FailureReason.UnknownError);
                return;
            }

            // open local game file
            {
                SaveSystemInternal.GameMetaData localGameMetadata = new SaveSystemInternal.GameMetaData(game);
                bool sucess = SaveManager.Instance.OpenGame(localGameMetadata);
                // if this fails, try create a new game and join that
                if (!sucess)
                {
                    Debug.LogError("Local save of member game does not exist, creating new");
                    var newLocalGame = SaveManager.Instance.CreateNewGame(localGameMetadata);
                    if(newLocalGame == null)
                    {
                        NewGameJoinedFailure(FailureReason.LocalSaveSystemError);
                        return;
                    }
                    sucess = SaveManager.Instance.OpenGame(localGameMetadata);
                    if (!sucess)
                    {
                        NewGameJoinedFailure(FailureReason.LocalSaveSystemError);
                        return;
                    }
                }
            }

            EnterGame(game);


        }

        private void ResumedGameFailure(FailureReason e)
        {
            string message;

            if (e != FailureReason.ItIsTheOtherPlayersTurn)
            {
                message = $"Something went wrong, the game could not be joined: {e}";
            }
            else
            {
                message = $"Game joined sucessfully, but the other player has not taken their turn yet.\n" +
                         $"Why not come back in a bit?";

            }

            enterGameText.enabled = true;
            enterGameText.text = message;
            enterGameLoadingImage.enabled = false;
        }

        void EnableBackButton() => backButton.interactable = true;
        void DisableBackButton() => backButton.interactable = false;    
        
        void EnableNewGameButton() => newGameButton.gameObject.SetActive(true);
        void DisableNewGameButton() => newGameButton.gameObject.SetActive(false);        
        
        void EnableEnterGamePannel() => enterGamePannel.SetActive(true);
        void DisableEnterGamePannel() => enterGamePannel.SetActive(false);

        public void ToMainMenu()
        {
            NetworkHandler.Instance.LogoutPlayer();
            SceneChangeController.Instance.ChangeScene(SceneChangeController.Scenes.MainMenu);
        }


    }
}
