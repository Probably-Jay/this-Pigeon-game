using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameCore;

public class MoodSelectController : MonoBehaviour
{
    public int playersMoodChoice = -1;

    public void EnterGame()
    {

        if (playersMoodChoice == -1)
        {
            return;
        }
        else
        {
            SaveEmotion();
            SceneChangeController.Instance.ChangeScene(SceneChangeController.Scenes.Game);
        }    
    }

    public void BackToMain()
    {
        SceneChangeController.Instance.ChangeScene(SceneChangeController.Scenes.MainMenu);
    }

    private void SaveEmotion()
    {    
        NetSystem.NetworkHandler.Instance.NetGame.CurrentNetworkGame.EnteredGameContext.SaveSelectedEmotion((Mood.Emotion.Emotions)playersMoodChoice);
    }
}
