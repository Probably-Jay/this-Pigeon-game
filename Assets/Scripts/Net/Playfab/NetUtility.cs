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
    /// Struct representing the satus of an API call
    /// </summary>
    public class CallStatus
    {
        public bool Complete { get; private set; }
        public bool Error { get; private set; }


        public void SetNotComplete() { Complete = false; Error = true; }
        public void SetSucess() { Complete = true; Error = false; }
        public void SetError() { Complete = true; Error = true; }

        public static CallStatus NotComplete => new CallStatus(complete: false, error: true);
        //public static CallStatus Sucess => new CallStatus(complete: true, error: false);
        //public static CallStatus Error => new CallStatus(complete: true, error: true);

        private CallStatus(bool complete, bool error)
        {
            Complete = complete;
            Error = error;
        }

        public void Set(CallStatus from) { Complete = from.Complete; Error = from.Error; }

        //public static Func<bool> CallComplete(CallStatus status) => () => status.complete;
    }


    /// <summary>
    /// Class which encapsulates the return values of an API call as well as if the call completed and was sucessful.
    /// </summary>
    public class CallResponse<T>
    {
        public T returnData;

        public readonly CallStatus status;

        public CallResponse()
        {
            //returnData = data;
            returnData = default;
            status = CallStatus.NotComplete;
        }
    }
    /// <summary>
    /// Paramaterless overload
    /// </summary>
    public class CallResponse
    {
        public readonly CallStatus status;

        public CallResponse()
        {
            status = CallStatus.NotComplete;
        }
    }

    public class APIOperationCallbacks
    {
        readonly Action onSucess;
        readonly Action onFailure;

        APIOperationCallbacks(Action onScess, Action onfailure)
        {
            this.onSucess = onScess;
            this.onFailure = onfailure;
        }

    } 
    public class APIOperationCallbacks<Ts>
    {
        public readonly Action<Ts> OnSucess;
        public readonly Action OnFailure;

        public APIOperationCallbacks(Action<Ts> onScess, Action onfailure)
        {
            this.OnSucess = onScess;
            this.OnFailure = onfailure;
        }

    }


}