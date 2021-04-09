using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NetSystem
{
    using System;
    using PlayFab;

    public class MatchMaker : MonoBehaviour
    {

        PlayFab.CloudScriptModels.EntityKey ClientEntity => client.ClientEntityKey;
        PlayerClient client;

        public void Init(PlayerClient client)
        {
            this.client = client;
            
        }


        public IEnumerator JoinNewGame()
        {
            {

                //CallStatus status = CallStatus.NotComplete; 
                List<PlayFab.GroupsModels.GroupWithRoles> groups = new List<PlayFab.GroupsModels.GroupWithRoles>();

                var callResponse = new CallResponse<List<PlayFab.GroupsModels.GroupWithRoles>>(groups);

                yield return StartCoroutine(ListMyGroups(callResponse));

                // callback: ( (CallStatus status, List<PlayFab.GroupsModels.GroupWithRoles> data) result ) => { response = result; }


                if (callResponse.status.error)
                {
                    Debug.LogError("error");
                }
                else
                {
                    Debug.Log("suceeeded");
                    Debug.Log(groups);
                    Debug.Log(groups?.Count);

                }



            }
            
            
        }

       // private IEnumerator ListMyGroups( out (CallStatus, List<PlayFab.GroupsModels.GroupWithRoles> ) response) // cannot pass ref param to iterator
       // private IEnumerator ListMyGroups( Action<(CallStatus, List<PlayFab.GroupsModels.GroupWithRoles> ) > callback)
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
                PlayFab.GroupsModels.ListMembershipResponse response = DeserialiseResponse<PlayFab.GroupsModels.ListMembershipResponse>(obj);

                if (response == null)
                {
                    callResponse.status.SetError();
                }

                callResponse.returnData.AddRange(response.Groups);

                callResponse.status.SetSucess();
            }

            yield return new WaitUntil(() => { Debug.Log("Loading"); return callResponse.status.complete; });
            Debug.Log("List fetched");

        }

        private void ScriptExecutedfailure<T>(PlayFabError obj, CallResponse<T> callResponse)
        {
            Debug.LogError(obj.GenerateErrorReport());
            callResponse.status.SetError();
            
        }

        private T DeserialiseResponse<T>(PlayFab.CloudScriptModels.ExecuteCloudScriptResult obj) where T : PlayFab.SharedModels.PlayFabResultCommon
        {
            // todo make this robust

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