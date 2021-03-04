using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoodSelectController : MonoBehaviour
{
    public void EnterGame() => SceneChangeController.Instance.ChangeScene(SceneChangeController.Scenes.Tutorial);
}
