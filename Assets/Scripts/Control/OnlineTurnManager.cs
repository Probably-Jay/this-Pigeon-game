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




       // public ActiveGame Game => NetworkHandler.Instance.NetGame;
        public Player LocalPlayer { get; private set; }
        public OnlineTurnTracker TurnTracker { get; private set; } = new OnlineTurnTracker();

//PlayerClient PlayerClient => NetworkHandler.Instance.PlayerClient;


        private void Initialise(Player.PlayerEnum playerID)
        {
            LocalPlayer = InstantiatePlayer(playerID);


            //GameManager.Instance.DataManager.PlayerData;

            switch (LocalPlayer.EnumID)
            {
                case Player.PlayerEnum.Player1:
                    GameManager.Instance.DataManager.PlayerData.player1ID = LocalPlayer.PlayerClient.ClientEntityKey.Id;

                    break;
                case Player.PlayerEnum.Player2:
                    GameManager.Instance.DataManager.PlayerData.player2ID = LocalPlayer.PlayerClient.ClientEntityKey.Id;

                    break;
 
            }

            //    players.Add(LocalPlayer.EnumID, LocalPlayer);
        }
        

        private Player InstantiatePlayer(Player.PlayerEnum playerID) 
        {
            GameObject playerObject = Instantiate(playerPrefab);
            playerObject.transform.SetParent(transform);
            Player player = playerObject.GetComponent<Player>();
            player.Init(NetworkHandler.Instance.PlayerClient);
            player.PlayerClient.PlayerGameEnumValue = playerID;
            return player;
        }

        private void ReLoad(Player.PlayerEnum playerID)
        {
            LocalPlayer = InstantiatePlayer(playerID);
           
        }

    

        public void ResumedGamePlaying(Player.PlayerEnum playerWeAre, Player.PlayerEnum playerWhoOwnsTurn)
        {
            ReLoad(playerWeAre);
            TurnTracker.ResumedGame(playerWeAre, playerWhoOwnsTurn);
            LocalPlayer.TurnPoints.Resume(playerWeAre);

            GameManager.Instance.DataManager.PlayerData.turnOwner = NetworkHandler.Instance.NetGame.CurrentNetworkGame.usableData.playerData.turnOwner;

        }



        public void InitialiseNewGame()
        {
            Initialise(Player.PlayerEnum.Player1);
            TurnTracker.InitialiseNewGame();
            LocalPlayer.TurnPoints.Initialise();
            GameManager.Instance.DataManager.PlayerData.turnOwner = LocalPlayer.PlayerClient.ClientEntityKey.Id;
        }

        public void JoinedGameNew()
        {
            //Initialise(Player.PlayerEnum.Player2);
            //TurnTracker.InitialiseNewGame();
            //LocalPlayer.TurnPoints.Initialise();
            //GameManager.Instance.DataManager.PlayerData.turnOwner = LocalPlayer.PlayerClient.ClientEntityKey.Id;
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