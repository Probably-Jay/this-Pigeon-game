using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// jay 07/03



public class GameSceneControl : MonoBehaviour
{
    //public void Menu() => SceneChangeController.Instance.ChangeScene(SceneChangeController.Scenes.MainMenu);
    //public void Quit() => ApplicationManager.Instance.QuitGame();


    private void Start()
    {
        EventsManager.InvokeEvent(EventsManager.EventType.EnterGameScene);
    }

    private void OnDestroy()
    {
        Debug.Log($"{nameof(GameSceneControl)} destroyed");
    }

    private void OnApplicationQuit()
    {
        //   GameCore.GameManager.Instance.SaveGame();
        Debug.LogWarning("Quit game saving has been disabled for debugging");
    }
}
