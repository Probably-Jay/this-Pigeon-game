using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NetSystem
{
    using System;
    using PlayFab;
    using PlayFab.GroupsModels;

    public class MatchMaker : NetComponent
    {

        PlayFab.CloudScriptModels.EntityKey ClientEntity => client.ClientEntityKey;
        PlayerClient client;

        public List<NetworkGame> cachedMemberGames = null;

        [SerializeField] int maximumActiveGames = 10;


        public void Init(PlayerClient client)
        {
            this.client = client;

        }

        /// <summary>
        /// Get a list of all game groups this client is a member of
        /// </summary>
        /// <param name="resultCallbacks">An object containing the funcitons to be called on sucess or failure of this operation</param>
        public IEnumerator GetAllMyGames(APIOperationCallbacks<List<NetworkGame>> resultCallbacks)
        {
            // if we have the cache, return it
            if (cachedMemberGames != null)
            {
                Debug.Log("Cached member games found, returning");
                resultCallbacks.OnSucess(cachedMemberGames);
                yield break;
            }

            // get the groups we are members of (including open unstarted games)
            List<GroupWithRoles> groupsWeAreIn;
            {
                var listGroupResponse = new CallResponse<List<PlayFab.GroupsModels.GroupWithRoles>>();

                yield return StartCoroutine(GetGroupsClientIsMemberOf(listGroupResponse));

                if (listGroupResponse.status.Error)
                {
                    resultCallbacks.OnFailure(listGroupResponse.status.ErrorData);
                    yield break;
                }

                groupsWeAreIn = listGroupResponse.returnData;
            }

            // if we aren't in any groups, return empty list
            if (groupsWeAreIn.Count == 0)
            {
                cachedMemberGames = new List<NetworkGame>();
                resultCallbacks.OnFailure(FailureReason.PlayerIsMemberOfNoGames);
                yield break;
            }

            // get the meta data on the games
            var gameDataRequestResponses = new List<CallResponse<NetworkGame.NetworkGameMetadata>>();
            {
                // make each call individually as playfab limits internal API calls to 15 per execution
                foreach (GroupWithRoles group in groupsWeAreIn)
                {
                    var response = new CallResponse<NetworkGame.NetworkGameMetadata>();

                    gameDataRequestResponses.Add(response);

                    StartCoroutine(GetGroupMetaData(group.Group, response)); // not yeilding here for maximum eficeincy
                }
            }

            // wait for previous calls to finish
            {
                foreach (var response in gameDataRequestResponses)
                {
                    yield return new WaitUntil(() => { return response.status.Complete; });
                    if (response.status.Error)
                    {
                        resultCallbacks.OnFailure(response.status.ErrorData);
                        yield break;
                    }
                }
            }

            // pack and return these
            var netGames = new List<NetworkGame>();
            {
                for (int i = 0; i < gameDataRequestResponses.Count; i++)
                {
                    var netGame = new NetworkGame(groupsWeAreIn[i], gameDataRequestResponses[i].returnData);
                    netGames.Add(netGame);
                }
            }

            cachedMemberGames = netGames;

            resultCallbacks.OnSucess(netGames);

        }

        // private IEnumerator ListMyGroups( out (CallStatus, List<PlayFab.GroupsModels.GroupWithRoles> ) response) // cannot pass ref param to iterator
        // private IEnumerator ListMyGroups( Action<(CallStatus, List<PlayFab.GroupsModels.GroupWithRoles> ) > callback) // this was stupid
        private IEnumerator GetGroupsClientIsMemberOf(CallResponse<List<PlayFab.GroupsModels.GroupWithRoles>> callResponse)
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
                request: request,
                resultCallback: (PlayFab.CloudScriptModels.ExecuteCloudScriptResult obj) => { ListMyGroupSucess(obj); },
                errorCallback: (PlayFabError obj) => ScriptExecutedfailure(obj, callResponse)
                );


            // local function
            void ListMyGroupSucess(PlayFab.CloudScriptModels.ExecuteCloudScriptResult obj)
            {

                var response = DeserialiseResponseToPlayfabObject<PlayFab.GroupsModels.ListMembershipResponse>(obj);

                if (response == null)
                {
                    callResponse.status.SetError(FailureReason.InternalError);
                    return;
                }

                //callResponse.returnData.AddRange(response.Groups);
                callResponse.returnData = response.Groups;

                callResponse.status.SetSucess();
            }

            yield return new WaitUntil(() => { Debug.Log("Loading"); return callResponse.status.Complete; });

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
              request: request,
              resultCallback: (PlayFab.CloudScriptModels.ExecuteCloudScriptResult obj) => { GetDataSucess(obj); },
              errorCallback: (PlayFabError obj) => ScriptExecutedfailure(obj, callResponse)
              );


            // local function
            void GetDataSucess(PlayFab.CloudScriptModels.ExecuteCloudScriptResult obj)
            {
                //var response = DeserialiseResponse<PlayFab.DataModels.GetObjectsResponse>(obj);
                // var response = DeserialiseResponseGeneric<PlayFab.DataModels.ObjectResult>(obj);
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


       

        public IEnumerator GetOpenGameGroups(APIOperationCallbacks<List<PlayFab.GroupsModels.GroupWithRoles>> resultsCallbacks)
        {

            // gather open games
            List<GroupWithRoles> openGroups;
            {
                var getOpenGamesResponse = new CallResponse<List<PlayFab.GroupsModels.GroupWithRoles>>();

                yield return StartCoroutine(GetOpenGamesFromServer(getOpenGamesResponse));

                if (getOpenGamesResponse.status.Error)
                {
                    resultsCallbacks.OnFailure(getOpenGamesResponse.status.ErrorData);
                    yield break;
                }

                // do not cache this value
                openGroups = getOpenGamesResponse.returnData;
            }

            // if there are no open games
            if (openGroups.Count == 0)
            {
                resultsCallbacks.OnFailure(FailureReason.NoOpenGamesAvailable);
                yield break;
            }


            // remove games we are in
            {
                // gather member games if not already cached
                if (cachedMemberGames == null)
                {
                    CallResponse response = new CallResponse();

                    var gatherCallbacks = new APIOperationCallbacks<List<NetworkGame>>(
                        onSucess: (_) => response.status.SetSucess()
                        , onfailure: (FailureReason) => response.status.SetError(FailureReason));

                    yield return StartCoroutine(GetAllMyGames(gatherCallbacks));

                    if (response.status.Error)
                    {
                        switch (response.status.ErrorData)
                        {
                            case FailureReason.PlayerIsMemberOfNoGames:
                                // do nothing
                                break;
                            default:
                                resultsCallbacks.OnFailure(response.status.ErrorData);
                                yield break;
                        }
                    }

                }

                // find matches
                List<GroupWithRoles> toRemove = new List<GroupWithRoles>();
                foreach (var memberGame in cachedMemberGames)
                {
                    foreach (var openGame in openGroups)
                    {
                        if(memberGame.GroupEntityKey.Id == openGame.Group.Id)
                        {
                            toRemove.Add(openGame);
                        }
                    }
                }

                // remove them
                foreach (var game in toRemove)
                {
                    openGroups.Remove(game);
                }
            }


            // if there are no open games
            if (openGroups.Count == 0)
            {
                resultsCallbacks.OnFailure(FailureReason.NoOpenGamesAvailable);
                yield break;
            }

            resultsCallbacks.OnSucess(openGroups);

        }

        private IEnumerator GetOpenGamesFromServer(CallResponse<List<GroupWithRoles>> getOpenGamesResponse)
        {
            var request = new PlayFab.CloudScriptModels.ExecuteEntityCloudScriptRequest
            {
                FunctionName = "ListOpenGroups"
            };


            PlayFabCloudScriptAPI.ExecuteEntityCloudScript(
               request: request,
               resultCallback: (PlayFab.CloudScriptModels.ExecuteCloudScriptResult obj) => { ListOpenGroupSucess(obj); },
               errorCallback: (PlayFabError obj) => ScriptExecutedfailure(obj, getOpenGamesResponse)
               );


            void ListOpenGroupSucess(PlayFab.CloudScriptModels.ExecuteCloudScriptResult obj)
            {
                var response = DeserialiseResponseToPlayfabObject<PlayFab.GroupsModels.ListMembershipResponse>(obj);

                if (response == null)
                {
                    getOpenGamesResponse.status.SetError(FailureReason.InternalError);
                    return;
                }

                getOpenGamesResponse.returnData = response.Groups;

                getOpenGamesResponse.status.SetSucess();
            }

            yield return new WaitUntil(() => getOpenGamesResponse.status.Complete);

        }

        public IEnumerator ValidateIfBelowGameLimit(CallResponse<bool> validationResponse)
        {
            int numberOfMemberGames;

            // gather member games if not already cached
            if (cachedMemberGames == null)
            {
                CallResponse response = new CallResponse();

                var gatherCallbacks = new APIOperationCallbacks<List<NetworkGame>>(
                    onSucess: (_) => response.status.SetSucess()
                    , onfailure: (FailureReason) => response.status.SetError(FailureReason));

                yield return StartCoroutine(GetAllMyGames(gatherCallbacks));

                if (response.status.Error)
                {
                    switch (response.status.ErrorData)
                    {
                        case FailureReason.PlayerIsMemberOfNoGames:
                            // do nothing
                            break;
                        default:
                            validationResponse.status.SetError(response.status.ErrorData);
                            yield break;
                    }
                }

            }

            numberOfMemberGames = cachedMemberGames.Count;

            // some upper limit on number of mambered games
            if (numberOfMemberGames < maximumActiveGames)
            {
                validationResponse.returnData = true;
            }
            else
            {
                validationResponse.returnData = false;
            }

            validationResponse.status.SetSucess();

        }

        public IEnumerator JoinOpenGameGroup(GroupWithRoles groupToJoin, APIOperationCallbacks<NetworkGame> resultsCallback)
        {
            // validate we are allowed to join a new game
            {
                var validatIfCanJoinNewGame = new CallResponse<bool>();

                yield return StartCoroutine(ValidateIfBelowGameLimit(validatIfCanJoinNewGame));

                if (validatIfCanJoinNewGame.status.Error)
                {
                    resultsCallback.OnFailure(validatIfCanJoinNewGame.status.ErrorData);
                    yield break;
                }

                if (validatIfCanJoinNewGame.returnData == false) // cannot join new game
                {
                    resultsCallback.OnFailure(FailureReason.TooManyActiveGames);
                    yield break;
                }
            }


            // could validate game is actually open and we arn't already members... but we can just try and learn this from it failing


            // join the game
            {
                var joinGameResponse = new CallResponse();

                yield return StartCoroutine(JoinGroup(groupToJoin.Group, joinGameResponse));

                if (joinGameResponse.status.Error)
                {
                    resultsCallback.OnFailure(joinGameResponse.status.ErrorData);
                    yield break;
                }
            }

            // get the metadata
            NetworkGame.NetworkGameMetadata metaData;
            {
                var getMetadataRespone = new CallResponse<NetworkGame.NetworkGameMetadata>();

                yield return StartCoroutine(GetGroupMetaData(groupToJoin.Group, getMetadataRespone));

                if (getMetadataRespone.status.Error)
                {
                    resultsCallback.OnFailure(getMetadataRespone.status.ErrorData);
                    yield break;
                }

                metaData = getMetadataRespone.returnData;
            }

            // build and reurn the game

            NetworkGame netGame = new NetworkGame(groupToJoin, metaData);

            cachedMemberGames.Add(netGame);
            
            resultsCallback.OnSucess(netGame);


        }

        private IEnumerator JoinGroup(EntityKey group, CallResponse joinGroupResponse)
        {
            var request = new PlayFab.CloudScriptModels.ExecuteEntityCloudScriptRequest
            {
                FunctionName = "JoinGroup",
                FunctionParameter = new
                {
                    Group = group,
                    Entity = ClientEntity
                }
            };


            PlayFabCloudScriptAPI.ExecuteEntityCloudScript(
               request: request,
               resultCallback: (PlayFab.CloudScriptModels.ExecuteCloudScriptResult obj) => { JoinGroupSucess(obj); },
               errorCallback: (PlayFabError obj) => ScriptExecutedfailure(obj, joinGroupResponse)
               );


            void JoinGroupSucess(PlayFab.CloudScriptModels.ExecuteCloudScriptResult obj)
            {

                if (obj.Error != null)
                {
                    joinGroupResponse.status.SetError(FailureReason.InternalError);
                    return;
                }
               
                joinGroupResponse.status.SetSucess();
            }

            yield return new WaitUntil(() => joinGroupResponse.status.Complete);

        }


        public IEnumerator CreateGame(APIOperationCallbacks<NetworkGame> resultsCallback)
        {
            // validate we are allowed to join a new game
            {
                var validatIfCanJoinNewGame = new CallResponse<bool>();

                yield return StartCoroutine(ValidateIfBelowGameLimit(validatIfCanJoinNewGame));

                if (validatIfCanJoinNewGame.status.Error)
                {
                    resultsCallback.OnFailure(validatIfCanJoinNewGame.status.ErrorData);
                    yield break;
                }

                if (validatIfCanJoinNewGame.returnData == false) // cannot join new game
                {
                    resultsCallback.OnFailure(FailureReason.TooManyActiveGames);
                    yield break;
                }
            }

            // previous call has side effect of ensuring "cachedMemberGames" is not null
            if(cachedMemberGames == null)
            {
                resultsCallback.OnFailure(FailureReason.UnknownError);
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

            cachedMemberGames.Add(networkGame);

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