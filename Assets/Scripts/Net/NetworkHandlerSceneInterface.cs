using System.Collections;
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
        [SerializeField] InputField inputField;

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
                onGamesGatheredSucess: (ReadOnlyCollection<NetworkGame> games) => Debug.Log($"Member of {games.Count} games"),
                onGamesGatherFailure: (_) => Debug.LogError(_)
                ) ;
        }

        public void ResumeMemberGameFromInputFeild()
        {
            if (!NetworkHandler.Instance.LoggedIn) return;

            ResumeMemberGame(int.Parse(inputField.text));
        }

        private void ResumeMemberGame(int i)
        {
            NetworkHandler.Instance.GatherAllMemberGames(
                   onGamesGatheredSucess: (ReadOnlyCollection<NetworkGame> games) => Resume(games[i]),
                   onGamesGatherFailure: (_) => Debug.LogError(_)
                   );
        }

        private void Resume(NetworkGame game)
        {
            NetworkHandler.Instance.ResumeMemberGame(game);

        }

        public void GetGameData()
        {
            if (!NetworkHandler.Instance.InGame) return;

            NetworkHandler.Instance.ReceiveData();
        }  
        
        public void UpdateGameData()
        {
            if (!NetworkHandler.Instance.InGame) return;

            NetworkHandler.Instance.SendData();
        }
    }
}