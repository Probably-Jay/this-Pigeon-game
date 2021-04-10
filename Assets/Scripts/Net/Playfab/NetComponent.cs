using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NetSystem
{
    using PlayFab;
    using PlayFab.GroupsModels;
    public abstract class NetComponent : MonoBehaviour
    {
        protected void ScriptExecutedfailure<T>(PlayFabError obj, CallResponse<T> callResponse)
        {
            Debug.LogError(obj.GenerateErrorReport());
            callResponse.status.SetError();

        }

        protected T DeserialiseResponseToPlayfabObject<T>(PlayFab.CloudScriptModels.ExecuteCloudScriptResult obj) where T : PlayFab.SharedModels.PlayFabResultCommon
        {

            if (obj.Error != null)
            {
                LogError(obj.Error);
                return null;
            }

            object objResult = obj.FunctionResult;

            if (objResult == null)
            {
                LogObjectResultIsNullError(obj);
                return null;
            }

            string stringValue = objResult.ToString();

            T response;
            try
            {
                response = JsonUtility.FromJson<T>(stringValue);
            }
            catch (Exception)
            {
                LogCannotDeserialiseError(obj);
                return null;
            }


            return response;
        }

        protected T DeserialiseResponseToCutomObject<T>(PlayFab.CloudScriptModels.ExecuteCloudScriptResult obj)
        {
            if (obj.Error != null)
            {
                LogError(obj.Error);
                return default;
            }

            object objResult = obj.FunctionResult;

            if (objResult == null)
            {
                LogObjectResultIsNullError(obj);
                return default;
            }

            string stringValue = objResult.ToString();

            T response;
            try
            {
                response = JsonUtility.FromJson<T>(stringValue);
            }
            catch (Exception)
            {
                LogCannotDeserialiseError(obj);
                return default;
            }


            return response;
        }


        protected void LogCannotDeserialiseError(PlayFab.CloudScriptModels.ExecuteCloudScriptResult obj)
        {
            Debug.LogError($"Result from {obj.FunctionName} result {obj.FunctionResult} cannot be deserilaised");
        }

        protected void LogObjectResultIsNullError(PlayFab.CloudScriptModels.ExecuteCloudScriptResult obj)
        {
            Debug.LogError($"Response from {obj.FunctionName} had no result");
        }

        protected void LogError(PlayFab.CloudScriptModels.ScriptExecutionError error)
        {
            Debug.LogError(error.Error + " " + error.Message + " " + error.StackTrace);
        }

    }
}