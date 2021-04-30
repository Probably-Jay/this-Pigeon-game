using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace NetSystem
{
    using System;
    using PlayFab;
    using PlayFab.GroupsModels;

    public class MemberGamesList : NetComponent
    {
        private DateTime? LastCachedTime { get; set; } = null;
        private bool HasRemoteGamesCached => memberGames != null;
        private ReadOnlyCollection<NetworkGame> CachedMemberGames
        {
            get => new ReadOnlyCollection<NetworkGame>(memberGames); 
        }
        private List<NetworkGame> MemberGames
        {
            set
            {
                memberGames = value;
                LastCachedTime = DateTime.Now;
            }
        }

    

        private List<NetworkGame> memberGames = null;


        public void Add(NetworkGame game)
        {
            if (HasRemoteGamesCached)
            {
                memberGames.Add(game);
            }
            else
            {
              //  RefreshList();
              
            }

        }

        public void ClearCache() => memberGames = null;

        //private void RefreshList()
        //{
        //    GetAllMyGames(new APIOperationCallbacks<ReadOnlyCollection<NetworkGame>>(
        //        onSucess: (_) => { },
        //        onfailure: (_) => throw new Exception(_)
        //        )) ;
        //}


        /// <summary>
        /// Gathers all games this client is a member of, including unstarted open games. This data is cached.
        /// <para/> Upon completion will invoke one of the following callbacks :
        /// <para><see cref="APIOperationCallbacks{List{PlayFab.GroupsModels.GroupWithRoles}}.OnSucess"/>: The cache was hit, or the open games were sucessfully gathered along with their metadata and cached</para>
        /// <para><see cref="APIOperationCallbacks{List{PlayFab.GroupsModels.GroupWithRoles}}.OnFailure"/>: The call failed due to a networking error, or for one of the following reasons (retuned in callback): 
        ///     <list type="bullet">
        ///         <item>
        ///         <term><see cref="FailureReason.PlayerIsMemberOfNoGames"/></term>
        ///         <description>The player is not a member of any games. This is returned as an explicit error case to be handled</description>
        ///         </item> 
        ///     </list>
        /// </para>
        /// </summary>
        /// <param name="resultsCallback">Callbakcs for the sucess or failure of this action</param>
        public IEnumerator GetAllMyGames(APIOperationCallbacks<ReadOnlyCollection<NetworkGame>> resultCallbacks)
        {
            // if we have the cache, return it
            if (HasRemoteGamesCached)
            {
                Debug.Log("Cached member games found");

                if (CachedMemberGames.Count == 0) // should still return no-games error here
                {
                    MemberGames = new List<NetworkGame>();
                    resultCallbacks.OnFailure(FailureReason.PlayerIsMemberOfNoGames);
                    yield break;
                }

                resultCallbacks.OnSucess(CachedMemberGames);
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

            // if we aren't in any groups, return empty list with error
            if (groupsWeAreIn.Count == 0)
            {
                MemberGames = new List<NetworkGame>();
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

            MemberGames = netGames;

            resultCallbacks.OnSucess(CachedMemberGames);

        }


        private IEnumerator GetGroupsClientIsMemberOf(CallResponse<List<PlayFab.GroupsModels.GroupWithRoles>> callResponse)
        {

            var request = new PlayFab.CloudScriptModels.ExecuteEntityCloudScriptRequest
            {
                FunctionName = "ListMyGroups",
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

                callResponse.returnData = response.Groups;

                callResponse.status.SetSucess();
            }

            yield return new WaitUntil(() => { Debug.Log("Loading"); return callResponse.status.Complete; });

        }


        



    }
}