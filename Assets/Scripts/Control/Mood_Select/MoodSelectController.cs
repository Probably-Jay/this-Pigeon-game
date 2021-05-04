using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameCore;

public class MoodSelectController : MonoBehaviour
{

    public int playersMoodChoice = 0;

    // drop down is 0 to 3

    public void EnterGame()
    {
        SaveEmotion();
        SceneChangeController.Instance.ChangeScene(SceneChangeController.Scenes.Game);
    }

    private void SaveEmotion()
    {    
        NetSystem.NetworkHandler.Instance.NetGame.CurrentNetworkGame.EnteredGameContext.SaveSelectedEmotion((Mood.Emotion.Emotions)playersMoodChoice);
    }
}
