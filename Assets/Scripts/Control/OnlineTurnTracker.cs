using NetSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    /// <summary>
    /// Class to track the players turn in the online game
    /// </summary>
    public class OnlineTurnTracker 
    {
        public int Turn { get; private set; } = 0;

        /// <summary>
        /// The player who currently owns the turn
        /// </summary>
        public Player.PlayerEnum TurnOwner { get; private set; }

        /// <summary>
        /// Marks if the turn is complete and ready to be handed over
        /// </summary>
        public bool TurnComplete { get; private set; }

        /// <summary>
        /// The turn belongs to the local player
        /// </summary>
        private bool OurTurn => GameManager.Instance.OnlineTurnManager.LocalPlayer.EnumID == TurnOwner;

        /// <summary>
        /// The turn belongs to the local player and they have not marked the turn as complete
        /// </summary>
        public bool CanPlayTurn => (!TurnComplete && OurTurn);

        /// <summary>
        /// The turn belongs to the remote player and they have marked the turn as complete
        /// </summary>
        public bool CanClaimTurn => (TurnComplete && !OurTurn);

        /// <summary>
        /// Set the local player as owning the turn and mark the turn as not complete
        /// </summary>
        /// <returns>If the action was legal</returns>
        //public bool ClaimTurn()
        //{
        //    if (OurTurn)
        //    {
        //        return true;
        //    }

        //    if (!CanClaimTurn)
        //    {
        //        return false;
        //    }

        //    TurnComplete = false;
        //    TurnOwner = GameManager.Instance.OnlineTurnManager.LocalPlayer.EnumID;
        //    return true;
        //}

       



        /// <summary>
        /// Mark the turn as complete and ready to be claimed by the remote player
        /// </summary>
        /// <returns>If the action was legal</returns>
        public bool CompleteTurn()
        {
            if (!OurTurn)
            {
                return false;
            }

            SetEndTurnInData();

            TurnComplete = true;


            return true;
        }

       

        private static void SetEndTurnInData()
        {
            GameManager.Instance.DataManager.SetStateOfTurnComplete(true);
        }

        public void CreateGame(NetworkGame.EnterGameContext context)
        {
            Turn = -1;
            GameManager.Instance.DataManager.InitialiseTurnCounter();
           
        }

        public void ReLoad(NetworkGame.EnterGameContext context)
        {
            //if (context.claimingTurn) // already done
            //{
            //    return;
            //}

            var data = NetSystem.NetworkHandler.Instance.NetGame.CurrentNetworkGame.usableData;

            Turn = data.playerData.turnCounter;
            TurnComplete = data.playerData.turnComplete;
            string turnBelongsToID = data.turnBelongsTo;
            TurnOwner = NetUtility.PlayfabIDToPlayerEnum(turnBelongsToID, NetworkHandler.Instance.NetGame.CurrentNetworkGame.usableData);

            GameManager.Instance.DataManager.SetTurnCounter(Turn);
            GameManager.Instance.DataManager.SetStateOfTurnComplete(TurnComplete);
            GameManager.Instance.DataManager.SetTheTurnsOwner(turnBelongsToID);

        }

        public void ClaimTurn()
        {
            Turn++;
            TurnComplete = false;
            TurnOwner = GameManager.Instance.OnlineTurnManager.LocalPlayer.EnumID;

            GameManager.Instance.DataManager.SetTurnCounter(Turn);
            GameManager.Instance.DataManager.SetStateOfTurnComplete(false);
            GameManager.Instance.DataManager.SetTheTurnsOwner(GameManager.Instance.OnlineTurnManager.LocalPlayer.PlayerClient.ClientEntityKey.Id);

        }

        /// <summary>
        /// Initialise a brand new game
        /// </summary>
        //public void InitialiseNewGame()
        //{
        //    Turn = 0;
        //    TurnComplete = false;
        //    TurnOwner = GameManager.Instance.OnlineTurnManager.LocalPlayer.EnumID;

        //    GameManager.Instance.DataManager.InitialiseTurnCounter();

        //}


        //public void ResumedGame(Player.PlayerEnum playerWeAre, Player.PlayerEnum playerWhoOwnsTurn)
        //{
        //    var data = NetSystem.NetworkHandler.Instance.NetGame.CurrentNetworkGame.usableData;

        //    if (data.NewTurn)
        //    {
        //        data.playerData.turnCounter++;
        //        data.playerData.turnComplete = false;
        //        data.playerData.turnOwner = NetSystem.NetworkHandler.Instance.ClientEntity.Id;
        //    }

        //    Turn = data.playerData.turnCounter;
        //    TurnComplete = data.playerData.turnComplete;
        //    TurnOwner = playerWhoOwnsTurn;

        //    GameManager.Instance.DataManager.SetTurnCounter(Turn);

        //}
       


    }
}