using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonalGardenController : MonoBehaviour
{
    public void EnterGame()
    {
        SceneChangeController.Instance.ChangeScene(SceneChangeController.Scenes.MoodSelectScreen);
    }

    public void QuitToMenu()
    {
        // SceneChangeController.Instance.ChangeScene(SceneChangeController.Scenes.MoodSelectScreen);
        EventsManager.InvokeEvent(EventsManager.EventType.QuitGame);
    }

}
