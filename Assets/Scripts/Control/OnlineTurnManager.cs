using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetSystem;
using System;

namespace GameCore
{
    /// <summary>
    /// Manages a turn in the networked game, replaces <see cref="HotSeatManager"/>
    /// </summary>
    public class OnlineTurnManager : MonoBehaviour
    {
        [SerializeField] GameObject playerPrefab;




        public ActiveGame Game => NetworkHandler.Instance.NetGame;
        public Player LocalPlayer { get; private set; }
        public OnlineTurnTracker TurnTracker { get; private set; } = new OnlineTurnTracker();

        PlayerClient PlayerClient => NetworkHandler.Instance.PlayerClient;


        private void Initialise()
        {
            LocalPlayer = InstantiatePlayer();
            LocalPlayer.Init(PlayerClient);
        //    players.Add(LocalPlayer.EnumID, LocalPlayer);
        }
        

        private Player InstantiatePlayer() 
        {
            GameObject playerObject = Instantiate(playerPrefab);
            playerObject.transform.SetParent(transform);
            Player player = playerObject.GetComponent<Player>();
            return player;
        }


        public void ResumedGame()
        {
            Initialise();

            if (GameManager.Instance.Playing)
            {
                
            }
            else
            {

            }
            TurnTracker.ResumedGame();
            throw new System.NotImplementedException();
        }

        public void InitialiseNewGame()
        {
            Initialise();
            TurnTracker.InitialiseNewGame();
        }
    }
}