using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;

using System;
using TestGuildController;
using PlayFab.CloudScriptModels;

//using PlayFab.CloudScriptModels;

namespace Net
{
    public class RoomTest : MonoBehaviour
    {
        [SerializeField] NetPlayerTest netPlayer;

        GuildTestController groupController = new GuildTestController();

        private void Start()
        {
          //  CreateAndListGroup();

        }


        public void CreateGroupOnServer()
        {
            // todo handle errors in this function


            var request = new PlayFab.CloudScriptModels.ExecuteEntityCloudScriptRequest
            {
                FunctionName = "StartNewGameGroup",
                FunctionParameter = new
                {
                    Entity = netPlayer.entityKey,
                    GroupName = GetEntityUniqueGroupName() + "." + DateTime.Now.Ticks.ToString()
                }
            };

            PlayFabCloudScriptAPI.ExecuteEntityCloudScript(request, CreateGroupSucess, ScriptExecutedfailure);
        }

        private string GetEntityUniqueGroupName()
        {
            // return SaveSystemInternal.SaveDataUtility.ComputeHashToAsciiString(netPlayer.entityKey.Id);
            return netPlayer.entityKey.Id;
        }

        private void ScriptExecutedfailure(PlayFabError obj)
        {
            Debug.LogError($"Attempted cloud execution failed");
            Debug.LogError(obj.GenerateErrorReport());
        }

        private void CreateGroupSucess(PlayFab.CloudScriptModels.ExecuteCloudScriptResult obj)
        {
            PlayFab.GroupsModels.CreateGroupResponse result = GetAndLogOutResponse<PlayFab.GroupsModels.CreateGroupResponse>(obj);

            Debug.Log($"Group: {result.GroupName}, ID: {result.Group.Id}, admin: {result.AdminRoleId}");
        }

        bool listing = false;
        public void ListGroups()
        {
            if (!listing)
            {
                StartCoroutine(ListGroupsCoroutine());
            }

        }

        private IEnumerator ListGroupsCoroutine()
        {
            listing = true;
            var request = new PlayFab.CloudScriptModels.ExecuteEntityCloudScriptRequest
            {
                FunctionName = "ListGroups"
            };


            (bool complete, PlayFab.GroupsModels.GroupWithRoles group, bool error) ListGroupResult = (complete: false, group: null, error: true); // if never initilased past this point, assume error

            PlayFabCloudScriptAPI.ExecuteEntityCloudScript(
                request,
                (PlayFab.CloudScriptModels.ExecuteCloudScriptResult obj) => { 
                    (ListGroupResult.group, ListGroupResult.error) = ListGroupsSucess(obj); 
                    ListGroupResult.complete = true; 
                },
                (PlayFabError obj) => { ScriptExecutedfailure(obj); (ListGroupResult.complete, ListGroupResult.error) = (true,true); }
                );

            yield return new WaitUntil(()=>ListGroupResult.complete);

            Debug.Log("Task complete");

            if (ListGroupResult.error)
            {
                Debug.Log("There was an error");
                yield break;
            }


            if (ListGroupResult.group == null)
            {
                Debug.Log("No groups, making new one");
                CreateGroupOnServer();
            }
            else
            {
                Debug.Log($"Group: {ListGroupResult.group.GroupName} chosen... attempting to join");
                JoinGroup(ListGroupResult.group.Group);
            }

            listing = false; // todo this should await the above functions
        }

        private (PlayFab.GroupsModels.GroupWithRoles, bool error) ListGroupsSucess(PlayFab.CloudScriptModels.ExecuteCloudScriptResult obj)
        {
            // todo make this robust


            PlayFab.GroupsModels.ListMembershipResponse response = GetAndLogOutResponse<PlayFab.GroupsModels.ListMembershipResponse>(obj);

           
            Debug.Log($"Groups: {response.Groups}");




            //foreach (PlayFab.GroupsModels.GroupWithRoles group in response.Groups)
            //{
            //    Debug.Log($"Group: {group.GroupName}");
            //    GetGroupMembers(group.Group);
            //}

            int count = response.Groups.Count;
            if (count > 0)
            {
                Debug.Log("Returning random group");
                return (response.Groups[UnityEngine.Random.Range(0, count)], error: false);
            }

            Debug.Log("Returning null");
            return (null, error: false);


        }

      

