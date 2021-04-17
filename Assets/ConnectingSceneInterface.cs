using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using NetSystem;
using System;

namespace SceneControl
{

    public class ConnectingSceneInterface : MonoBehaviour
    {
        [SerializeField] TMP_Text messagText;
        [SerializeField] Image loadingImage;
        [SerializeField] Button backButton;
        [SerializeField] Button newGameButton;

       // Coroutine logginInCoroutine;

        const float rotationSpeed = 100;

        private void Start()
        {
            StartCoroutine(ServerOperations());
        }

        IEnumerator ServerOperations()
        {
            DisableNewGameButton();

            DisableBackButton();

            {
                CallResponse loginResponse = Login();
                yield return new WaitUntil(() => loginResponse.status.Complete);

                if (loginResponse.status.Error)
                {
                    yield break;
                }
            }

            {
                CallResponse getGamesResponse = GetGames();
                yield return new WaitUntil(() => getGamesResponse.status.Complete);

                if (getGamesResponse.status.Error && getGamesResponse.status.ErrorData != FailureReason.PlayerIsMemberOfNoGames)
                {
                    yield break;
                }
            }

            EnableBackButton();

            EnableNewGameButton();

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

        private CallResponse GetGames()
        {
            var response = new CallResponse();

            StartCoroutine(ShowWaitingDisplay("Gathering games...", response)); // display

            var getGamesCallback = new APIOperationCallbacks<ReadOnlyCollection<NetworkGame>>
                (
                onSucess: (games) =>
                {
                    GamesGatheredSucess(games);
                    response.status.SetSucess();
                },
                onfailure: (errorReason) =>
                {
                    if(errorReason == FailureReason.PlayerIsMemberOfNoGames)
                    {
                        MemberOfNoGames();
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

            // todo, display all active games in list (maybe the open game too if we have one)

            messagText.text = $"Member of {activeGames.Count} active games and {openGames.Count} open games";
        }

        private void MemberOfNoGames()
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


        public void EnterNewGame()
        {
            StartCoroutine(NewGame());
        }

        private IEnumerator NewGame()
        {
            {
                CallResponse newGameResponse = EnterNewGameCall();
                yield return new WaitUntil(() => newGameResponse.status.Complete);

                if (newGameResponse.status.Error)
                {
                    yield break;
                }
            }
        }

        private CallResponse EnterNewGameCall()
        {
            var response = new CallResponse();

            // pop-up here

            var enterGamesCallback = new APIOperationCallbacks<NetworkGame>
               (
               onSucess: (game) =>
               {
                   response.status.SetSucess();
                   NewGameEnteredSucess(game);
               },
               onfailure: (errorReason) =>
               {
                   NewGameEnteredFailure(errorReason);
                   response.status.SetError(errorReason);
               }
               );

            NetworkHandler.Instance.EnterNewGame(enterGamesCallback);

            return response;

            //StartCoroutine(ShowWaitingDisplay("Gathering games...", response)); // display
        }

        private void NewGameEnteredSucess(NetworkGame game)
        {
            throw new NotImplementedException();
        }

        private  void NewGameEnteredFailure(FailureReason errorReason)
        {
            throw new NotImplementedException(); 
        }


        void EnableBackButton() => backButton.interactable = true;
        void DisableBackButton() => backButton.interactable = false;    
        
        void EnableNewGameButton() => newGameButton.gameObject.SetActive(true);
        void DisableNewGameButton() => newGameButton.gameObject.SetActive(false);


    }
}
