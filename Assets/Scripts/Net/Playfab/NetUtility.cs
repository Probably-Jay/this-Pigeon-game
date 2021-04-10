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

        public CallResponse()
        {
            status = CallStatus.NotComplete;
        }

        /// <summary>
        /// Class representing the satus of an API call
        /// </summary>
        public class CallStatus
        {
            public bool Complete { get; private set; }
            public bool Error { get; private set; }

            public FailureReason ErrorData { get; private set; }

            public void SetNotComplete() { Complete = false; Error = true; }
            public void SetSucess() { Complete = true; Error = false; }
      

            public void SetError(FailureReason reason) { Complete = true; Error = true; ErrorData = reason; }

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

        public CallResponse() : base()
        {
            returnData = default;
        }      
        public CallResponse(T data) : base()
        {
            returnData = data;
        }
    }




    public enum FailureReason 
    { 
        None

        ,PlayFabError
        ,InternalError

        ,TooManyActiveGames
        ,NoOpenGamesAvailable
    }


    public class APIOperationCallbacks
    {
        public readonly Action OnSucess;
        public readonly Action<FailureReason> OnFailure;

        public APIOperationCallbacks(Action onSucess, Action<FailureReason> onfailure)
        {
            this.OnSucess = onSucess;
            this.OnFailure = onfailure;
        }

    } 
    public class APIOperationCallbacks<Ts>
    {
        public readonly Action<Ts> OnSucess;
        public readonly Action<FailureReason> OnFailure;

        public APIOperationCallbacks(Action<Ts> onSucess, Action<FailureReason> onfailure)
        {
            this.OnSucess = onSucess;
            this.OnFailure = onfailure;
        }

    }


}