        public void GetGroupMembers(PlayFab.GroupsModels.EntityKey group)
        {
            // todo handle errors in this function
           
            var request = new PlayFab.CloudScriptModels.ExecuteEntityCloudScriptRequest
            {
                FunctionName = "ReturnGroupMembers",
                FunctionParameter = new
                {
                    Group = group
                }
            };

            PlayFabCloudScriptAPI.ExecuteEntityCloudScript(request, GetGroupMembersSucess, ScriptExecutedfailure);
        }

        private void GetGroupMembersSucess(PlayFab.CloudScriptModels.ExecuteCloudScriptResult obj)
        {
            var groupMembers = GetAndLogOutResponse<PlayFab.GroupsModels.ListGroupMembersResponse>(obj);

            Debug.Log($"Group contains ({groupMembers.Members.Count}) members");

            foreach (var member in groupMembers.Members)
            {
                Debug.Log($"Role: {member.RoleName}");
                foreach (var entitiy in member.Members)
                {
                    Debug.Log($"key {entitiy.Key.Id}");

                    if (entitiy.Lineage == null) continue;

                    foreach (var lineage in entitiy.Lineage)
                    {
                        Debug.Log($"Lineage {lineage.Key}: {lineage.Value.Id}");
                    }
                }
            }
        }

        private static T GetAndLogOutResponse<T>(PlayFab.CloudScriptModels.ExecuteCloudScriptResult obj) where T: PlayFab.SharedModels.PlayFabResultCommon
        {
            // todo this should report errors

            Debug.Log("Sucess?");

            object objResult = obj.FunctionResult;

            string stringValue = objResult.ToString();

            //Debug.Log(stringValue);

            T response = JsonUtility.FromJson<T>(stringValue);

            Debug.Log($"Response: {response}");

            return response;
        }



        public void JoinGroup(PlayFab.GroupsModels.EntityKey group)
        {
            // todo handle errors in this function

            var request = new PlayFab.CloudScriptModels.ExecuteEntityCloudScriptRequest
            {
                FunctionName = "JoinGroup",
                FunctionParameter = new
                {
                    Group = group,
                    Entity = netPlayer.entityKey
                }
            };
            Debug.Log($"Joining group {group.Id}");
            PlayFabCloudScriptAPI.ExecuteEntityCloudScript(request, JoinGroupSucess, ScriptExecutedfailure);
        }

        private void JoinGroupSucess(ExecuteCloudScriptResult obj)
        {
            if (obj.Error != null)
            {
                ScriptExecutedfailure(obj.Error);
                return;
            }
            Debug.Log("Sucessfully joined group");
        }

        private void ScriptExecutedfailure(ScriptExecutionError error)
        {
            Debug.LogError(error.Error + " "+ error.Message + " " + error.StackTrace);
        }

        //public void foo()
        //{
        //    var request = new PlayFab.ClientModels.ExecuteCloudScriptRequest
        //    {
        //        FunctionName = "ListGroups"
        //    };
        //    PlayFab.PlayFabClientAPI.ExecuteCloudScript(request, ListGroupsSucess, ScriptExecutedfailure);
        //}
        //private void ListGroupsSucess(PlayFab.ClientModels.ExecuteCloudScriptResult obj)
        //{
        //    //Debug.Log("Sucess?");
        //    //Debug.Log(obj.FunctionResult?.ToString());

        //    //var result = (ListGroupMembersResponse)obj.FunctionResult;

        //    //foreach (EntityMemberRole item in result.Members)
        //    //{
        //    //    Debug.Log(item.ToString());
        //    //}
        //}



        //private void ScriptExecutedSucess(PlayFab.ClientModels.ExecuteCloudScriptResult obj)
        //{
        //    Debug.Log("Sucess?");
        //    Debug.Log(obj.FunctionResult?.ToString());

