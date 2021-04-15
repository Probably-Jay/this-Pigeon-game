using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetSystem
{
    /// <summary>
    /// Debugging class, interface to <see cref="NetworkHandler"/> as scene buttons cannot referance singletons directly
    /// </summary>
    public class NetworkHandlerSceneInterface : MonoBehaviour
    {
        public void LoginPlayer() => NetworkHandler.Instance.AnonymousLoginPlayer();
        public void DebugLoginPlayer() => NetworkHandler.Instance.AnonymousLoginDebugPlayer();

        public void JoinNewGame() => NetworkHandler.Instance.EnterNewGame();
        public void UpdateGameData() => NetworkHandler.Instance.SendData();


    }
}