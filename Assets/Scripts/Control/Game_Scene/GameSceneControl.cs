using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// jay 07/03

public class GameSceneControl : MonoBehaviour
{
    public void Menu() => SceneChangeController.Instance.ChangeScene(SceneChangeController.Scenes.MainMenu);
    public void Quit() => ApplicationManager.Instance.QuitGame();


    private void Start()
    {
        EventsManager.InvokeEvent(EventsManager.EventType.StartGame);
    }


}