        //}

        //public void CreateAndListGroup()
        //{
        //    var entity = new EntityKey() { Id = netPlayer.entityKey.Id, Type = netPlayer.entityKey.Type }; // this may be wrong?
        //    groupController.CreateGroup("Test", entity);

        //    groupController.ListGroups(entity);
        //    foreach (var item in groupController.EntityGroupPairs)
        //    {
        //        if (item.Key != entity.Id)
        //        {
        //            Debug.LogError("Somone else is in this group");
        //            continue;
        //        }

        //        Debug.Log($"{item.Value}");

        //    }
        //}
    }
}



//https://docs.microsoft.com/en-us/gaming/playfab/features/social/groups/quickstart
namespace TestGuildController
{
    /// <summary>
    /// Assumptions for this controller:
    /// + Entities can be in multiple groups
    ///   - This is game specific, many games would only allow 1 group, meaning you'd have to perform some additional checks to validate this.
    /// </summary>
    [Serializable]
    public class GuildTestController
    {
        // A local cache of some bits of PlayFab data
        // This cache pretty much only serves this example , and assumes that entities are uniquely identifiable by EntityId alone, which isn't technically true. Your data cache will have to be better.
        public readonly HashSet<KeyValuePair<string, string>> EntityGroupPairs = new HashSet<KeyValuePair<string, string>>();
        public readonly Dictionary<string, string> GroupNameById = new Dictionary<string, string>();

        public static PlayFab.GroupsModels.EntityKey EntityKeyMaker(string entityId)
        {
            return new PlayFab.GroupsModels.EntityKey { Id = entityId };
        }

        private void OnSharedError(PlayFab.PlayFabError error)
        {
            Debug.LogError(error.GenerateErrorReport());
        }

        public void ListGroups(PlayFab.GroupsModels.EntityKey entityKey)
        {
            var request = new PlayFab.GroupsModels.ListMembershipRequest { Entity = entityKey };
            PlayFabGroupsAPI.ListMembership(request, OnListGroups, OnSharedError);
        }
        private void OnListGroups(PlayFab.GroupsModels.ListMembershipResponse response)
        {
            var prevRequest = (PlayFab.GroupsModels.ListMembershipRequest)response.Request;
            foreach (var pair in response.Groups)
            {
                GroupNameById[pair.Group.Id] = pair.GroupName;
                EntityGroupPairs.Add(new KeyValuePair<string, string>(prevRequest.Entity.Id, pair.Group.Id));
            }
        }

        public void CreateGroup(string groupName, PlayFab.GroupsModels.EntityKey entityKey)
        {
            // A player-controlled entity creates a new group
            var request = new PlayFab.GroupsModels.CreateGroupRequest { GroupName = groupName, Entity = entityKey };
            PlayFabGroupsAPI.CreateGroup(request, OnCreateGroup, OnSharedError);
        }
        private void OnCreateGroup(PlayFab.GroupsModels.CreateGroupResponse response)
        {
            Debug.Log("Group Created: " + response.GroupName + " - " + response.Group.Id);

            var prevRequest = (PlayFab.GroupsModels.CreateGroupRequest)response.Request;
            EntityGroupPairs.Add(new KeyValuePair<string, string>(prevRequest.Entity.Id, response.Group.Id));
            GroupNameById[response.Group.Id] = response.GroupName;
        }
        public void DeleteGroup(string groupId)
        {
            // A title, or player-controlled entity with authority to do so, decides to destroy an existing group
            var request = new PlayFab.GroupsModels.DeleteGroupRequest { Group = EntityKeyMaker(groupId) };
            PlayFabGroupsAPI.DeleteGroup(request, OnDeleteGroup, OnSharedError);
        }
        private void OnDeleteGroup(PlayFab.GroupsModels.EmptyResponse response)
        {
            var prevRequest = (PlayFab.GroupsModels.DeleteGroupRequest)response.Request;
            Debug.Log("Group Deleted: " + prevRequest.Group.Id);

            var temp = new HashSet<KeyValuePair<string, string>>();
            foreach (var each in EntityGroupPairs)
                if (each.Value != prevRequest.Group.Id)
                    temp.Add(each);
            EntityGroupPairs.IntersectWith(temp);
            GroupNameById.Remove(prevRequest.Group.Id);
        }

