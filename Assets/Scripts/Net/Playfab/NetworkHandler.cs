using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab.GroupsModels;
using UnityEngine;


namespace NetSystem
{


    [RequireComponent(typeof(MatchMaker))]
    public class NetworkHandler : MonoBehaviour
    {
        [SerializeField] PlayerClient playerClient;
        MatchMaker matchMaker;

       
        

        private void Awake()
        {
            matchMaker = GetComponent<MatchMaker>();
            matchMaker.Init(playerClient);
        }

        public void GatherMemberGames()
        {
            var callbacks = new APIOperationCallbacks<List<NetworkGame>>(onSucess: OnGamesGatheredSucess, onfailure: OnGamesGatherFailure);
            StartCoroutine(matchMaker.GetAllMyGames(callbacks));
        }

        private void OnGamesGatheredSucess(List<NetworkGame> games)
        {
            Debug.Log("Games Gathered");

            if(games.Count == 0)
            {
                Debug.Log("No active games");
                return;
            }

            foreach (var game in games)
            {
                Debug.Log(game.GroupName);
              //  Debug.Log($"Open: {game.MetaData.}");

            }
        }

        private void OnGamesGatherFailure(FailureReason reason)
        {
            throw new NotImplementedException();
        }

        public void EnterNewGame()
        {
            var callbacks = new APIOperationCallbacks<List<PlayFab.GroupsModels.GroupWithRoles>>(onSucess: OnGotOpenGameGroupsSucess, onfailure: OnGetOpenGamefailure);
            StartCoroutine(matchMaker.GetOpenGameGroups(callbacks));
        }

        private void OnGotOpenGameGroupsSucess(List<PlayFab.GroupsModels.GroupWithRoles> groups)
        {
            var groupToJoin = SelectGroupToJoin(groups);
            var callbacks = new APIOperationCallbacks<NetworkGame>(onSucess: OnJoinedGameSucess, onfailure: OnJoinedGameSucess);
            StartCoroutine(matchMaker.GetOpenGameGroups(callbacks));
        }

        private object SelectGroupToJoin(List<GroupWithRoles> groups)
        {
            return groups[UnityEngine.Random.Range(0, groups.Count)];
        }

        private void OnGetOpenGamefailure(FailureReason reason)
        {
            switch (reason)
            {
               
                case FailureReason.PlayFabError:
                    UnexpectedPlayfabError();
                    return;
                case FailureReason.TooManyActiveGames:
                    PlayerHasTooManyActiveGames();
                    return;
                case FailureReason.NoOpenGamesAvailable: // this is not a problem, just start a new game
                    StartNewGame();
                    return;
                default:
                    UnknownError();
                    return;
            }
            
        }

        private void StartNewGame()
        {
            throw new NotImplementedException();
        }

        private void PlayerHasTooManyActiveGames()
        {
            throw new NotImplementedException();
        }

        private void UnexpectedPlayfabError()
        {
            throw new NotImplementedException();
        }

        private void UnknownError()
        {
            throw new NotImplementedException();
        }
    }
   

   




}