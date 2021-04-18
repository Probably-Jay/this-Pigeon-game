using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;


namespace NetSystem
{
    using System;
    using PlayFab;
    using PlayFab.GroupsModels;


    public class ActiveGame : NetComponent
    {
        private NetworkGame currentNetworkGame = null;

        public bool IsInGame => CurrentNetworkGame != null;
        public NetworkGame CurrentNetworkGame
        {
            get => currentNetworkGame; 
            private set
            {
                currentNetworkGame = value;
                Debug.Log($"Entered game {currentNetworkGame.GroupName}");
            }
        }

        public void SetActiveNetorkGame(NetworkGame game) => CurrentNetworkGame = game;


        public IEnumerator ResumeMemberGame(NetworkGame gameToResume, APIOperationCallbacks<NetworkGame> resultsCallback)
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

            foreach (var game in cachedMemberGames)
            {
                if(game.GroupEntityKey.Id == gameToResume.GroupEntityKey.Id)
                {
                    resultsCallback.OnSucess(game);
                    yield break;
                }
            }

            resultsCallback.OnFailure(FailureReason.PlayerIsNotAMemberOfThisGame);
            yield break;
            

           
            
        }

        /// <summary>
        /// Joins an open game on the server that was started by another player. This removes the title entity from the game and sets the game to "closed".
        /// <para/> Upon completion will invoke one of the following callbacks :
        /// <para><see cref="APIOperationCallbacks{NetworkGame}.OnSucess"/>: The game was sucessfuly entered, its metadata was gathered, and a new <see cref="NetworkGame"/> object has been stored</para>
        /// <para><see cref="APIOperationCallbacks{NetworkGame}.OnFailure"/>: The call failed due to a networking error, or for one of the following reasons (retuned in callback): 
        ///     <list type="bullet">
        ///         <item>
        ///         <term><see cref="FailureReason.TooManyActiveGames"/></term>
        ///         <description>The user is a member of too many games and is not allowed to enter a new one</description>
        ///         </item> 
        ///     </list>
        /// </para>
        /// </summary>
        /// <param name="resultsCallback">Callbakcs for the sucess or failure of this action</param>
        public IEnumerator JoinOpenGameGroup(GroupWithRoles groupToJoin, APIOperationCallbacks<NetworkGame> resultsCallback)
        {

            // validate we are allowed to join a new game
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

                //validate below limit
                if (!ValidateIfBelowGameLimit(cachedMemberGames))
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

            NetworkHandler.Instance.RemoteMemberGamesList.Add(netGame);

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


        public void ExitGame()
        {
            currentNetworkGame = null;
        }
    }
}