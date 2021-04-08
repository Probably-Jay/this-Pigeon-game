using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.GroupsModels;
using System;
using TestGuildController;
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
            var request = new PlayFab.CloudScriptModels.ExecuteEntityCloudScriptRequest
            {
                FunctionName = "StartNewGameGroup",
                FunctionParameter = new
                {
                    Entity = netPlayer.entityKey,
                    GroupName = EntityUniqueGroupName() + "." + DateTime.Now.Ticks.ToString()
                }


            };

            PlayFabCloudScriptAPI.ExecuteEntityCloudScript(request, CreateGroupSucess, ScriptExecutedfailure);
        }

        private string EntityUniqueGroupName()
        {
            return SaveSystemInternal.SaveDataUtility.ComputeHashToString(netPlayer.entityKey.Id);
        }

        private void ScriptExecutedfailure(PlayFabError obj)
        {
            Debug.LogError($"Execution Failed");
            Debug.LogError(obj.GenerateErrorReport());



        }
        private void CreateGroupSucess(PlayFab.CloudScriptModels.ExecuteCloudScriptResult obj)
        {
            Debug.Log("Sucess?");
            //Debug.Log(obj.FunctionResult?.ToString());

            var objResult = obj.FunctionResult;

            Debug.Log(objResult);

            string stringValue = obj.FunctionResult.ToString();
            Debug.Log(stringValue);

            // var result = (ListGroupMembersResponse)obj.FunctionResult;
            CreateGroupResponse result = JsonUtility.FromJson<CreateGroupResponse>(stringValue);

            Debug.Log(result);
            Debug.Log($"Group: {result.GroupName}, ID: {result.Group.Id}, admin: {result.AdminRoleId}");

            foreach (var role in result.Roles)
            {
                Debug.Log($"{role.Key}: {role.Value}");
            }


        }

        public void ListGroups()
        {

            var request = new PlayFab.CloudScriptModels.ExecuteEntityCloudScriptRequest
            {
                FunctionName = "ListGroups"
            };


            (GroupWithRoles group, bool error)  gameGroup = (group: null, error: true); // if never initilased past this point, assume error

            PlayFabCloudScriptAPI.ExecuteEntityCloudScript(
                request, 
                (PlayFab.CloudScriptModels.ExecuteCloudScriptResult obj) => { gameGroup = ListGroupsSucess(obj); }, 
                ScriptExecutedfailure);

            if (gameGroup.error)
            {
                return;
            }
           

            if(gameGroup.group == null)
            {
                Debug.Log("No groups, making new one");
                CreateGroupOnServer();
            }
            else
            {
                Debug.Log($"Group: {gameGroup.group.GroupName} chosen... attempting to join");

            }
            
           
        }

        private (GroupWithRoles, bool error) ListGroupsSucess(PlayFab.CloudScriptModels.ExecuteCloudScriptResult obj)
        {
            // todo make this robust


            Debug.Log("Sucess?");

            var objResult = obj.FunctionResult;

            Debug.Log(objResult);

            string stringValue = obj.FunctionResult.ToString();
            Debug.Log(stringValue);

            // var result = (ListGroupMembersResponse)obj.FunctionResult;
            var result = JsonUtility.FromJson<ListMembershipResponse>(stringValue);

            Debug.Log(result);
           // Debug.Log(result.Groups);
            //var result = PlayFab.Json.PlayFabSimpleJson.DeserializeObject<ListGroupMembersResponse>(stringValue);



            foreach (GroupWithRoles group in result.Groups)
            {
                Debug.Log($"Group: {group.GroupName}");
                
            }

            int count = result.Groups.Count;
            if (count > 0)
            {
                return (result.Groups[UnityEngine.Random.Range(0, count)] , error: false);
            }
            else
            {
                return (null, error: false);
            }

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

        public static EntityKey EntityKeyMaker(string entityId)
        {
            return new EntityKey { Id = entityId };
        }

        private void OnSharedError(PlayFab.PlayFabError error)
        {
            Debug.LogError(error.GenerateErrorReport());
        }

        public void ListGroups(EntityKey entityKey)
        {
            var request = new ListMembershipRequest { Entity = entityKey };
            PlayFabGroupsAPI.ListMembership(request, OnListGroups, OnSharedError);
        }
        private void OnListGroups(ListMembershipResponse response)
        {
            var prevRequest = (ListMembershipRequest)response.Request;
            foreach (var pair in response.Groups)
            {
                GroupNameById[pair.Group.Id] = pair.GroupName;
                EntityGroupPairs.Add(new KeyValuePair<string, string>(prevRequest.Entity.Id, pair.Group.Id));
            }
        }

        public void CreateGroup(string groupName, EntityKey entityKey)
        {
            // A player-controlled entity creates a new group
            var request = new CreateGroupRequest { GroupName = groupName, Entity = entityKey };
            PlayFabGroupsAPI.CreateGroup(request, OnCreateGroup, OnSharedError);
        }
        private void OnCreateGroup(CreateGroupResponse response)
        {
            Debug.Log("Group Created: " + response.GroupName + " - " + response.Group.Id);

            var prevRequest = (CreateGroupRequest)response.Request;
            EntityGroupPairs.Add(new KeyValuePair<string, string>(prevRequest.Entity.Id, response.Group.Id));
            GroupNameById[response.Group.Id] = response.GroupName;
        }
        public void DeleteGroup(string groupId)
        {
            // A title, or player-controlled entity with authority to do so, decides to destroy an existing group
            var request = new DeleteGroupRequest { Group = EntityKeyMaker(groupId) };
            PlayFabGroupsAPI.DeleteGroup(request, OnDeleteGroup, OnSharedError);
        }
        private void OnDeleteGroup(EmptyResponse response)
        {
            var prevRequest = (DeleteGroupRequest)response.Request;
            Debug.Log("Group Deleted: " + prevRequest.Group.Id);

            var temp = new HashSet<KeyValuePair<string, string>>();
            foreach (var each in EntityGroupPairs)
                if (each.Value != prevRequest.Group.Id)
                    temp.Add(each);
            EntityGroupPairs.IntersectWith(temp);
            GroupNameById.Remove(prevRequest.Group.Id);
        }

        public void InviteToGroup(string groupId, EntityKey entityKey)
        {
            // A player-controlled entity invites another player-controlled entity to an existing group
            var request = new InviteToGroupRequest { Group = EntityKeyMaker(groupId), Entity = entityKey };
            PlayFabGroupsAPI.InviteToGroup(request, OnInvite, OnSharedError);
        }
        public void OnInvite(InviteToGroupResponse response)
        {
            var prevRequest = (InviteToGroupRequest)response.Request;

            // Presumably, this would be part of a separate process where the recipient reviews and accepts the request
            var request = new AcceptGroupInvitationRequest { Group = EntityKeyMaker(prevRequest.Group.Id), Entity = prevRequest.Entity };
            PlayFabGroupsAPI.AcceptGroupInvitation(request, OnAcceptInvite, OnSharedError);
        }
        public void OnAcceptInvite(EmptyResponse response)
        {
            var prevRequest = (AcceptGroupInvitationRequest)response.Request;
            Debug.Log("Entity Added to Group: " + prevRequest.Entity.Id + " to " + prevRequest.Group.Id);
            EntityGroupPairs.Add(new KeyValuePair<string, string>(prevRequest.Entity.Id, prevRequest.Group.Id));
        }

        public void ApplyToGroup(string groupId, EntityKey entityKey)
        {
            // A player-controlled entity applies to join an existing group (of which they are not already a member)
            var request = new ApplyToGroupRequest { Group = EntityKeyMaker(groupId), Entity = entityKey };
            PlayFabGroupsAPI.ApplyToGroup(request, OnApply, OnSharedError);
        }
        public void OnApply(ApplyToGroupResponse response)
        {
            var prevRequest = (ApplyToGroupRequest)response.Request;

            // Presumably, this would be part of a separate process where the recipient reviews and accepts the request
            var request = new AcceptGroupApplicationRequest { Group = prevRequest.Group, Entity = prevRequest.Entity };
            PlayFabGroupsAPI.AcceptGroupApplication(request, OnAcceptApplication, OnSharedError);
        }
        public void OnAcceptApplication(EmptyResponse response)
        {
            var prevRequest = (AcceptGroupApplicationRequest)response.Request;
            Debug.Log("Entity Added to Group: " + prevRequest.Entity.Id + " to " + prevRequest.Group.Id);
        }
        public void KickMember(string groupId, EntityKey entityKey)
        {
            var request = new RemoveMembersRequest { Group = EntityKeyMaker(groupId), Members = new List<EntityKey> { entityKey } };
            PlayFabGroupsAPI.RemoveMembers(request, OnKickMembers, OnSharedError);
        }
        private void OnKickMembers(EmptyResponse response)
        {
            var prevRequest = (RemoveMembersRequest)response.Request;

            Debug.Log("Entity kicked from Group: " + prevRequest.Members[0].Id + " to " + prevRequest.Group.Id);
            EntityGroupPairs.Remove(new KeyValuePair<string, string>(prevRequest.Members[0].Id, prevRequest.Group.Id));
        }
    }
}