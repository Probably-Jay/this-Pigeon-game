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

        /// <summary>
        /// Creates a new open game on the server, which will contain the calling client and the title entity.
        /// <para/> Upon completion will invoke one of the following callbacks :
        /// <para><see cref="APIOperationCallbacks{NetworkGame}.OnSucess"/>: The game was sucessfuly created and entered, its metadata was gathered, and a new <see cref="NetworkGame"/> object has been stored</para>
        /// <para><see cref="APIOperationCallbacks{NetworkGame}.OnFailure"/>: The call failed due to a networking error, or for one of the following reasons (retuned in callback): 
        ///     <list type="bullet">
        ///         <item>
        ///         <term><see cref="FailureReason.TooManyActiveGames"/></term>
        ///         <description>The user is a member of too many games to enter a new one</description>
        ///         </item> 
        ///         <item>
        ///         <term><see cref="FailureReason.AboveOpenGamesLimit"/></term>
        ///         <description>The user cannot create a new game as there are no open games to join and they own too many open games to start a new one</description>
        ///         </item>
        ///     </list>
        /// </para>
        /// </summary>
        /// <param name="resultsCallback">Callbakcs for the sucess or failure of this action</param>
        public IEnumerator CreateGame(APIOperationCallbacks<NetworkGame> resultsCallback)
        {
            // gather member games
            ReadOnlyCollection<NetworkGame> cachedMemberGames;
            {

                var response = new CallResponse<ReadOnlyCollection<NetworkGame>>();

                yield return StartCoroutine(GetMemberGamesList(response));

                if (response.status.Error)
                {
                    //resultsCallback.OnFailure(response.status.ErrorData);
                    //yield break;
                    switch (response.status.ErrorData)
                    {
                        case FailureReason.PlayerIsMemberOfNoGames:
                            // this is fine, continue 
                            break;
                        default:
                            resultsCallback.OnFailure(response.status.ErrorData);
                            yield break;
                    }

                }

                cachedMemberGames = response.returnData;
                
            }

            // validate we are allowed to create a new game
            {
                if (!ValidateIfBelowGameLimit(cachedMemberGames))
                {
                    resultsCallback.OnFailure(FailureReason.TooManyActiveGames);
                    yield break;
                }

                // cont open games we are in
                int myOpenGamesCount = 0;
                foreach (var game in cachedMemberGames)
                {
                    if (game.GameOpenToJoin)
                    {
                        myOpenGamesCount++;
                    }
                }

                // validate we are below open game limit
                if (myOpenGamesCount >= maximumOpenGames)
                {
                    resultsCallback.OnFailure(FailureReason.AboveOpenGamesLimit);
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

            var networkGame = new NetworkGame(createdGroup, metaData, newGame: true);

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