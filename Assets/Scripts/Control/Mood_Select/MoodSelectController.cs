using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameCore;

public class MoodSelectController : MonoBehaviour
{

    [SerializeField] Dropdown dropdown;


    public void EnterGame()
    {
        SaveEmotion();
        SceneChangeController.Instance.ChangeScene(SceneChangeController.Scenes.Game);
    }

    private void SaveEmotion()
    {
        GameCore.GameManager.Instance.NewGameMoodGoalTemp = dropdown.value;
    }
}
