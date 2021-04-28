using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsButton : MonoBehaviour
{
    public void OpenCreditsScene()
    {
        SceneManager.LoadScene("CreditsScene");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
