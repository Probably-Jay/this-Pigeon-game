using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using System;

namespace NetSystem
{
    using PlayFab;
    using PlayFab.GroupsModels;
    public abstract class NetComponent : MonoBehaviour
    {
        protected PlayFab.CloudScriptModels.EntityKey ClientEntity => NetworkHandler.Instance.ClientEntity;

        public const int maximumActiveGames = 10;
        public const int maximumOpenGames = 1;

        protected bool ValidateIfBelowGameLimit(ReadOnlyCollection<NetworkGame> cachedMemberGames) // server ideally should handle this
        {
            int numberOfMemberGames = cachedMemberGames.Count;

            // some upper limit on number of mambered games
            if (numberOfMemberGames >= maximumActiveGames)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Get the metadata of a group
        /// </summary>
        protected IEnumerator GetGroupMetaData(EntityKey group, CallResponse<NetworkGame.NetworkGameMetadata> callResponse)
        {
            var request = new PlayFab.CloudScriptModels.ExecuteEntityCloudScriptRequest
            {
                FunctionName = "GetGroupMetaData",
                FunctionParameter = new
                {
                    Group = group
                }
            };

            PlayFabCloudScriptAPI.ExecuteEntityCloudScript(
              request: request,
              resultCallback: (PlayFab.CloudScriptModels.ExecuteCloudScriptResult obj) => { GetDataSucess(obj); },
              errorCallback: (PlayFabError obj) => ScriptExecutedfailure(obj, callResponse)
              );


            // local function
            void GetDataSucess(PlayFab.CloudScriptModels.ExecuteCloudScriptResult obj)
            {
                var response = DeserialiseResponseToCutomObject<NetworkGame.NetworkGameMetadata>(obj);

                if (response == null)
                {
                    callResponse.status.SetError(FailureReason.InternalError);
                    return;
                }

                callResponse.returnData = response;

                callResponse.status.SetSucess();

            }

            yield return new WaitUntil(() => { Debug.Log("Loading"); return callResponse.status.Complete; });


        }


        protected IEnumerator GetMemberGamesList(CallResponse<ReadOnlyCollection<NetworkGame>> response)
        {
            var gatherGameCallbacks = new APIOperationCallbacks<ReadOnlyCollection<NetworkGame>>(
                  onSucess: (ReadOnlyCollection<NetworkGame> games) => {
                      response.returnData = games;
                      response.status.SetSucess();
                  },
                  onfailure: (FailureReason) => response.status.SetError(FailureReason)
                  );

            NetworkHandler.Instance.GatherAllMemberGames(gatherGameCallbacks);



            yield return new WaitUntil(() => response.status.Complete);

            if (response.status.Error)
            {
                switch (response.status.ErrorData)
                {
                    case FailureReason.PlayerIsMemberOfNoGames:
                        // do nothing, continue
                        break;
                    default:
                        response.status.SetError(response.status.ErrorData);
                        yield break;
                }
            }

            if (response.returnData == null)
            {
                response.status.SetError(FailureReason.InternalError);
                yield break;
            }

            response.status.SetSucess();
        }


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
                var data = JsonUtility.FromJson<T>(stringValue);
                response = data;
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