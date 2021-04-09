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
        public bool complete;
        public bool error;


        public void SetNotComplete() { complete = false; error = true; }
        public void SetSucess() { complete = true; error = false; }
        public void SetError() { complete = true; error = true; }

        public static CallStatus NotComplete => new CallStatus(complete: false, error: true);
        //public static CallStatus Sucess => new CallStatus(complete: true, error: false);
        //public static CallStatus Error => new CallStatus(complete: true, error: true);

        private CallStatus(bool complete, bool error)
        {
            this.complete = complete;
            this.error = error;
        }

        public void Set(CallStatus from) { this.complete = from.complete; this.error = from.error; }

        //public static Func<bool> CallComplete(CallStatus status) => () => status.complete;
    }
}