using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoodSelectController : MonoBehaviour
{
    public void NextMoodSelect() => SceneChangeController.Instance.ChangeScene(SceneChangeController.Scenes.MoodSelectScreen2);
    public void EnterGame() => SceneChangeController.Instance.ChangeScene(SceneChangeController.Scenes.Game);
}
