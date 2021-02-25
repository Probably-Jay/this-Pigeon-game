using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AplicationManager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        EventsManager.BindEvent(EventsManager.EventType.QuitGame, QuitGame);
    }


    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.QuitGame, QuitGame);
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        Debug.Log("Attempted to quit game");
#else
        Application.Quit();
#endif
    }

}
