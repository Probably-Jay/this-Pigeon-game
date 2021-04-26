using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Mood;

using TMPro;
using System;
using GameCore;

// created scott

// altered Jay 02/04, 26/04

public class GoalSliderScript : MonoBehaviour
{
    Mood.Emotion.Emotions P1G;
    Mood.Emotion.Emotions P2G;

    [SerializeField] Image MoodTraitOne;
    [SerializeField] Image MoodTraitTwo;

    [SerializeField] TMP_Text M1T;
    [SerializeField] TMP_Text M2T;

   // Action UpdateTraitsDisplayOnEnd

    private void OnEnable()
    {
        EventsManager.BindEvent(EventsManager.EventType.GardenStatsUpdated, UpdateTraitsDisplay);
        EventsManager.BindEvent(EventsManager.EventType.GameLoaded, UpdateTraitsDisplay);
    }

    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.GardenStatsUpdated, UpdateTraitsDisplay);
        EventsManager.UnbindEvent(EventsManager.EventType.GameLoaded, UpdateTraitsDisplay);
    }


    // ICON INDEX
    // 0 - Energetic
    // 1 - Reflective
    // 2 - Social
    // 3 - Calm
    // 4 - Joyful
    // 5 - Painful

    //private static Image MoodTraitThree; // ToDo
    //[SerializeField] TMP_Text M3T;


    /// <summary>
    /// Set the value of the current trait
    /// </summary>
    /// <param name="value">should be between 0 to 1</param>
    private static void SetBarValue(int value, int goalvalue, Image img)
    {
        float fillAmount = GetFillvalue(value, goalvalue);
        
        img.fillAmount = fillAmount;
    }

    private static float GetFillvalue(int value, int goalvalue) => Mathf.InverseLerp(0, goalvalue, value);

    public static float GetBarValue(Image img)
    {
        return img.fillAmount;
    }

    public void DisablePanel()
    {
        this.transform.position += new Vector3(0,100f);
    }
    public void EnablePanel()
    {
        this.transform.position -= new Vector3(0, -100f);
    }



    /// <summary>
    /// Initialize the variables
    /// </summary>
    public void UpdateTraitsDisplay()
    {
       // int activePlayer = (int)GameManager.Instance.ActivePlayer.PlayerEnumValue;


       // Player.PlayerEnum activePlayer = GameManager.Instance.LocalPlayerID;
        Mood.TraitValue currentTraits = GameManager.Instance.EmotionTracker.CurrentGardenTraits;
        Mood.Emotion goalEmotion = GameManager.Instance.EmotionTracker.EmotionGoal;
        Mood.TraitValue goalTraits = goalEmotion.traits;



       // Mood.TraitValue traits = GameManager.Instance.EmotionTracker.GardenGoalTraits[activePlayer];

        (Mood.TraitValue.Scales, Mood.TraitValue.Scales) traitsInEmotion = Mood.Emotion.GetScalesInEmotion(goalEmotion.enumValue);

        SetBarColors(traitsInEmotion.Item1, traitsInEmotion.Item2);

        SetBarValue(currentTraits[traitsInEmotion.Item1], goalTraits[traitsInEmotion.Item1], MoodTraitOne);
        SetBarValue(currentTraits[traitsInEmotion.Item2], goalTraits[traitsInEmotion.Item2], MoodTraitTwo);

    }

    /// <summary>
    /// Sets a bar's color
    /// </summary>
    private void SetBarColor(Mood.TraitValue.Scales trait, Image bar, TMP_Text icon)
    {
        switch (trait)
        {
            case Mood.TraitValue.Scales.Social:
                bar.color = Color.magenta; // Apparently there's no base purple? Change later
                break;
            case Mood.TraitValue.Scales.Joyful:
                bar.color = Color.blue;
                break;
            case Mood.TraitValue.Scales.Energetic:
                bar.color = Color.yellow;
                break;
            case Mood.TraitValue.Scales.Painful:
                bar.color = Color.red;
                break;
            default:
                break;
        }

        icon.SetText(TraitValue.GetIconDisplay(trait));
    }



    private void SetBarColors(Mood.TraitValue.Scales traitOne, Mood.TraitValue.Scales traitTwo)
    {
        SetBarColor(traitOne, MoodTraitOne, M1T);
        SetBarColor(traitTwo, MoodTraitTwo, M2T);
    }

    //private void UpdateBarValues(int val1, int val2)
    //{
    //    SetBarValue(val1, MoodTraitOne);
    //    SetBarValue(val2, MoodTraitTwo);
    //}

   
}