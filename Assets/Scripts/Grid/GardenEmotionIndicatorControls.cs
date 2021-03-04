using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

//Created by Alexander Purvis 04/03/2021

public class GardenEmotionIndicatorControls : MonoBehaviour
{
    public Sprite leftOfScaleSprite;
    public Sprite rightOfScaleSprite;
    public Sprite neutralSprite;

    public enum EmotionState { 
        LeftOfScale,
        RightOfScale,
        Neutral
    }
   
    public void UpdateIndicator(GardenEmotionIndicatorControls.EmotionState newEmotionState)
    {
        switch (newEmotionState)
        {
            case EmotionState.LeftOfScale:
                gameObject.GetComponent<Image>().sprite = leftOfScaleSprite;
                break;
            case EmotionState.RightOfScale:
                gameObject.GetComponent<Image>().sprite = rightOfScaleSprite;
                break;
            case EmotionState.Neutral:
                gameObject.GetComponent<Image>().sprite = neutralSprite;
                break;
            default:
                break;
        }       
    }
}
