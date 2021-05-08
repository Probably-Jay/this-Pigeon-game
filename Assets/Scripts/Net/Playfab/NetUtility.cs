using PlayFab.CloudScriptModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NetSystem
{


    /// <summary>
    /// Class which encapsulates if an API call was completed and was sucessful.
    /// </summary>
    public class CallResponse
    {
        public readonly CallStatus status;

        /// <summary>
        /// Construct a new <see cref="CallResponse"/> and set the status to NotComplete
        /// </summary>
        public CallResponse()
        {
            status = CallStatus.NotComplete;
        }

        /// <summary>
        /// Class representing the satus of an API call
        /// </summary>
        public class CallStatus
        {
            /// <summary>
            /// If the call has been completed
            /// </summary>
            public bool Complete { get; private set; }

            /// <summary>
            /// If the call returned an error
            /// </summary>
            public bool Error { get; private set; }

            /// <summary>
            /// The reason for error
            /// </summary>
            public FailureReason ErrorData { get; private set; }

           
           // public void SetNotComplete() { Complete = false; Error = true; }

            /// <summary>
            /// Flag this call as complete and sucessful
            /// </summary>
            public void SetSucess() { Complete = true; Error = false; }

            /// <summary>
            /// Flag this call as complete but returning an error
            /// </summary>
            public void SetError(FailureReason reason) { Complete = true; Error = true; ErrorData = reason; }


            /// <summary>
            /// Internal use only do not call. Create a new <see cref="CallStatus"/> and mark it as not complete
            /// </summary>
            internal static CallStatus NotComplete => new CallStatus(complete: false, error: true);


            private CallStatus(bool complete, bool error)
            {
                Complete = complete;
                Error = error;
                ErrorData = FailureReason.None;
            }

            public void Set(CallStatus from) { Complete = from.Complete; Error = from.Error; }

        }
    }



    /// <summary>
    /// Class which encapsulates the return values of an API call as well as if the call completed and was sucessful.
    /// </summary>
    public class CallResponse<T> : CallResponse
    {
        public T returnData;

        /// <summary>
        /// Construct a new <see cref="CallResponse"/> and set the status to NotComplete and the return data to <c>default</c>
        /// </summary>
        public CallResponse() : base()
        {
            returnData = default;
        }

        /// <summary>
        ///  Construct a new <see cref="CallResponse"/> and set the status to NotComplete and the return data to <paramref name="data"/>
        /// </summary>
        /// <param name="data">Default data value</param>
        public CallResponse(T data) : base()
        {
            returnData = data;
        }
    }




    public enum FailureReason 
    { 
        None

        /// An error occured within a call to playfab API. Often no meaninful distinction from <see cref="InternalError"/>
        , PlayFabError
        /// An error occured after a call to playfab API. Often no meaninful distinction from <see cref="InternalError"/>
        , InternalError
        
        , LocalSaveSystemError
        
        ,PlayerIsMemberOfNoGames

        ,TooManyActiveGames
        ,NoOpenGamesAvailable

        ,AboveOpenGamesLimit

        ,PlayerIsNotAMemberOfThisGame

        ,GameNotBegun

        ,ItIsTheOtherPlayersTurn
        
        /// The reason for the error is unknown, but the program entered an illegal state during an API call
        ,UnknownError
    }

    /// <summary>
    /// Wrapper for callbacks to be invoked after playfab API calls
    /// </summary>
    public class APIOperationCallbacks
    {
        /// <summary>
        /// Should be called after sucessful API operation
        /// </summary>
        public readonly Action OnSucess;

        /// <summary>
        /// Should be called after unsucessful API operation, and be passed the reason for failure
        /// </summary>
        public readonly Action<FailureReason> OnFailure;

        /// <summary>
        /// Provide functions to be called in the event of sucess or failure
        /// </summary>
        /// <param name="onSucess">Should be called after sucessful API operation</param>
        /// <param name="onfailure">Should be called after unsucessful API operation, and be passed the reason for failure</param>
        public APIOperationCallbacks(Action onSucess, Action<FailureReason> onfailure)
        {
            this.OnSucess = onSucess;
            this.OnFailure = onfailure;
        }

        /// <summary>
        /// Helper constructor for do-nothing callbacks with empty delegates
        /// </summary>
        public static APIOperationCallbacks DoNothing => new APIOperationCallbacks(() => { }, (_) => { });

    }

    /// <summary>
    /// Wrapper for callbacks to be invoked after playfab API call where sucessful calls will return a value through the <see cref="OnSucess"/> callback
    /// </summary>
    /// <typeparam name="Ts">The type that <see cref="OnSucess"/> will take as a paramater</typeparam>
    public class APIOperationCallbacks<Ts>
    {
        /// <summary>
        /// Should be called after sucessful API operation and passed a paramater of type <see cref="Ts"/>
        /// </summary>
        public readonly Action<Ts> OnSucess;

        /// <summary>
        /// Should be called after unsucessful API operation, and be passed the reason for failure
        /// </summary>
        public readonly Action<FailureReason> OnFailure;

        /// <summary>
        /// Provide functions to be called in the event of sucess or failure
        /// </summary>
        /// <param name="onSucess">Should be called after sucessful API operation and passed a paramater of type <see cref="Ts"/></param>
        /// <param name="onfailure">Should be called after unsucessful API operation, and be passed the reason for failure</param>
        public APIOperationCallbacks(Action<Ts> onSucess, Action<FailureReason> onfailure)
        {
            this.OnSucess = onSucess;
            this.OnFailure = onfailure;
        }

        /// <summary>
        /// Helper constructor for do-nothing callbacks with empty delegates
        /// </summary>
        public static APIOperationCallbacks<Ts> NoCallbacks => new APIOperationCallbacks<Ts>((_) => { }, (_) => { });

    }

    public static class NetUtility
    {
        public static string GetEntityUniqueGroupName(PlayFab.CloudScriptModels.EntityKey clientEntity, DateTime time)
        {
            //return SaveSystemInternal.SaveDataUtility.ComputeHashToAsciiString(clientEntity.Id + time.Ticks.ToString());
             return clientEntity.Id + "-" + time.Ticks.ToString();

              
        }

        public static Player.PlayerEnum PlayfabIDToPlayerEnum(string playerID, NetworkGame.UsableData data)
        {
            return playerID == data.gameStartedBy ? Player.PlayerEnum.Player1 : Player.PlayerEnum.Player2;
        }

        public static bool AllowedToTakeTurn(NetworkGame.UsableData data) // todo replace this with actual struct
        {

            bool ourTurn = data.turnBelongsTo == NetSystem.NetworkHandler.Instance.ClientEntity.Id;

            // if the turn is not complete, we can play if it is our turn (naturally), if the turn is complete we can play if it is (was) our oponents turn
            return CanTakeTurn(data.turnComplete, ourTurn);

        }

        public static bool? AllowedToJoin(NetworkGame.UsableData data)
        {
            if(data == null)
            {
                return null;
            }

            bool ourTurn = data.turnBelongsTo == NetSystem.NetworkHandler.Instance.ClientEntity.Id;

            if (ourTurn || data.gameBegun)
            {
                return true;
            }

            return false;

        }

        private static bool CanTakeTurn(bool turnComplete, bool ourTurn) => !turnComplete ? ourTurn : !ourTurn;

        public static bool CanClaimTurn(bool turnComplete, bool ourTurn)
        {
            return turnComplete && !ourTurn;
        }

        private static bool IsOurTurn(NetworkGame.UsableData data, PlayerClient player) => data.turnBelongsTo == player.ClientEntityKey.Id;
        private static bool TurnBelongsToPlayerOne(NetworkGame.UsableData data) => data.turnBelongsTo == data.gameStartedBy;

        private static bool IsPlayerOne(NetworkGame.UsableData data, PlayerClient player) => data.gameStartedBy == player.ClientEntityKey.Id;





        public static EnterGameContext DetermineEnterGameContext(NetworkGame.UsableData data, PlayerClient player)
        {
            if(data == null || player == null)
            {
                return EnterGameContext.Error;
            }

            bool gameBegun = data.gameBegun;
            bool weArePlayerOne = IsPlayerOne(data, player);

            if (!gameBegun)
            {
                return DetermineContextEnteringGameNotBegun(weArePlayerOne);
            }

            bool turnComplete = data.turnComplete;
            bool isPlayerOnesTurn = TurnBelongsToPlayerOne(data);
            bool playerTwoInitilised = gameBegun && data.playerData.Player2Initilised;

            if (!weArePlayerOne) // we are player 2
            {
                return DetermineContextPlayer2(turnComplete, isPlayerOnesTurn, playerTwoInitilised);
            }
            else // we are player 1
            {
                return DeterminContextPlayer1(turnComplete, isPlayerOnesTurn);
            }    

        }

        private static EnterGameContext DetermineContextEnteringGameNotBegun(bool isPlayerOne)
        {
            if (isPlayerOne)
            {
                return EnterGameContext.CreateNewGamePlayer1;
            }
            else
            {
                return EnterGameContext.CannotEnterUnilitilasedGamePlayer2;
            }
        }

        private static EnterGameContext DetermineContextPlayer2(bool turnComplete, bool isPlayerOnesTurn, bool playerTwoInitilised)
        {
            if (!playerTwoInitilised) // we are uninitialised
            {
                // will be player 1s turn if we are uninitilaised 

                return DeterminContextUninitilisedPlayer2(turnComplete);
            }
            else // we are initialised
            {
                return DetermineContextInitialisedPlayer2(turnComplete, isPlayerOnesTurn);
            }
        }

        private static EnterGameContext DetermineContextInitialisedPlayer2(bool turnComplete, bool isPlayerOnesTurn)
        {
            if (!isPlayerOnesTurn) // it is our turn
            {
                return DeterminContextOurTurnPlayer2(turnComplete);
            }
            else // player 1s turn
            {
                return DetermineContextTheirTurnPlayer2(turnComplete);
            }
        }

        private static EnterGameContext DetermineContextTheirTurnPlayer2(bool turnComplete)
        {
            if (!turnComplete) // still player 1s turn
            {
                return EnterGameContext.ResumeSpectatingPlayer2;
            }
            else // player 1 finished turn
            {
                return EnterGameContext.ResumeClaimTurnPlayer2;
            }
        }

        private static EnterGameContext DeterminContextOurTurnPlayer2(bool turnComplete)
        {
            if (!turnComplete) // still our turn
            {
                return EnterGameContext.ResumePlayingPlayer2;
            }
            else // we have finished our turn
            {
                return EnterGameContext.ResumeSpectatingPlayer2;
            }
        }

        private static EnterGameContext DeterminContextUninitilisedPlayer2(bool turnComplete)
        {
            if (!turnComplete) // still player 1s turn
            {
                return EnterGameContext.CannotEnterUnilitilasedGamePlayer2;
            }
            else // player 1 has finished their turn
            {
                return EnterGameContext.JoinGamePlayer2;
            }
        }

        private static EnterGameContext DeterminContextPlayer1(bool turnComplete, bool isPlayerOnesTurn)
        {
            if (isPlayerOnesTurn) // our turn
            {
                return DeterminContextOurTurnPlayer1(turnComplete);
            }
            else // player 2s turn
            {
                return DeterminContextTheirTurnPlayer1(turnComplete);
            }
        }

        private static EnterGameContext DeterminContextTheirTurnPlayer1(bool turnComplete)
        {
            if (turnComplete) // they have finished their turn
            {
                return EnterGameContext.ResumeClaimTurnPlayer1;
            }
            else // it is still player 2s turn
            {
                return EnterGameContext.ResumeSpectatingPlayer1;
            }
        }

        private static EnterGameContext DeterminContextOurTurnPlayer1(bool turnComplete)
        {
            if (turnComplete) // we have finished our turn
            {
                return EnterGameContext.ResumeSpectatingPlayer1;
            }
            else // we have not finished our turn
            {
                return EnterGameContext.ResumePlayingPlayer1;
            }
        }

        internal static object DetermineEnterGameContext(NetworkGame.UsableData usableData, EntityKey playerClient)
        {
            throw new NotImplementedException();
        }

        public enum EnterGameContext
        {
            CreateNewGamePlayer1,
            JoinGamePlayer2,
            ResumePlayingPlayer1,
            ResumePlayingPlayer2,
            ResumeClaimTurnPlayer1,
            ResumeClaimTurnPlayer2,
            ResumeSpectatingPlayer1,
            ResumeSpectatingPlayer2,
            CannotEnterUnilitilasedGamePlayer2,
            Error
        }


    }

}