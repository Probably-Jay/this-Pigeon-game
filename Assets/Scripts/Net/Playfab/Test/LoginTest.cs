using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class LoginTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Login();
    }

    void Login()
    {
        LoginWithCustomIDRequest request = new LoginWithCustomIDRequest() { 
            CustomId = SystemInfo.deviceUniqueIdentifier+"2", 
            CreateAccount = true };

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
    }

    private void Failure(PlayFabError obj)
    {
        Debug.LogError($"Login Failed");
        Debug.LogError(obj.GenerateErrorReport());
    }
    // https://discord.com/api/webhooks/829027756771770378/uRo4KiS-B6ealQjUbBrpWu-AkTa1KPAZkWW5P2bgZqBIWo0FYmgS_BgFCc7J1V883WHF
}
