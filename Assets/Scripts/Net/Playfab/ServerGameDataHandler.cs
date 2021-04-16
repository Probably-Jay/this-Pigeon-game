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

        public NetworkGame NetworkGame => NetworkHandler.Instance.NetGame.CurrentNetworkGame;

        public IEnumerator GetDataFromTheServer(APIOperationCallbacks<string> resultsCallback)
        {
            var getDataResponse = new CallResponse<string>();
            {

                yield return StartCoroutine(ReceiveDataFromTheServer(getDataResponse));

                if (getDataResponse.status.Error)
                {
                    resultsCallback.OnFailure(FailureReason.PlayFabError);
                    yield break;
                }
            }

            resultsCallback.OnSucess(getDataResponse.returnData);
        }

        private IEnumerator ReceiveDataFromTheServer(CallResponse<string> getDataResponse)
        {
            var request = new PlayFab.CloudScriptModels.ExecuteEntityCloudScriptRequest
            {
                FunctionName = "GetGroupSharedData",
                FunctionParameter = new
                {
                    Group = NetworkGame.GroupEntityKey,
                }
            };

            PlayFabCloudScriptAPI.ExecuteEntityCloudScript(
                request: request,
                resultCallback: (PlayFab.CloudScriptModels.ExecuteCloudScriptResult obj) => { ReceiveDataSucess(obj); },
                errorCallback: (PlayFabError obj) => ScriptExecutedfailure(obj, getDataResponse)
                );

            // local function
            void ReceiveDataSucess(PlayFab.CloudScriptModels.ExecuteCloudScriptResult obj)
            {
  
              //  string result = DeserialiseResponseToCutomObject<string>(obj);
                var result = DeserialiseResponseToCutomObject<NetworkGame.RawData>(obj);

                if (result == null)
                {
                    getDataResponse.status.SetError(FailureReason.InternalError);
                    return;
                }

                //if (result == "")
                //{
                //    LogError(obj.Error);
                //    getDataResponse.status.SetError(FailureReason.InternalError);
                //}

              //  getDataResponse.returnData = result;

                getDataResponse.status.SetSucess();
            }

            yield return new WaitUntil(() => { Debug.Log("Getting"); return getDataResponse.status.Complete; });

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
            EntityKey groupEntityKey = NetworkGame.GroupEntityKey;
            var request = new PlayFab.CloudScriptModels.ExecuteEntityCloudScriptRequest
            {
                FunctionName = "SetGroupSharedData",
                FunctionParameter = new
                {
                    Group = groupEntityKey,
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