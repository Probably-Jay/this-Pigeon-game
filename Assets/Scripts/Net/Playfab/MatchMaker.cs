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

            if (cachedMemberGames != null)
            {
                Debug.Log("Cached member games found, returning");
                resultCallbacks.OnSucess(cachedMemberGames);
                yield break;
            }

            // get the groups we are members of (including open unstarted games)
            var listGroupResponse = new CallResponse<List<PlayFab.GroupsModels.GroupWithRoles>>();

            yield return StartCoroutine(GetGroupsClientIsMemberOf(listGroupResponse));

            if (listGroupResponse.status.Error)
            {
                resultCallbacks.OnFailure(listGroupResponse.status.ErrorData);
                yield break;
            }

            // if we aren't in any groups, return empty list
            if (listGroupResponse.returnData.Count == 0)
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
                if (response.status.Error)
                {
                    resultCallbacks.OnFailure(response.status.ErrorData);
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


        public IEnumerator ValidateIfBelowGameLimit(APIOperationCallbacks resultsCallbacks)
        {
            // gather member games if not already cached
            if (cachedMemberGames == null)
            {
                CallResponse response = new CallResponse();

                var gatherCallbacks = new APIOperationCallbacks<List<NetworkGame>>(
                    onSucess: (_) => response.status.SetSucess()
                    , onfailure: (FailureReason) => response.status.SetError(FailureReason));

                yield return StartCoroutine(GetAllMyGames(gatherCallbacks));

                if (response.status.Error || cachedMemberGames == null)
                {
                    resultsCallbacks.OnFailure(response.status.ErrorData);
                    yield break;
                }
            }

            // some upper limit on number of mambered games
            if (cachedMemberGames.Count >= maximumActiveGames)
            {
                resultsCallbacks.OnFailure(FailureReason.TooManyActiveGames);
                yield break;
            }

            resultsCallbacks.OnSucess();

        }

        public IEnumerator GetOpenGameGroups(APIOperationCallbacks<List<PlayFab.GroupsModels.GroupWithRoles>> resultsCallbacks)
        {
            
            // gather open games
            var getOpenGamesResponse = new CallResponse<List<PlayFab.GroupsModels.GroupWithRoles>>();

            yield return StartCoroutine(GetOpenGamesFromServer(getOpenGamesResponse));

            if (getOpenGamesResponse.status.Error)
            {
                resultsCallbacks.OnFailure(getOpenGamesResponse.status.ErrorData);
                yield break;
            }

            // if there is an open game
            if(getOpenGamesResponse.returnData.Count == 0)
            {
                resultsCallbacks.OnFailure(FailureReason.NoOpenGamesAvailable);
                yield break;
            }

            resultsCallbacks.OnSucess(getOpenGamesResponse.returnData);

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



        public IEnumerator JoinOpenGameGroup(GroupWithRoles groupToJoin, APIOperationCallbacks<NetworkGame> resultsCallback)
        {
            // join the game
            var joinGameResponse = new CallResponse();

            yield return StartCoroutine(JoinGroup(groupToJoin.Group, joinGameResponse));

            if (joinGameResponse.status.Error)
            {
                resultsCallback.OnFailure(joinGameResponse.status.ErrorData);
                yield break;
            }

            // get the metadata
            var getMetadataRespone = new CallResponse<NetworkGame.NetworkGameMetadata>();

            yield return StartCoroutine(GetGroupMetaData(groupToJoin.Group, getMetadataRespone));

            if (getMetadataRespone.status.Error)
            {
                resultsCallback.OnFailure(joinGameResponse.status.ErrorData);
                yield break;
            }


            NetworkGame netGame = new NetworkGame(groupToJoin, getMetadataRespone.returnData);

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



    }
}