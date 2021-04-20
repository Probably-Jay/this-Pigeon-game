using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public static class NetUtility 
//{
//    public static Func<CallResponse,T> APISucess<T>(T obj, Func<T,CallResponse> sucessFunc, ref CallResponse response)
//    {

//        CallResponse Function<T1>(T1 obj, Func<T1, CallResponse> sucessFunc, CallResponse response)
//        {
//            var r = sucessFunc(obj);
//            response.error = r.error;
//            response.complete = true;
//            return response;
//        };

//        return Function<T>;
//    }
//}

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
        
        ,PlayerIsMemberOfNoGames

        ,TooManyActiveGames
        ,NoOpenGamesAvailable

        ,AboveOpenGamesLimit

        ,PlayerIsNotAMemberOfThisGame

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

    }

}