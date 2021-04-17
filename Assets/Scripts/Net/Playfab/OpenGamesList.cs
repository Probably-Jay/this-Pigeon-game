using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

// Jay Easter

namespace NetSystem
{
    using System;
    using PlayFab;
    using PlayFab.GroupsModels;
    public class OpenGamesList : NetComponent
    {

        /// <summary>
        /// Gathers all open games on the server, excluding open games started by this client. This data is not cached as it is highly volatile so calls to this function should be infrequent.
        /// <para/> Upon completion will invoke one of the following callbacks :
        /// <para><see cref="APIOperationCallbacks{List{PlayFab.GroupsModels.GroupWithRoles}}.OnSucess"/>: The open games were sucessfully gathered</para>
        /// <para><see cref="APIOperationCallbacks{List{PlayFab.GroupsModels.GroupWithRoles}}.OnFailure"/>: The call failed due to a networking error, or for one of the following reasons (retuned in callback): 
        ///     <list type="bullet">
        ///         <item>
        ///         <term><see cref="FailureReason.NoOpenGamesAvailable"/></term>
        ///         <description>There are no open games on the server that the client is not already a member of</description>
        ///         </item> 
        ///     </list>
        /// </para>
        /// </summary>
        /// <param name="resultsCallback">Callbakcs for the sucess or failure of this action</param>
        public IEnumerator GetOpenGameGroups(APIOperationCallbacks<List<PlayFab.GroupsModels.GroupWithRoles>> resultsCallback)
        {

            // gather open games
            List<GroupWithRoles> openGroups;
            {
                var getOpenGamesResponse = new CallResponse<List<PlayFab.GroupsModels.GroupWithRoles>>();

                yield return StartCoroutine(GetOpenGamesFromServer(getOpenGamesResponse));

                if (getOpenGamesResponse.status.Error)
                {
                    resultsCallback.OnFailure(getOpenGamesResponse.status.ErrorData);
                    yield break;
                }

                // do not cache this value
                openGroups = getOpenGamesResponse.returnData;
            }

            // if there are no open games (this is checked again before then end of this function, this is an optimisation)
            if (openGroups.Count == 0)
            {
                resultsCallback.OnFailure(FailureReason.NoOpenGamesAvailable);
                yield break;
            }


            // remove games we are in
            ReadOnlyCollection<NetworkGame> cachedMemberGames;
            {
                // gather member games
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


                // find matches
                List<GroupWithRoles> toRemove = new List<GroupWithRoles>();
                foreach (var memberGame in cachedMemberGames)
                {
                    foreach (var openGame in openGroups)
                    {
                        if (memberGame.GroupEntityKey.Id == openGame.Group.Id)
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


            // we might have removed all the games, check this again
            if (openGroups.Count == 0)
            {
                resultsCallback.OnFailure(FailureReason.NoOpenGamesAvailable);
                yield break;
            }

            resultsCallback.OnSucess(openGroups);

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

    }
}