﻿using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;

namespace NetSystem
{
    using PlayFab.CloudScriptModels;
    /// <summary>
    /// Debugging class, interface to <see cref="NetworkHandler"/> as scene buttons cannot referance singletons directly
    /// </summary>
    public class NetworkHandlerSceneInterface : MonoBehaviour
    {
        [SerializeField] InputField gameInputField;
        [SerializeField] InputField dataInputField;

        public void LoginPlayer() => NetworkHandler.Instance.AnonymousLoginPlayer();
        public void DebugLoginPlayer() => NetworkHandler.Instance.AnonymousLoginDebugPlayer();

        public void JoinNewGame()
        {
            if (!NetworkHandler.Instance.LoggedIn) return;

            NetworkHandler.Instance.EnterNewGame();
        }

        public void ListMemberGames()
        {
            if (!NetworkHandler.Instance.LoggedIn) return;

            NetworkHandler.Instance.GatherAllMemberGames(
                onGamesGatheredSucess: ListGames,
                onGamesGatherFailure: (FailureReason reason) =>
                {
                    if(reason == FailureReason.PlayerIsMemberOfNoGames)
                    {
                        Debug.Log("Member of no games");
                        return;
                    }
                    Debug.LogError($"Request failed because of: {reason}");
                });
        }

        private void  ListGames(ReadOnlyCollection<NetworkGame> games)
        {
           
            List<NetworkGame> openGames = new List<NetworkGame>();
            List<NetworkGame> activeGames = new List<NetworkGame>();
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

            Debug.Log($"Member of {activeGames.Count} active games and {openGames.Count} open games");

        }

        public void ResumeMemberGameFromInputField()
        {
            if (!NetworkHandler.Instance.LoggedIn)
            {
                Debug.LogError("Not logged in");
                return;
            }

            ResumeMemberGame(int.Parse(gameInputField.text));
        }

        private void ResumeMemberGame(int i)
        {
            NetworkHandler.Instance.GatherAllMemberGames(
                   onGamesGatheredSucess: (ReadOnlyCollection<NetworkGame> games) =>
                   {
                       if(i>= games.Count)
                       {
                           Debug.LogError($"Index {i} is beyond the bounds of this member games array");
                           return;
                       }
                       Resume(games[i]);
                   },
                   onGamesGatherFailure: (FailureReason reason) => Debug.LogError($"Request failed because of: {reason}")
                   );
        }

        private void Resume(NetworkGame game)
        {
            NetworkHandler.Instance.ResumeMemberGame(game);

        }

        public void GetGameData()
        {
            if (!NetworkHandler.Instance.InGame)
            {
                Debug.LogError("Not in a game");
                return;
            }

            NetworkHandler.Instance.ReceiveData();
        }  
        
        public void UpdateGameData()
        {
            if (!NetworkHandler.Instance.InGame)
            {
                Debug.LogError("Not in a game");
                return;
            }

            var data = new NetworkGame.RawData()
            {
                gardenA = dataInputField.text,
                gardenB = "Some other data"
            };

            NetworkHandler.Instance.SendData(data);
        }
    }
}