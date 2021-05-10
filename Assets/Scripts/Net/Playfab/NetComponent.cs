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

        public const int maximumActiveGames = 250;
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

        /// <summary>
        /// Gathers all games this client is a member of, including unstarted open games. This data is cached.
        /// <para/> Upon completion will invoke one of the following callbacks :
        /// <para><see cref="APIOperationCallbacks{List{PlayFab.GroupsModels.GroupWithRoles}}.OnSucess"/>: The cache was hit, or the open games were sucessfully gathered along with their metadata and cached</para>
        /// <para><see cref="APIOperationCallbacks{List{PlayFab.GroupsModels.GroupWithRoles}}.OnFailure"/>: The call failed due to a networking error, or for one of the following reasons (retuned in callback): 
        ///     <list type="bullet">
        ///         <item>
        ///         <term><see cref="FailureReason.PlayerIsMemberOfNoGames"/></term>
        ///         <description>The player is not a member of any games. This is returned as an explicit error case to be handled</description>
        ///         </item> 
        ///     </list>
        /// </para>
        /// </summary>
        /// <param name="resultsCallback">Callbakcs for the sucess or failure of this action</param>
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
                        response.returnData = new ReadOnlyCollection<NetworkGame>(new List<NetworkGame>()); // set empty list
                        response.status.SetError(response.status.ErrorData);
                        yield break;
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