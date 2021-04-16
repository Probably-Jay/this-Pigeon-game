using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace NetSystem
{
    using System;
    using PlayFab;
    using PlayFab.GroupsModels;
    public class OpenGamesList : NetComponent
    {


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

            // if there are no open games
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


            // if there are no open games
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