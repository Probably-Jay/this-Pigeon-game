using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NetSystem
{
    using System;
    using PlayFab;

    public class MatchMaker : MonoBehaviour
    {

        PlayFab.CloudScriptModels.EntityKey clientEntity;

        public void Init(PlayFab.CloudScriptModels.EntityKey clientEntity)
        {
            this.clientEntity = clientEntity;
            
        }


        public IEnumerator JoinNewGame()
        {
            {
                (CallStatus status, List<PlayFab.GroupsModels.GroupWithRoles> data) response= (CallStatus.NotComplete,null);
                yield return StartCoroutine(
                    ListMyGroups(
                        callback: ( (CallStatus status, List<PlayFab.GroupsModels.GroupWithRoles> data) result ) => { response = result; }
                        )
                    );

                
                if (response.status.error)
                {

                }

                

            }
            
            
        }

       // private IEnumerator ListMyGroups( out (CallStatus, List<PlayFab.GroupsModels.GroupWithRoles> ) response) // cannot pass ref param to iterator
        private IEnumerator ListMyGroups( Action<(CallStatus, List<PlayFab.GroupsModels.GroupWithRoles> ) > callback)
        {

           // response = (CallStatus.NotComplete, null);

            var request = new PlayFab.ClientModels.ExecuteCloudScriptRequest
            {
                FunctionName = "ListMyGroups",
                FunctionParameter = new
                {
                    Entity = clientEntity
                }
            };

            var status = CallStatus.NotComplete;
            List<PlayFab.GroupsModels.GroupWithRoles> groups = null;

            PlayFabClientAPI.ExecuteCloudScript(
                request, 
                (PlayFab.ClientModels.ExecuteCloudScriptResult obj) => { (status, groups) = ListMyGroupSucess(obj); },
                (PlayFabError obj) => { status = ScriptExecutedfailure(obj); }
                );


            (CallStatus, List<PlayFab.GroupsModels.GroupWithRoles>) ListMyGroupSucess(PlayFab.ClientModels.ExecuteCloudScriptResult obj) // local function
            {
                PlayFab.GroupsModels.ListMembershipResponse response = DeserialiseResponse<PlayFab.GroupsModels.ListMembershipResponse>(obj);

                //if (response == null)
                //{
                //    return (CallStatus.Error, null);
                //}

                //return (CallStatus.Sucess, response.Groups);
                return (null,null);
                
            }

            yield return new WaitUntil(() => status.complete);


            callback((status, groups)); // this feels insane


        }

        private CallStatus ScriptExecutedfailure(PlayFabError obj)
        {
            Debug.LogError(obj.GenerateErrorReport());
           // return CallStatus.Error;
            return null;
        }

        private T DeserialiseResponse<T>(PlayFab.ClientModels.ExecuteCloudScriptResult obj) where T : PlayFab.SharedModels.PlayFabResultCommon
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

        private void LogCannotDeserialiseError(PlayFab.ClientModels.ExecuteCloudScriptResult obj)
        {
            Debug.LogError($"Result from {obj.FunctionName} result {obj.FunctionResult} cannot be deserilaised");
        }

        private void LogObjectResultIsNullError(PlayFab.ClientModels.ExecuteCloudScriptResult obj)
        {
            Debug.LogError($"Response from {obj.FunctionName} had no result");
        }

        private void LogError(PlayFab.CloudScriptModels.ScriptExecutionError error)
        {
            Debug.LogError(error.Error + " " + error.Message + " " + error.StackTrace);
        }

        private void LogError(PlayFab.ClientModels.ScriptExecutionError error)
        {
            Debug.LogError(error.Error + " " + error.Message + " " + error.StackTrace);
        }




    }
}