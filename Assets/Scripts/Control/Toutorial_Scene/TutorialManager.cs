using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public void EnterGame() => SceneChangeController.Instance.ChangeScene(SceneChangeController.Scenes.MoodSelectScreen);
}
