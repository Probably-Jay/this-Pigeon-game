using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsButton : MonoBehaviour
{
    public void OpenCreditsScene()
    {
        SceneChangeController.Instance.ChangeScene(SceneChangeController.Scenes.Credits);
    }

    public void MainMenu()
    {
        SceneChangeController.Instance.ChangeScene(SceneChangeController.Scenes.MainMenu);

    }

  
}
