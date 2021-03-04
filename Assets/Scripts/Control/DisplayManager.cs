using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// created by Alexander Purvis 04/03

public class DisplayManager : MonoBehaviour
{      
    int currentGarden1MoodPride = 0;
    int currentGarden1MoodEnergy = 0;
    int currentGarden1Social = 0;

    int currentGarden2MoodPride = -1;
    int currentGarden2MoodEnergy = 1;
    int currentGarden2Social = -1;

    Text displayText;

    GardenEmotionIndicatorControls indicatorControls;

    // indicators for player 1
    public GameObject PlesentUnplesentIndicator;
   public GameObject PersonalSocialIndicator;
   public GameObject CalmEnergisedIndicator;

    // indicators for player 2
    public GameObject PlesentUnplesentIndicator2;
    public GameObject PersonalSocialIndicator2;
    public GameObject CalmEnergisedIndicator2;

  

    private void Awake()
    {
        displayText = GetComponent<Text>();
    }

    //Start is called before the first frame update
    void Start()
    {
        DisplayCurrentGardenMood();
    }

    public void AddtoGarden1Stats(Vector3 UpdateToStats) {

        currentGarden1MoodPride += Mathf.FloorToInt(UpdateToStats.x);
        currentGarden1MoodEnergy += Mathf.FloorToInt(UpdateToStats.y);
        currentGarden1Social += Mathf.FloorToInt(UpdateToStats.z);

        DisplayCurrentGardenMood();
    }

    public void AddtoGarden2Stats(Vector3 UpdateToStats)
    {
        currentGarden2MoodPride += Mathf.FloorToInt(UpdateToStats.x);
        currentGarden2MoodEnergy += Mathf.FloorToInt(UpdateToStats.y);
        currentGarden2Social += Mathf.FloorToInt(UpdateToStats.z);

        DisplayCurrentGardenMood();
    }

    public void SubtractFromGarden1Stats(Vector3 UpdateToStats)
    {

        currentGarden1MoodPride -= Mathf.FloorToInt(UpdateToStats.x);
        currentGarden1MoodEnergy -= Mathf.FloorToInt(UpdateToStats.y);
        currentGarden1Social -= Mathf.FloorToInt(UpdateToStats.z);

        DisplayCurrentGardenMood();
    }

    public void ASubtractFromGarden2Stats(Vector3 UpdateToStats)
    {
        currentGarden2MoodPride -= Mathf.FloorToInt(UpdateToStats.x);
        currentGarden2MoodEnergy -= Mathf.FloorToInt(UpdateToStats.y);
        currentGarden2Social -= Mathf.FloorToInt(UpdateToStats.z);

        DisplayCurrentGardenMood();
    }

    void DisplayCurrentGardenMood()
    {
        displayText.text = $"P1: Unpleasant/Pleasant {GetDisplayValue(currentGarden1MoodPride)},      Personal/Social {GetDisplayValue(currentGarden1MoodEnergy)},      Calm/Energised {GetDisplayValue(currentGarden1Social)}\n" +
                           $"P2: Unpleasant/Pleasant {GetDisplayValue(currentGarden2MoodPride)},      Personal/Social {GetDisplayValue(currentGarden2MoodEnergy)},      Calm/Energised {GetDisplayValue(currentGarden2Social)}";

        UpdateGarden1MoodIndicators();

        UpdateGarden1MoodIndicators2();
    }

    private string GetDisplayValue(int val)
    {

        return Mathf.Abs(val).ToString();

    }

    void UpdateGarden1MoodIndicators()
    {

        // update the First scale 
        indicatorControls = PlesentUnplesentIndicator.GetComponent<GardenEmotionIndicatorControls>();

        if (currentGarden1MoodPride < 0)
        {
            indicatorControls.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.LeftOfScale);
        }
        else if (currentGarden1MoodPride > 0)
        {
            indicatorControls.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.RightOfScale);
        }
        else if (currentGarden1MoodPride == 0)
        {
            indicatorControls.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.Neutral);
        }

        // update the Second scale 
        indicatorControls = PersonalSocialIndicator.GetComponent<GardenEmotionIndicatorControls>();

        if (currentGarden1MoodEnergy < 0)
        {
            indicatorControls.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.LeftOfScale);
        }
        else if (currentGarden1MoodEnergy > 0)
        {
            indicatorControls.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.RightOfScale);
        }
        else if (currentGarden1MoodEnergy == 0)
        {
            indicatorControls.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.Neutral);
        }

        // update the Third scale 
        indicatorControls = CalmEnergisedIndicator.GetComponent<GardenEmotionIndicatorControls>();

        if (currentGarden1Social < 0)
        {
            indicatorControls.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.LeftOfScale);
        }
        else if (currentGarden1Social > 0)
        {
            indicatorControls.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.RightOfScale);
        }
        else if (currentGarden1Social == 0)
        {
            indicatorControls.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.Neutral);
        }
    }


    void UpdateGarden1MoodIndicators2()
    {

        // update the First scale 
        indicatorControls = PlesentUnplesentIndicator2.GetComponent<GardenEmotionIndicatorControls>();

        if (currentGarden2MoodPride < 0)
        {
            indicatorControls.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.LeftOfScale);
        }
        else if (currentGarden2MoodPride > 0)
        {
            indicatorControls.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.RightOfScale);
        }
        else if (currentGarden2MoodPride == 0)
        {
            indicatorControls.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.Neutral);
        }

        // update the Second scale 
        indicatorControls = PersonalSocialIndicator2.GetComponent<GardenEmotionIndicatorControls>();

        if (currentGarden2MoodEnergy < 0)
        {
            indicatorControls.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.LeftOfScale);
        }
        else if (currentGarden2MoodEnergy > 0)
        {
            indicatorControls.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.RightOfScale);
        }
        else if (currentGarden2MoodEnergy == 0)
        {
            indicatorControls.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.Neutral);
        }

        // update the Third scale 
        indicatorControls = CalmEnergisedIndicator2.GetComponent<GardenEmotionIndicatorControls>();

        if (currentGarden2Social < 0)
        {
            indicatorControls.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.LeftOfScale);
        }
        else if (currentGarden2Social > 0)
        {
            indicatorControls.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.RightOfScale);
        }
        else if (currentGarden2Social == 0)
        {
            indicatorControls.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.Neutral);
        }
    }


    public Vector3[] GetMoodValuesGarden1()
    {
        Vector3[] gardenMoodarray;

        gardenMoodarray = new Vector3[2];

        gardenMoodarray[0] = new Vector3(currentGarden1MoodPride, currentGarden1MoodEnergy,currentGarden1Social);
        gardenMoodarray[1] = new Vector3(currentGarden2MoodPride, currentGarden2MoodEnergy, currentGarden2Social);

        return gardenMoodarray;     
    }

}
