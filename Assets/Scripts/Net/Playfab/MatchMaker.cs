using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NetSystem
{
    using System;
    using PlayFab;
    using PlayFab.GroupsModels;

    public class MatchMaker : MonoBehaviour
    {

        PlayFab.CloudScriptModels.EntityKey ClientEntity => client.ClientEntityKey;
        PlayerClient client;

        public void Init(PlayerClient client)
        {
            this.client = client;
            
        }


        public IEnumerator GetAllMyGames(APIOperationCallbacks<List<NetworkGame>> resultCallbacks)
        {
            
            // get the groups we are members of (including open unstarted games)
            var listGroupResponse = new CallResponse<List<PlayFab.GroupsModels.GroupWithRoles>>();

            yield return StartCoroutine(ListMyGroups(listGroupResponse));

            if (listGroupResponse.status.Error)
            {
                resultCallbacks.OnFailure();
                yield break;
            }

            // if we aren't in any groups, return empty list
            if(listGroupResponse.returnData.Count == 0)
            {
                resultCallbacks.OnSucess(new List<NetworkGame>());
                yield break;
            }

            // get the meta data on the games
            var gameDataRequestResponses = new List<CallResponse<NetworkGame.NetworkGameMetadata>>();

            // make each call individually as playfab limits internal API calls to 15 per execution
            foreach (GroupWithRoles group in listGroupResponse.returnData)
            {
                var response = new CallResponse<NetworkGame.NetworkGameMetadata>();

                gameDataRequestResponses.Add(response);

                StartCoroutine(GetGroupMetaData(group.Group, response)); // not yeilding here for maximum eficeincy

            }

            // wait for these to finish
            foreach (var response in gameDataRequestResponses)
            {
                yield return new WaitUntil(() => { return response.status.Complete; });
                if(response.status.Error)
                {
                    resultCallbacks.OnFailure();
                    yield break;
                }
            }


            // pack and return these
            var netGames = new List<NetworkGame>();

            for (int i = 0; i < gameDataRequestResponses.Count; i++)
            {
                var netGame = new NetworkGame(listGroupResponse.returnData[i], gameDataRequestResponses[i].returnData);
                netGames.Add(netGame);
            }

            resultCallbacks.OnSucess(netGames);

        }

     

        // private IEnumerator ListMyGroups( out (CallStatus, List<PlayFab.GroupsModels.GroupWithRoles> ) response) // cannot pass ref param to iterator
        // private IEnumerator ListMyGroups( Action<(CallStatus, List<PlayFab.GroupsModels.GroupWithRoles> ) > callback) // this was stupid
        private IEnumerator ListMyGroups(CallResponse<List<PlayFab.GroupsModels.GroupWithRoles>> callResponse)
        {



            var request = new PlayFab.CloudScriptModels.ExecuteEntityCloudScriptRequest
            {
                FunctionName = "ListMyGroups",
                FunctionParameter = new
                {
                    Entity = ClientEntity
                }
            };


            PlayFabCloudScriptAPI.ExecuteEntityCloudScript(
                request, 
                (PlayFab.CloudScriptModels.ExecuteCloudScriptResult obj) => { ListMyGroupSucess(obj); },
                (PlayFabError obj) => ScriptExecutedfailure(obj, callResponse)
                );


            // local function
            void ListMyGroupSucess(PlayFab.CloudScriptModels.ExecuteCloudScriptResult obj) 
            {
              
                var response = DeserialiseResponse<PlayFab.GroupsModels.ListMembershipResponse>(obj);

                if (response == null)
                {
                    callResponse.status.SetError();
                }

                //callResponse.returnData.AddRange(response.Groups);
                callResponse.returnData = response.Groups;

                callResponse.status.SetSucess();
            }

            yield return new WaitUntil(() => { Debug.Log("Loading"); return callResponse.status.Complete; });
            Debug.Log("List fetched");

        }

        private IEnumerator GetGroupMetaData(EntityKey group, CallResponse<NetworkGame.NetworkGameMetadata> callResponse)
        {
            var request = new PlayFab.CloudScriptModels.ExecuteEntityCloudScriptRequest
            {
                FunctionName = "GetGroupData",
                FunctionParameter = new
                {
                    Group = group
                }
            };

            PlayFabCloudScriptAPI.ExecuteEntityCloudScript(
              request,
              (PlayFab.CloudScriptModels.ExecuteCloudScriptResult obj) => { GetDataSucess(obj); },
              (PlayFabError obj) => ScriptExecutedfailure(obj, callResponse)
              );


            // local function
            void GetDataSucess(PlayFab.CloudScriptModels.ExecuteCloudScriptResult obj)
            {
                //var response = DeserialiseResponse<PlayFab.DataModels.GetObjectsResponse>(obj);
               // var response = DeserialiseResponseGeneric<PlayFab.DataModels.ObjectResult>(obj);
                var response = DeserialiseResponseGeneric<NetworkGame.NetworkGameMetadata>(obj);

                if (response == null)
                {
                    callResponse.status.SetError();
                }

                callResponse.returnData = response;

                callResponse.status.SetSucess();

            }

            yield return new WaitUntil(() => { Debug.Log("Loading"); return callResponse.status.Complete; });
            Debug.Log("List fetched");

        }
        private void ScriptExecutedfailure<T>(PlayFabError obj, CallResponse<T> callResponse)
        {
            Debug.LogError(obj.GenerateErrorReport());
            callResponse.status.SetError();
            
        }

        private T DeserialiseResponse<T>(PlayFab.CloudScriptModels.ExecuteCloudScriptResult obj) where T : PlayFab.SharedModels.PlayFabResultCommon
        {

            if (obj.Error != null)
            {
                LogError(obj.Error);
                return null;
            }

            object objResult = obj.FunctionResult;

            if(objResult == null)
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

        private T DeserialiseResponseGeneric<T>(PlayFab.CloudScriptModels.ExecuteCloudScriptResult obj)
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


        private void LogCannotDeserialiseError(PlayFab.CloudScriptModels.ExecuteCloudScriptResult obj)
        {
            Debug.LogError($"Result from {obj.FunctionName} result {obj.FunctionResult} cannot be deserilaised");
        }

        private void LogObjectResultIsNullError(PlayFab.CloudScriptModels.ExecuteCloudScriptResult obj)
        {
            Debug.LogError($"Response from {obj.FunctionName} had no result");
        }

        private void LogError(PlayFab.CloudScriptModels.ScriptExecutionError error)
        {
            Debug.LogError(error.Error + " " + error.Message + " " + error.StackTrace);
        }

        //private void LogError(PlayFab.ClientModels.ScriptExecutionError error)
        //{
        //    Debug.LogError(error.Error + " " + error.Message + " " + error.StackTrace);
        //}




    }
}