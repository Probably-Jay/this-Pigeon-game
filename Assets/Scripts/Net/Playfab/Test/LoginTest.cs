﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using System;
namespace Net {
    public class LoginTest : MonoBehaviour
    {


        [SerializeField] InputField messageBox;

        string ID;

        public void SendMessage()
        {
            string message = $"{ID} send message from unity: {messageBox.text}";

            var request = new ExecuteCloudScriptRequest()
            {
                FunctionName = "SendDiscordMessage",
                FunctionParameter = new
                { 
                    msg = message
                }

            };
            PlayFabClientAPI.ExecuteCloudScript(request, ScriptExecutedSucess, ScriptExecutedfailure);
        }

        private void ScriptExecutedfailure(PlayFabError obj)
        {
            Debug.LogError($"Execution Failed");
            Debug.LogError(obj.GenerateErrorReport());
        }

        private void ScriptExecutedSucess(ExecuteCloudScriptResult obj)
        {
            Debug.Log("Sent");
            Debug.Log(obj.ToString());
            Debug.Log(obj.HttpRequestsIssued);
            Debug.Log(obj.Logs);
        }



        // Start is called before the first frame update
        void Start()
        {
            Login();
        }

        void Login()
        {
            switch (Application.platform)
            {

                case RuntimePlatform.WindowsPlayer:
                    WindowsLogin();
                    break;
                case RuntimePlatform.WindowsEditor:
                    WindowsLogin();
                    break;
                case RuntimePlatform.Android:
                    AndroidLogin();
                    break;
                default:
                    throw new Exception();

            }


        }

        private void AndroidLogin()
        {
            var androidRequest = new LoginWithAndroidDeviceIDRequest()
            {
                AndroidDeviceId = SystemInfo.deviceUniqueIdentifier,
                CreateAccount = true
            };

            PlayFabClientAPI.LoginWithAndroidDeviceID(androidRequest, Sucess, Failure);
        }

        private void WindowsLogin()
        {
            LoginWithCustomIDRequest request = new LoginWithCustomIDRequest()
            {
                CustomId = SystemInfo.deviceUniqueIdentifier + "2",
                CreateAccount = true
            };

            PlayFabClientAPI.LoginWithCustomID(request, Sucess, Failure);
        }

        private void Sucess(LoginResult obj)
        {
            if (obj.NewlyCreated)
            {
                Debug.Log($"Account created and logged in! {obj.PlayFabId}");

            }
            else
            {
                Debug.Log($"logged in! {obj.PlayFabId}");

            }
            ID = obj.PlayFabId;
        }

        private void Failure(PlayFabError obj)
        {
            Debug.LogError($"Login Failed");
            Debug.LogError(obj.GenerateErrorReport());
        }
        // https://discord.com/api/webhooks/829027756771770378/uRo4KiS-B6ealQjUbBrpWu-AkTa1KPAZkWW5P2bgZqBIWo0FYmgS_BgFCc7J1V883WHF
    }
}