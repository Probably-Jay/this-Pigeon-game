using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationManager : MonoBehaviour
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

    // todo remove this for the actual build
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            EventsManager.InvokeEvent(EventsManager.EventType.QuitGame);
        }
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
