using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationManager : Singleton<ApplicationManager>
{
    public override void Initialise()
    {
        InitSingleton();
    }

    private void OnEnable()
    {
        EventsManager.BindEvent(EventsManager.EventType.QuitGame, Quit);
    }


    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.QuitGame, Quit);
    }


   
    public void QuitGame()
    {
        EventsManager.InvokeEvent(EventsManager.EventType.QuitGame);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }


    private void Quit()
    {
#if UNITY_EDITOR
        Debug.Log("Attempted to quit game");
#else
        Application.Quit();
#endif
    }
}

