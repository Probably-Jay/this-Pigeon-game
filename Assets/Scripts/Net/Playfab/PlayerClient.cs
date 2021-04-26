using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetSystem
{
    using System;
    using PlayFab;

    public class PlayerClient : MonoBehaviour
    {

        [SerializeField]
        private PlayFab.CloudScriptModels.EntityKey entityKey = new PlayFab.CloudScriptModels.EntityKey();

        /// <summary>
        /// Read-only copy of <see cref="PlayFab.CloudScriptModels.EntityKey"/>. May need to be cast to other modules <see cref="EntityKey"/> type
        /// </summary>
        public PlayFab.CloudScriptModels.EntityKey ClientEntityKey { get => new PlayFab.CloudScriptModels.EntityKey { Id = entityKey.Id, Type = entityKey.Type }; private set => entityKey = value; }
       
        /// <summary>
        /// If account was created this session
        /// </summary>
        public bool NewAccountThisSession { get; private set; }

        /// <summary>
        /// Time the player last logged in
        /// </summary>
        public System.DateTime? LastLoginTime { get; private set; }

        /// <summary>
        /// If the player is logged in
        /// </summary>
        public bool IsLoggedIn { get; private set; } = false;

        public int PlayerGameEnumValue { get; set; }

        

        /// <summary>
        /// Log in using <see cref="SystemInfo.deviceUniqueIdentifier"/> and no password
        /// </summary>
        public void AnonymousLogin(APIOperationCallbacks loginCallbacks, bool debugAltLogin = false)
        {
            StartCoroutine(AnonymousLoginAsyc(loginCallbacks, debugAltLogin));
        }

        private IEnumerator AnonymousLoginAsyc(APIOperationCallbacks loginCallbacks, bool debugAltLogin)
        {
            CallResponse response = new CallResponse();
            switch (Application.platform) // auto detect login method needed
            {
                case RuntimePlatform.WindowsPlayer:
                    WindowsAnonymousLogin(response);
                    break;
                case RuntimePlatform.WindowsEditor:
                    {
                        if (debugAltLogin)
                        {
                            WindowsAnonymousDebugLogin(response);
                            break;
                        }
                        WindowsAnonymousLogin(response);
                    }
                    break;
                case RuntimePlatform.Android:
                    AndroidAnonymousLogin(response);
                    break;
                default:
                    throw new System.Exception($"Sorry you cannot run this application on a {Application.platform}");
            }


            yield return new WaitUntil(() => {Debug.Log("Connecting"); return response.status.Complete; });

            if (response.status.Error)
            {
                loginCallbacks.OnFailure(response.status.ErrorData);
                yield break;
            }

            loginCallbacks.OnSucess();
          
        }


        private void AndroidAnonymousLogin(CallResponse response)
        {
            var androidRequest = new PlayFab.ClientModels.LoginWithAndroidDeviceIDRequest()
            {
                AndroidDeviceId = SystemInfo.deviceUniqueIdentifier,
                CreateAccount = true
            };

           
            PlayFabClientAPI.LoginWithAndroidDeviceID(
                androidRequest,
                (PlayFab.ClientModels.LoginResult obj) => { LoginSucess(obj, response); },
                (PlayFabError obj) => { LoginFailure(obj, response); }
                );

           
        }

        private void WindowsAnonymousLogin(CallResponse response)
        {
            PlayFab.ClientModels.LoginWithCustomIDRequest request = new PlayFab.ClientModels.LoginWithCustomIDRequest()
            {
                CustomId = SystemInfo.deviceUniqueIdentifier
               
                ,CreateAccount = true
            };

           // CallStatus status = CallStatus.NotComplete;
            PlayFabClientAPI.LoginWithCustomID(
                request,
                (PlayFab.ClientModels.LoginResult obj) => { LoginSucess(obj, response); },
                (PlayFabError obj) => { LoginFailure(obj, response); }
                );

        }

        ///// <summary>
        ///// Allow second user login for debugging
        ///// </summary>
        private void WindowsAnonymousDebugLogin(CallResponse response) // todo remove this
        {
            const string UDIModifier = "2";

            PlayFab.ClientModels.LoginWithCustomIDRequest request = new PlayFab.ClientModels.LoginWithCustomIDRequest()
            {
                CustomId = SystemInfo.deviceUniqueIdentifier + UDIModifier,
                CreateAccount = true
            };

    
            PlayFabClientAPI.LoginWithCustomID(
                request,
                (PlayFab.ClientModels.LoginResult obj) => LoginSucess(obj, response),
                (PlayFabError obj) => LoginFailure(obj, response)
                );


        }

        private void LoginSucess(PlayFab.ClientModels.LoginResult obj, CallResponse response)
        {

            Debug.Log($"Login Succeeded");

            IsLoggedIn = true;

            NewAccountThisSession = obj.NewlyCreated;

            LastLoginTime = obj.LastLoginTime;

            ClientEntityKey = new PlayFab.CloudScriptModels.EntityKey()
            {
                Id = obj.EntityToken.Entity.Id
                ,Type = obj.EntityToken.Entity.Type
            };

            response.status.SetSucess();
        
        }

        private void LoginFailure(PlayFabError obj, CallResponse response)
        {
            Debug.LogError($"Login Failed");
            Debug.LogError(obj.GenerateErrorReport());
            response.status.SetError(FailureReason.PlayFabError);
        }


        public void Logout() // this never needs to be called
        {
            PlayFabClientAPI.ForgetAllCredentials();

            IsLoggedIn = false;
            ClientEntityKey = null;
            LastLoginTime = null;

            //EventsManager.InvokeEvent(EventsManager.EventType.PostLogout);
            Debug.Log("Logged out");
        }

    }
}
