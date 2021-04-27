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


        private void Initialise(Player.PlayerEnum playerID)
        {
            LocalPlayer = InstantiatePlayer();
            LocalPlayer.playerClient.PlayerGameEnumValue = playerID;
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


        public void ResumedGame(Player.PlayerEnum player)
        {
          //  Game.CurrentNetworkGame.

            Initialise(player);

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
            Initialise(Player.PlayerEnum.Player1);
            TurnTracker.InitialiseNewGame();
        }

        public void EndTurn()
        {

            TurnTracker.CompleteTurn();



            switch (LocalPlayer.EnumID)
            {
                case Player.PlayerEnum.Player1:
                    GameManager.Instance.DataManager.ResetPlayer2ActionPoints();
                    break;
                case Player.PlayerEnum.Player2:
                    GameManager.Instance.DataManager.ResetPlayer1ActionPoints();
                    break;
            }

        }
    }
}