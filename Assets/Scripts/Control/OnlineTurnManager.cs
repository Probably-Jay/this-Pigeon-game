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

        private void CreateLocalPlayer(Player.PlayerEnum playerWeAre)
        {
            LocalPlayer = InstantiatePlayer(playerWeAre);
        }

        /// <summary>
        /// Create player data for server, on create game
        /// </summary>
        private void CreatePlayerData(Player.PlayerEnum playerWeAre)
        {
           
            switch (playerWeAre)
            {
                case Player.PlayerEnum.Player1:
                    GameManager.Instance.DataManager.PlayerData.player1ID = LocalPlayer.PlayerClient.ClientEntityKey.Id;
                    break;
                case Player.PlayerEnum.Player2:
                    GameManager.Instance.DataManager.PlayerData.player1ID = NetworkHandler.Instance.NetGame.CurrentNetworkGame.UsableGameData.playerData.player1ID;
                    GameManager.Instance.DataManager.PlayerData.player2ID = LocalPlayer.PlayerClient.ClientEntityKey.Id;
                    break;
            }

            //    players.Add(LocalPlayer.EnumID, LocalPlayer);
        }

        private void ReLoadPlayerData(Player.PlayerEnum playerWeAre)
        {
            PlayerDataPacket data = NetworkHandler.Instance.NetGame.CurrentNetworkGame.UsableGameData.playerData;
            GameManager.Instance.DataManager.PlayerData.player1ID = data.player1ID;
            GameManager.Instance.DataManager.PlayerData.player2ID = data.player2ID;
        }

        private Player InstantiatePlayer(Player.PlayerEnum playerID)
        {
            GameObject playerObject = Instantiate(playerPrefab);
            playerObject.transform.SetParent(transform);

            Player player = playerObject.GetComponent<Player>();
            player.Init(NetworkHandler.Instance.PlayerClient, playerID);


            return player;
        }

        public void CreateGame(NetworkGame.EnterGameContext context)
        {
            TurnTracker.CreateGame(context);
        }

        public void ReloadGameOnly(NetworkGame.EnterGameContext context)
        {
            TurnTracker.ReLoad(context);
        }

        public void CreatePlayer(NetworkGame.EnterGameContext context)
        {
            CreateLocalPlayer(context.playerWeAre);
            CreatePlayerData(context.playerWeAre);
            ClaimTurn(); // will reset turn points
            
        }

        public void ReloadPlayer(NetworkGame.EnterGameContext context)
        {
            CreateLocalPlayer(context.playerWeAre);
            ReLoadPlayerData(context.playerWeAre);
            LocalPlayer.TurnPoints.Reload(context);
        }

       

        public void ClaimTurn()
        {
            TurnTracker.ClaimTurn();
            LocalPlayer.TurnPoints.ResetPoints(LocalPlayer.EnumID);
        }



        //public void ReloadGame(NetworkGame.EnterGameContext context)
        //{
        //    TurnTracker.ReLoad(context);
        //    LocalPlayer.TurnPoints.Reload(context);
        //}

        //private void Resume(Player.PlayerEnum playerID)
        //{
        //    LocalPlayer = InstantiatePlayer(playerID);
        //}

        //public void ResumeGame(NetworkGame.EnterGameContext context)
        //{
        //    Resume(context.playerWeAre);

        //}












        ////public void ResumedGamePlaying(Player.PlayerEnum playerWeAre, Player.PlayerEnum playerWhoOwnsTurn)
        ////{
        ////    Debug.Log("Resumed game playing");


        ////    ReLoad(playerWeAre);
        ////    TurnTracker.ResumedGame(playerWeAre, playerWhoOwnsTurn);
        ////    LocalPlayer.TurnPoints.Resume(playerWeAre);

        ////    GameManager.Instance.DataManager.PlayerData.turnOwner = NetworkHandler.Instance.NetGame.CurrentNetworkGame.usableData.playerData.turnOwner;

        ////}

        ////public void JoinedGameNewPlaying(Player.PlayerEnum playerWeAre, Player.PlayerEnum playerWhoOwnsTheTurn)
        ////{
        ////    Debug.Log("Joined game playing");

        ////    Initialise(playerWeAre);
        ////    TurnTracker.ResumedGame(playerWeAre, playerWhoOwnsTheTurn);
        ////    LocalPlayer.TurnPoints.Resume(playerWeAre);

        ////    GameManager.Instance.DataManager.PlayerData.turnOwner = NetworkHandler.Instance.NetGame.CurrentNetworkGame.usableData.playerData.turnOwner;
        ////} 

        ////public void JoinedGameNewSpectator(Player.PlayerEnum playerWeAre, Player.PlayerEnum playerWhoOwnsTheTurn)
        ////{
        ////    Debug.Log("Joined game spectating");

        ////    Initialise(playerWeAre);
        ////    TurnTracker.ResumedGame(playerWeAre, playerWhoOwnsTheTurn);
        ////    LocalPlayer.TurnPoints.ResumeSpectator(playerWeAre);

        ////    GameManager.Instance.DataManager.PlayerData.turnOwner = NetworkHandler.Instance.NetGame.CurrentNetworkGame.usableData.playerData.turnOwner;
        ////}


        ////public void ResumedGameSpectating(Player.PlayerEnum playerWeAre, Player.PlayerEnum PlayerWhosTurnItIs)
        ////{
        ////    ReLoad(playerWeAre);
        ////    TurnTracker.ResumedGame(playerWeAre, PlayerWhosTurnItIs);
        ////    LocalPlayer.TurnPoints.ResumeSpectator(playerWeAre);

        ////    GameManager.Instance.DataManager.PlayerData.turnOwner = NetworkHandler.Instance.NetGame.CurrentNetworkGame.usableData.playerData.turnOwner;
        ////}

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