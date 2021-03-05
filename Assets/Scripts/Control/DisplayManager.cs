using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// created by Alexander Purvis 04/03

public class DisplayManager : MonoBehaviour
{      
    int currentGarden1MoodPleasant = 0;
    int currentGarden1MoodEnergy = 0;
    int currentGarden1Social = 0;

    int currentGarden2MoodPleasant = 0;
    int currentGarden2MoodEnergy = 0;
    int currentGarden2Social = 0;

    Text displayText;

    GardenEmotionIndicatorControls indicatorControls;

    // indicators for player 1
    public GameObject pleasantUnplesentIndicator;
    public GameObject personalSocialIndicator;
    public GameObject calmEnergisedIndicator;

    // indicators for player 2
    public GameObject pleasantUnplesentIndicator2;
    public GameObject personalSocialIndicator2;
    public GameObject calmEnergisedIndicator2;

  

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

        currentGarden1MoodPleasant += Mathf.FloorToInt(UpdateToStats.x);
        currentGarden1MoodEnergy += Mathf.FloorToInt(UpdateToStats.y);
        currentGarden1Social += Mathf.FloorToInt(UpdateToStats.z);

        DisplayCurrentGardenMood();
    }

    public void AddtoGarden2Stats(Vector3 UpdateToStats)
    {
        currentGarden2MoodPleasant += Mathf.FloorToInt(UpdateToStats.x);
        currentGarden2MoodEnergy += Mathf.FloorToInt(UpdateToStats.y);
        currentGarden2Social += Mathf.FloorToInt(UpdateToStats.z);

        DisplayCurrentGardenMood();
    }

    public void SubtractFromGarden1Stats(Vector3 UpdateToStats)
    {

        currentGarden1MoodPleasant -= Mathf.FloorToInt(UpdateToStats.x);
        currentGarden1MoodEnergy -= Mathf.FloorToInt(UpdateToStats.y);
        currentGarden1Social -= Mathf.FloorToInt(UpdateToStats.z);

        DisplayCurrentGardenMood();
    }

    public void ASubtractFromGarden2Stats(Vector3 UpdateToStats)
    {
        currentGarden2MoodPleasant -= Mathf.FloorToInt(UpdateToStats.x);
        currentGarden2MoodEnergy -= Mathf.FloorToInt(UpdateToStats.y);
        currentGarden2Social -= Mathf.FloorToInt(UpdateToStats.z);

        DisplayCurrentGardenMood();
    }

    void DisplayCurrentGardenMood()
    {
        displayText.text = $"P1: {GetDisplayText("Pleasant", "Unpleasant", currentGarden1MoodPleasant)},      {GetDisplayText("Personal", "Social", currentGarden1MoodEnergy)},      {GetDisplayText("Calm", "Energised", currentGarden1Social)}\n" +
                           $"P2: {GetDisplayText("Pleasant", "Unpleasant", currentGarden2MoodPleasant)},      {GetDisplayText("Personal", "Social", currentGarden2MoodEnergy)},      {GetDisplayText("Calm", "Energised", currentGarden2Social)}";

        UpdateGarden1MoodIndicators();

        UpdateGarden1MoodIndicators2();
    }


    string GetDisplayText(string srt1, string str2, int value)
    {
        if (value > 0)
        {
            return $"<b>{srt1}</b>/{str2} {Mathf.Abs(value).ToString()}";
        }
        else if (value < 0)
        {
            return $"{srt1}/<b>{str2}</b> {Mathf.Abs(value).ToString()}";
        }
        else
        {
            return $"{srt1}/{str2} {Mathf.Abs(value).ToString()}";
        }
    }

    private string GetDisplayValue(int val)
    {

        return Mathf.Abs(val).ToString();

    }

    void UpdateGarden1MoodIndicators()
    {

        // update the First scale 
        indicatorControls = pleasantUnplesentIndicator.GetComponent<GardenEmotionIndicatorControls>();

        if (currentGarden1MoodPleasant < 0)
        {
            indicatorControls.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.LeftOfScale);
        }
        else if (currentGarden1MoodPleasant > 0)
        {
            indicatorControls.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.RightOfScale);
        }
        else if (currentGarden1MoodPleasant == 0)
        {
            indicatorControls.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.Neutral);
        }

        // update the Second scale 
        indicatorControls = personalSocialIndicator.GetComponent<GardenEmotionIndicatorControls>();

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
        indicatorControls = calmEnergisedIndicator.GetComponent<GardenEmotionIndicatorControls>();

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
        indicatorControls = pleasantUnplesentIndicator2.GetComponent<GardenEmotionIndicatorControls>();

        if (currentGarden2MoodPleasant < 0)
        {
            indicatorControls.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.LeftOfScale);
        }
        else if (currentGarden2MoodPleasant > 0)
        {
            indicatorControls.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.RightOfScale);
        }
        else if (currentGarden2MoodPleasant == 0)
        {
            indicatorControls.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.Neutral);
        }

        // update the Second scale 
        indicatorControls = personalSocialIndicator2.GetComponent<GardenEmotionIndicatorControls>();

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
        indicatorControls = calmEnergisedIndicator2.GetComponent<GardenEmotionIndicatorControls>();

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


    public Dictionary<Player.PlayerEnum, int[]> GetMoodValuesGardens()
    {
        Dictionary<Player.PlayerEnum, int[]> gardenMoodarray;

        gardenMoodarray = new Dictionary<Player.PlayerEnum, int[]>();

        gardenMoodarray[Player.PlayerEnum.Player0] = new int[3] { currentGarden1MoodPleasant, currentGarden1MoodEnergy, currentGarden1Social };
        gardenMoodarray[Player.PlayerEnum.Player1] = new int[3] { currentGarden2MoodPleasant, currentGarden2MoodEnergy, currentGarden2Social };

        return gardenMoodarray;     
    }

}
