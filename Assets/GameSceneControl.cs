using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneControl : MonoBehaviour
{
    public void Menu() => SceneChangeController.Instance.ChangeScene(SceneChangeController.Scenes.MainMenu);
    public void Quit() => ApplicationManager.Instance.QuitGame();


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

        }
    }


}
