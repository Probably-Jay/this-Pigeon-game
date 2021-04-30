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

        /// <summary>
        /// Gets the data of the provided game (current game by default
        /// <para/> Upon completion will invoke one of the following callbacks :
        /// <para><see cref="APIOperationCallbacks{List{PlayFab.GroupsModels.GroupWithRoles}}.OnSucess"/>: The game data was sucessfully obtained</para>
        /// <para><see cref="APIOperationCallbacks{List{PlayFab.GroupsModels.GroupWithRoles}}.OnFailure"/>: The call failed due to a networking error (returned in callback)</para>
        /// </summary>
        /// <param name="resultsCallback">Callbakcs for the sucess or failure of this action</param>
        public IEnumerator GetDataFromTheServer(APIOperationCallbacks<NetworkGame.RawData> resultsCallback, NetworkGame game = null)
        {
            game = game != null ? game : NetworkGame;

            var getDataResponse = new CallResponse<NetworkGame.RawData>();
            {

                yield return StartCoroutine(ReceiveDataFromTheServer(getDataResponse, game));

                if (getDataResponse.status.Error)
                {
                    resultsCallback.OnFailure(FailureReason.PlayFabError);
                    yield break;
                }
            }

            resultsCallback.OnSucess(getDataResponse.returnData);
        }

        private IEnumerator ReceiveDataFromTheServer(CallResponse<NetworkGame.RawData> getDataResponse, NetworkGame game)
        {
          //  EntityKey groupEntityKey = NetworkGame.GroupEntityKey;
            var request = new PlayFab.CloudScriptModels.ExecuteEntityCloudScriptRequest
            {
                FunctionName = "GetGroupSharedData",
                FunctionParameter = new
                {
                    Group = game.GroupEntityKey,
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
  
                var result = DeserialiseResponseToCutomObject<NetworkGame.RawData>(obj);

                if (result == null)
                {
                    getDataResponse.status.SetError(FailureReason.InternalError);
                    return;
                }

                getDataResponse.returnData = result;

                getDataResponse.status.SetSucess();
            }

            yield return new WaitUntil(() => { Debug.Log("Getting"); return getDataResponse.status.Complete; });

        }


        /// <summary>
        /// Sends the data provided to the server, overwiting the current save data
        /// <para/> Upon completion will invoke one of the following callbacks :
        /// <para><see cref="APIOperationCallbacks{List{PlayFab.GroupsModels.GroupWithRoles}}.OnSucess"/>: The game data was sucessfully sent</para>
        /// <para><see cref="APIOperationCallbacks{List{PlayFab.GroupsModels.GroupWithRoles}}.OnFailure"/>: The call failed due to a networking error (returned in callback)</para>
        /// </summary>
        /// <param name="resultsCallback">Callbakcs for the sucess or failure of this action</param>
        public IEnumerator SaveDataToServer(NetworkGame.RawData data, APIOperationCallbacks resultsCallback)
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

        private IEnumerator SendDataToServer(NetworkGame.RawData data, CallResponse sendDataResponse)
        {
            EntityKey groupEntityKey = NetworkGame.GroupEntityKey;
            var request = new PlayFab.CloudScriptModels.ExecuteEntityCloudScriptRequest
            {
                FunctionName = "SetGroupSharedData",
                FunctionParameter = new
                {
                    Group = groupEntityKey,
                    Data = new {
                        // game begun set on server automatically
                        turnBelongsTo = data.turnBelongsTo,
                        turnComplete = data.turnComplete,
                        gardenData = data.gardenData,
                        playerData = data.playerData
                    }
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