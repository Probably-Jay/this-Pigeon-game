using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NetSystem
{
    using System;
    using PlayFab;
    using PlayFab.CloudScriptModels;

    public class ServerGameDataHandler : NetComponent
    {
        PlayFab.CloudScriptModels.EntityKey ClientEntity => client.ClientEntityKey;
        PlayerClient client;

        public NetworkGame networkGame;

        public void Init(PlayerClient client, NetworkGame networkGame)
        {
            this.client = client;
            this.networkGame = networkGame;
        }

        public IEnumerator SaveDataToServer(string data, APIOperationCallbacks resultsCallback)
        {
            {
                var sendDataResponse = new CallResponse();

                yield return StartCoroutine(SendDataToServer(data, sendDataResponse));

                if (sendDataResponse.status.Error)
                {
                    resultsCallback.OnFailure(FailureReason.PlayFabError);
                    yield break;
                }
            }

            resultsCallback.OnSucess();

        }

        private IEnumerator SendDataToServer(string data, CallResponse sendDataResponse)
        {
            var request = new PlayFab.CloudScriptModels.ExecuteEntityCloudScriptRequest
            {
                FunctionName = "SetGroupSharedData",
                FunctionParameter = new
                {
                    Group = networkGame.GroupEntityKey,
                    Data = data
                }

            };

            PlayFabCloudScriptAPI.ExecuteEntityCloudScript(
            request: request,
            resultCallback: (PlayFab.CloudScriptModels.ExecuteCloudScriptResult obj) => { SendDataSucess(obj); },
            errorCallback: (PlayFabError obj) => ScriptExecutedfailure(obj, sendDataResponse)
            );


            // local function
            void SendDataSucess(PlayFab.CloudScriptModels.ExecuteCloudScriptResult obj)
            {

                if (obj.Error != null)
                {
                    LogError(obj.Error);
                    sendDataResponse.status.SetError(FailureReason.PlayFabError);
                }


                sendDataResponse.status.SetSucess();
            }

            yield return new WaitUntil(() => { Debug.Log("Sending"); return sendDataResponse.status.Complete; });
       
        }
    }
}