using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class GoalSliderScript : MonoBehaviour
{
    Mood.Emotion.Emotions P1G;
    Mood.Emotion.Emotions P2G;

    [SerializeField] Image MoodTraitOne;
    [SerializeField] Image MoodTraitTwo;

    [SerializeField] TMP_Text M1T;
    [SerializeField] TMP_Text M2T;

    //private static Image MoodTraitThree; // ToDo
    //[SerializeField] TMP_Text M3T;


    /// <summary>
    /// Set the value of the current trait
    /// </summary>
    /// <param name="value">should be between 0 to 1</param>
    private static void SetBarValue(float value, Image img)
    {
        float fillAmount;
        switch (value)
        {
            case 1:
                fillAmount = 0.33f; // 1 mood point
                break;
            case 2:
                fillAmount = 0.67f; // 2 mood points
                break;
            case 3:
                fillAmount = 1.0f; // 3 mood points
                break;
            default:
                fillAmount = 0.1f; // no mood points
                break;
        }
        img.fillAmount = fillAmount;
    }

    public static float GetBarValue(Image img)
    {
        return img.fillAmount;
    }

    /// <summary>
    /// Initialize the variables
    /// </summary>
    public void UpdateTraitsDisplay()
    {
        var EV = GameManager.Instance.ActivePlayer.PlayerEnumValue;

        var GM = GameManager.Instance;
        var ET = GM.EmotionTracker;
        var GE = ET.GardenEmotions;

        var TV = GE[EV];
       // Mood.TraitValue TV = GameManager.Instance.CurrentMoods.GardenEmotions[GameManager.Instance.ActivePlayer.PlayerEnumValue];
          
        if (EV == 0) // P1
        {
            switch (GameManager.Instance.Player1Goal)
            {
                case Mood.Emotion.Emotions.Loving:
                    // Social + Joyful
                    SetBarColors(Mood.TraitValue.Scales.Social, Mood.TraitValue.Scales.Joyful);
                    SetBarValue(TV.Social, MoodTraitOne);
                    SetBarValue(TV.Joyful, MoodTraitTwo);
                    break;
                case Mood.Emotion.Emotions.Excited:
                    // Joy + Energy
                    SetBarColors(Mood.TraitValue.Scales.Joyful, Mood.TraitValue.Scales.Energetic);
                    break;
                case Mood.Emotion.Emotions.Stressed:
                    // Energy + Pain
                    SetBarColors(Mood.TraitValue.Scales.Energetic, Mood.TraitValue.Scales.Painful);
                    break;
                case Mood.Emotion.Emotions.Lonley:
                    // Pain + Social
                    SetBarColors(Mood.TraitValue.Scales.Painful, Mood.TraitValue.Scales.Social);
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (GameManager.Instance.Player2Goal)
            {
                case Mood.Emotion.Emotions.Loving:
                    // Social + Joyful
                    SetBarColors(Mood.TraitValue.Scales.Social, Mood.TraitValue.Scales.Joyful);
                    SetBarValue(TV.Social, MoodTraitOne);
                    SetBarValue(TV.Joyful, MoodTraitTwo);
                    break;
                case Mood.Emotion.Emotions.Excited:
                    // Joy + Energy
                    SetBarColors(Mood.TraitValue.Scales.Joyful, Mood.TraitValue.Scales.Energetic);
                    break;
                case Mood.Emotion.Emotions.Stressed:
                    // Energy + Pain
                    SetBarColors(Mood.TraitValue.Scales.Energetic, Mood.TraitValue.Scales.Painful);
                    break;
                case Mood.Emotion.Emotions.Lonley:
                    // Pain + Social
                    SetBarColors(Mood.TraitValue.Scales.Painful, Mood.TraitValue.Scales.Social);
                    break;
                default:
                    break;
            }
        }
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
                icon.SetText("<sprite index= 5>");
                break;
            case Mood.TraitValue.Scales.Joyful:
                bar.color = Color.blue;
                icon.SetText("<sprite index= 2>");
                break;
            case Mood.TraitValue.Scales.Energetic:
                bar.color = Color.yellow;
                icon.SetText("<sprite index= 0>");
                break;
            case Mood.TraitValue.Scales.Painful:
                bar.color = Color.red;
                icon.SetText("<sprite index= 3>");
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

    private void UpdateBarValues(int val1, int val2)
    {
        SetBarValue(val1, MoodTraitOne);
        SetBarValue(val2, MoodTraitTwo);
    }

    void Update() {
        UpdateTraitsDisplay();
    
    }
}