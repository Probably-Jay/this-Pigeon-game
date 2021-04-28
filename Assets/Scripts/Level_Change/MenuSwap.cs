using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Obsolete("Depracted",true)]
public class MenuSwap : MonoBehaviour
{
    public void GoToMainMenu()
    {
        SceneChangeController.Instance.ChangeScene(SceneChangeController.Scenes.MainMenu);
    }

    public void QuitGame()
    {
        // Change to metagarden after metagarden added
        EventsManager.InvokeEvent(EventsManager.EventType.QuitGame);
    }
}
