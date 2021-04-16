using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;


namespace NetSystem
{
    using System;
    using PlayFab;
    using PlayFab.GroupsModels;

    public class MatchMaker : NetComponent
    {

  
        public IEnumerator CreateGame(APIOperationCallbacks<NetworkGame> resultsCallback)
        {
            // gather member games
            ReadOnlyCollection<NetworkGame> cachedMemberGames;
            {

                var response = new CallResponse<ReadOnlyCollection<NetworkGame>>();

                yield return StartCoroutine(GetMemberGamesList(response));

                if (response.status.Error)
                {
                    resultsCallback.OnFailure(response.status.ErrorData);
                    yield break;
                }

                cachedMemberGames = response.returnData;
                
            }

            // validate we are allowed to join a new game

            if (!ValidateIfBelowGameLimit(cachedMemberGames))
            {
                resultsCallback.OnFailure(FailureReason.TooManyActiveGames);
                yield break;
            }
            



            // validate we don't already have an open game
            foreach (var game in cachedMemberGames)
            {
                if (game.GameOpenToJoin)
                {
                    resultsCallback.OnFailure(FailureReason.AlreadyHasOpenGame);
                    yield break;
                }
            }

            // Create the group
            GroupWithRoles createdGroup;
            {
                var createGameRequest = new CallResponse<PlayFab.GroupsModels.CreateGroupResponse>();

                yield return StartCoroutine(CreateGameOnServer(createGameRequest));

                if (createGameRequest.status.Error)
                {
                    resultsCallback.OnFailure(createGameRequest.status.ErrorData);
                    yield break;
                }

                createdGroup  = new GroupWithRoles()
                {
                    Group = createGameRequest.returnData.Group
                    ,GroupName = createGameRequest.returnData.GroupName
                    , Roles = null // this would be a pain to populate and we don't need it
                    , ProfileVersion = createGameRequest.returnData.ProfileVersion
                };
              
            }

            // Get the created group's metadata
            NetworkGame.NetworkGameMetadata metaData;
            {
                var getMetadataRespone = new CallResponse<NetworkGame.NetworkGameMetadata>();

                yield return StartCoroutine(GetGroupMetaData(createdGroup.Group, getMetadataRespone));

                if (getMetadataRespone.status.Error)
                {
                    resultsCallback.OnFailure(getMetadataRespone.status.ErrorData);
                    yield break;
                }

                metaData = getMetadataRespone.returnData;
            }

            var networkGame = new NetworkGame(createdGroup, metaData);

            NetworkHandler.Instance.RemoteMemberGamesList.Add(networkGame);

            resultsCallback.OnSucess(networkGame);

        }

        private IEnumerator CreateGameOnServer(CallResponse<CreateGroupResponse> createGameRequest)
        {
            string name = NetUtility.GetEntityUniqueGroupName(ClientEntity, DateTime.Now);
            var request = new PlayFab.CloudScriptModels.ExecuteEntityCloudScriptRequest
            {
                FunctionName = "StartNewGameGroup",
                FunctionParameter = new
                {
                    Entity = ClientEntity,
                    GroupName = name
                }
            };

          

            PlayFabCloudScriptAPI.ExecuteEntityCloudScript(
               request: request,
               resultCallback: (PlayFab.CloudScriptModels.ExecuteCloudScriptResult obj) => { CreateGroupSucess(obj); },
               errorCallback: (PlayFabError obj) => ScriptExecutedfailure(obj, createGameRequest)
               );


            void CreateGroupSucess(PlayFab.CloudScriptModels.ExecuteCloudScriptResult obj)
            {

                var response = DeserialiseResponseToPlayfabObject<CreateGroupResponse>(obj);

                if (response == null)
                {
                    createGameRequest.status.SetError(FailureReason.InternalError);
                    return;
                }

                createGameRequest.returnData = response;

                createGameRequest.status.SetSucess();
            }

            yield return new WaitUntil(() => createGameRequest.status.Complete);
        }

     
    }
}