        public void InviteToGroup(string groupId, PlayFab.GroupsModels.EntityKey entityKey)
        {
            // A player-controlled entity invites another player-controlled entity to an existing group
            var request = new PlayFab.GroupsModels.InviteToGroupRequest { Group = EntityKeyMaker(groupId), Entity = entityKey };
            PlayFabGroupsAPI.InviteToGroup(request, OnInvite, OnSharedError);
        }
        public void OnInvite(PlayFab.GroupsModels.InviteToGroupResponse response)
        {
            var prevRequest = (PlayFab.GroupsModels.InviteToGroupRequest)response.Request;

            // Presumably, this would be part of a separate process where the recipient reviews and accepts the request
            var request = new PlayFab.GroupsModels.AcceptGroupInvitationRequest { Group = EntityKeyMaker(prevRequest.Group.Id), Entity = prevRequest.Entity };
            PlayFabGroupsAPI.AcceptGroupInvitation(request, OnAcceptInvite, OnSharedError);
        }
        public void OnAcceptInvite(PlayFab.GroupsModels.EmptyResponse response)
        {
            var prevRequest = (PlayFab.GroupsModels.AcceptGroupInvitationRequest)response.Request;
            Debug.Log("Entity Added to Group: " + prevRequest.Entity.Id + " to " + prevRequest.Group.Id);
            EntityGroupPairs.Add(new KeyValuePair<string, string>(prevRequest.Entity.Id, prevRequest.Group.Id));
        }

        public void ApplyToGroup(string groupId, PlayFab.GroupsModels.EntityKey entityKey)
        {
            // A player-controlled entity applies to join an existing group (of which they are not already a member)
            var request = new PlayFab.GroupsModels.ApplyToGroupRequest { Group = EntityKeyMaker(groupId), Entity = entityKey };
            PlayFabGroupsAPI.ApplyToGroup(request, OnApply, OnSharedError);
        }
        public void OnApply(PlayFab.GroupsModels.ApplyToGroupResponse response)
        {
            var prevRequest = (PlayFab.GroupsModels.ApplyToGroupRequest)response.Request;

            // Presumably, this would be part of a separate process where the recipient reviews and accepts the request
            var request = new PlayFab.GroupsModels.AcceptGroupApplicationRequest { Group = prevRequest.Group, Entity = prevRequest.Entity };
            PlayFabGroupsAPI.AcceptGroupApplication(request, OnAcceptApplication, OnSharedError);
        }
        public void OnAcceptApplication(PlayFab.GroupsModels.EmptyResponse response)
        {
            var prevRequest = (PlayFab.GroupsModels.AcceptGroupApplicationRequest)response.Request;
            Debug.Log("Entity Added to Group: " + prevRequest.Entity.Id + " to " + prevRequest.Group.Id);
        }
        public void KickMember(string groupId, PlayFab.GroupsModels.EntityKey entityKey)
        {
            var request = new PlayFab.GroupsModels.RemoveMembersRequest { Group = EntityKeyMaker(groupId), Members = new List<PlayFab.GroupsModels.EntityKey> { entityKey } };
            PlayFabGroupsAPI.RemoveMembers(request, OnKickMembers, OnSharedError);
        }
        private void OnKickMembers(PlayFab.GroupsModels.EmptyResponse response)
        {
            var prevRequest = (PlayFab.GroupsModels.RemoveMembersRequest)response.Request;

            Debug.Log("Entity kicked from Group: " + prevRequest.Members[0].Id + " to " + prevRequest.Group.Id);
            EntityGroupPairs.Remove(new KeyValuePair<string, string>(prevRequest.Members[0].Id, prevRequest.Group.Id));
        }
    }
}