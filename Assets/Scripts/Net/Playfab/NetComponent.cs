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
     
        protected void ScriptExecutedfailure(PlayFabError obj, CallResponse callResponse)
        {
            Debug.LogError(obj.GenerateErrorReport());
            callResponse.status.SetError(FailureReason.PlayFabError);
        }

        /// <summary>
        /// Deserialise an API response to a playfab object <see cref="{T}"/>. Returns <c>null</c> in case of error
        /// </summary>
        /// <param name="obj">A response from an API call</param>
        /// <returns><see cref="{T}"/> or <c>null</c></returns>
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

        /// <summary>
        /// Deserialise an API response to a custom class <see cref="{T}"/>. Returns <c>null</c> in case of error
        /// </summary>
        /// <param name="obj">A response from an API call</param>
        /// <returns><see cref="{T}"/> or <c>null</c></returns>
        protected T DeserialiseResponseToCutomObject<T>(PlayFab.CloudScriptModels.ExecuteCloudScriptResult obj) where T : class
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