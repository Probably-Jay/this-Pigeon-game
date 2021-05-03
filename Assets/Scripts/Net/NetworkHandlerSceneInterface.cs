using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;

// jay



namespace NetSystem
{
    using PlayFab.CloudScriptModels;
    /// <summary>
    /// Debugging class, interface to <see cref="NetworkHandler"/> as scene buttons cannot referance singletons directly
    /// </summary>
    [System.Obsolete("This is a test object and should not be used, see " + nameof(SceneInterface.ConnectingSceneInterface))]
    public class NetworkHandlerSceneInterface : MonoBehaviour
    {
        [SerializeField] InputField gameInputField;
    //    [SerializeField] InputField dataInputField;

        public void LoginPlayer() => NetworkHandler.Instance.AnonymousLoginPlayer(APIOperationCallbacks.DoNothing);
        public void DebugLoginPlayer() => NetworkHandler.Instance.AnonymousLoginDebugPlayer(APIOperationCallbacks.DoNothing);

        public void LogoutPlayer() => NetworkHandler.Instance.LogoutPlayer();

        public void JoinNewGame()
        {
            if (!NetworkHandler.Instance.LoggedIn) return;

            NetworkHandler.Instance.EnterNewGame(APIOperationCallbacks<NetworkGame>.NoCallbacks);
        }

        public void ListMemberGames()
        {
            if (!NetworkHandler.Instance.LoggedIn) return;

            var gatherCallbacks = new APIOperationCallbacks<ReadOnlyCollection<NetworkGame>> (
                onSucess: ListGames,
                onfailure: (FailureReason reason) =>
                {
                    if (reason == FailureReason.PlayerIsMemberOfNoGames)
                    {
                        Debug.Log("Member of no games");
                        return;
                    }
                    Debug.LogError($"Request failed because of: {reason}");
                });

            NetworkHandler.Instance.GatherAllMemberGames(gatherCallbacks);
               
        }

        private void  ListGames(ReadOnlyCollection<NetworkGame> games)
        {
            List<NetworkGame> openGames, activeGames;
            NetworkGame.SeperateOpenAndClosedGames(games, out openGames, out activeGames);

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
            var gatherGamesCallbacks = new APIOperationCallbacks<ReadOnlyCollection<NetworkGame>>(
                  onSucess: (ReadOnlyCollection<NetworkGame> games) =>
                  {
                      if (i >= games.Count)
                      {
                          Debug.LogError($"Index {i} is beyond the bounds of this member games array");
                          return;
                      }
                      Resume(games[i]);
                  },
                  onfailure: (FailureReason reason) => Debug.LogError($"Request failed because of: {reason}")
                  );

            NetworkHandler.Instance.GatherAllMemberGames(gatherGamesCallbacks);
        }

        private void Resume(NetworkGame game)
        {
            NetworkHandler.Instance.ResumeMemberGame(game, APIOperationCallbacks<NetworkGame>.NoCallbacks);

        }

        public void GetGameData()
        {
            if (!NetworkHandler.Instance.InGame)
            {
                Debug.LogError("Not in a game");
                return;
            }

            var callbacks = new APIOperationCallbacks<NetworkGame.RawData>(
                onSucess: (data) => Debug.Log($"Data received"),
                onfailure: (e) => Debug.LogError(e)
                );

           // NetworkHandler.Instance.ReceiveData(callbacks);
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
                //gardenA = dataInputField.text,
                //gardenB = "Some other data"
            };

            NetworkHandler.Instance.SendData(APIOperationCallbacks.DoNothing, data);
        }
    }
}