using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AplicationManager : Singleton<AplicationManager>
{
    //public override void Awake()
    //{
    //    InitSingleton();

    //}

    public override void Initialise()
    {
        InitSingleton();
    }

    private void OnEnable()
    {
        EventsManager.BindEvent(EventsManager.EventType.QuitGame, QuitGame);
    }


    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.QuitGame, QuitGame);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        Debug.Log("Attempted to quit game");
#else
        Application.Quit();
#endif
    }

}
