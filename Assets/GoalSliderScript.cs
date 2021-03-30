using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;
using System;

public class GoalSliderScript : MonoBehaviour
{
    Mood.Emotion.Emotions P1G;
    Mood.Emotion.Emotions P2G;

    [SerializeField] Image MoodTraitOne;
    [SerializeField] Image MoodTraitTwo;

    [SerializeField] TMP_Text M1T;
    [SerializeField] TMP_Text M2T;



    private void OnEnable()
    {
        EventsManager.BindEvent(EventsManager.EventType.PlantAlterStats, UpdateTraitsDisplay);
    }

    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.PlantAlterStats, UpdateTraitsDisplay);
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
        //switch (value)
        //{
        //    case 1:
        //        fillAmount = 0.33f; // 1 mood point
        //        break;
        //    case 2:
        //        fillAmount = 0.67f; // 2 mood points
        //        break;
        //    case 3:
        //        fillAmount = 1.0f; // 3 mood points
        //        break;
        //    default:
        //        fillAmount = 0.1f; // no mood points
        //        break;
        //}
        img.fillAmount = fillAmount;
    }

    private static float GetFillvalue(int value, int goalvalue) => Mathf.InverseLerp(0, goalvalue, value);

    public static float GetBarValue(Image img)
    {
        return img.fillAmount;
    }





    /// <summary>
    /// Initialize the variables
    /// </summary>
    public void UpdateTraitsDisplay()
    {
       // int activePlayer = (int)GameManager.Instance.ActivePlayer.PlayerEnumValue;


        Player.PlayerEnum activePlayer = GameManager.Instance.ActivePlayer.PlayerEnumValue;
        Mood.TraitValue currentTraits = GameManager.Instance.EmotionTracker.GardenCurrentTraits[activePlayer];
        Mood.TraitValue goalTraits = GameManager.Instance.EmotionTracker.GardenGoalTraits[activePlayer];



        Mood.TraitValue traits = GameManager.Instance.EmotionTracker.GardenGoalTraits[activePlayer];

        (Mood.TraitValue.Scales, Mood.TraitValue.Scales) traitsInEmotion = Mood.Emotion.GetScalesInEmotion(GameManager.Instance.EmotionTracker.GardenGoalEmotions[activePlayer]);

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
                icon.SetText("<sprite index= 2>");
                break;
            case Mood.TraitValue.Scales.Joyful:
                bar.color = Color.blue;
                icon.SetText("<sprite index= 4>");
                break;
            case Mood.TraitValue.Scales.Energetic:
                bar.color = Color.yellow;
                icon.SetText("<sprite index= 0>");
                break;
            case Mood.TraitValue.Scales.Painful:
                bar.color = Color.red;
                icon.SetText("<sprite index= 5>");
                break;
            default:
                break;
        }
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

    void Update() {
        UpdateTraitsDisplay();
        
    }
}