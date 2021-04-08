using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoodSelectController : MonoBehaviour
{


    [SerializeField] GoalStore goalStore;


    public void NextMoodSelect()
    {
        SaveEmotion();
        SceneChangeController.Instance.ChangeScene(SceneChangeController.Scenes.MoodSelectScreen2);
    }

    public void EnterGame()
    {
        SaveEmotion();
        SceneChangeController.Instance.ChangeScene(SceneChangeController.Scenes.Game);
    }

    private void SaveEmotion() => goalStore.StoreGoal();
}
