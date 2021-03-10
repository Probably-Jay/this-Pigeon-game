using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// created by Alexander Purvis 04/03
// Edited SJ 10/03

public class DisplayManager : MonoBehaviour
{
    int garden1MoodUnpleasantPleasant = 0;
    int garden1MoodPersonalSocial = 0;
    int garden1CalmEnergised = 0;

    int garden2MoodUnpleasantPleasant = 0;
    int garden2MoodPersonalSocial = 0;
    int garden2CalmEnergised = 0;

    TMP_Text displayText;

    //GardenEmotionIndicatorControls indicatorControls;

    // indicators for player 1
    //public GameObject pleasantUnplesentIndicator;
    // public GameObject personalSocialIndicator;
    //public GameObject calmEnergisedIndicator;

    // indicators for player 2
    // public GameObject pleasantUnplesentIndicator2;
    //public GameObject personalSocialIndicator2;
    //public GameObject calmEnergisedIndicator2;



    public TMP_Text P1UnpleasantPleasantTextDisplay;
    public TMP_Text P1PersonalSocialTextDisplay;
    public TMP_Text P1CalmEnergisedTextDisplay;

    public TMP_Text P2UnpleasantPleasantTextDisplay;
    public TMP_Text P2PersonalSocialTextDisplay;
    public TMP_Text P2CalmEnergisedTextDisplay;



    string p1UnpleasantPleasant_Text = "";
    string p1PersonalSocial_Text = "";
    string p1CalmEnergised_Text = "";

    string p2UnpleasantPleasant_Text = "";
    string p2PersonalSocial_Text = "";
    string p2CalmEnergised_Text = "";


    private void Awake()
    {
        displayText = GetComponent<TMP_Text>();
    }

    //Start is called before the first frame update
    void Start()
    {
        DisplayCurrentGardenMood();
    }

    public void AddtoGarden1Stats(Vector3 UpdateToStats)
    {

        garden1MoodUnpleasantPleasant += Mathf.FloorToInt(UpdateToStats.x);
        garden1MoodPersonalSocial += Mathf.FloorToInt(UpdateToStats.y);
        garden1CalmEnergised += Mathf.FloorToInt(UpdateToStats.z);


        DisplayCurrentGardenMood();
    }

    public void AddtoGarden2Stats(Vector3 UpdateToStats)
    {
        garden2MoodUnpleasantPleasant += Mathf.FloorToInt(UpdateToStats.x);
        garden2MoodPersonalSocial += Mathf.FloorToInt(UpdateToStats.y);
        garden2CalmEnergised += Mathf.FloorToInt(UpdateToStats.z);

        DisplayCurrentGardenMood();
    }

    public void SubtractFromGarden1Stats(Vector3 UpdateToStats)
    {

        garden1MoodUnpleasantPleasant -= Mathf.FloorToInt(UpdateToStats.x);
        garden1MoodPersonalSocial -= Mathf.FloorToInt(UpdateToStats.y);
        garden1CalmEnergised -= Mathf.FloorToInt(UpdateToStats.z);

        DisplayCurrentGardenMood();
    }

    public void ASubtractFromGarden2Stats(Vector3 UpdateToStats)
    {
        garden2MoodUnpleasantPleasant -= Mathf.FloorToInt(UpdateToStats.x);
        garden2MoodPersonalSocial -= Mathf.FloorToInt(UpdateToStats.y);
        garden2CalmEnergised -= Mathf.FloorToInt(UpdateToStats.z);

        DisplayCurrentGardenMood();
    }

    /// Mood Index Sprites
    /// 0 = Energetic
    /// 1 = NeutPleasant
    /// 2 = Pleasant
    /// 3 = Sad (TEMP, ONLY FOR TESTING)
    /// 4 = NeutSocial
    /// 5 = Social
    /// 6 = Calm
    /// 7 = Unpleasant
    /// 8 = Personal
    /// 9 = NeutEnergy

    void getUnpleasantPleasanttext()
    {

        // set player 1 Unpleasant/Pleasant text        
        if (garden1MoodUnpleasantPleasant < 0)
        {
            P1UnpleasantPleasantTextDisplay.text = $"Unpleasant: {Mathf.Abs(garden1MoodUnpleasantPleasant).ToString()} <sprite=3>";
        }
        else if (garden1MoodUnpleasantPleasant > 0)
        {
            P1UnpleasantPleasantTextDisplay.text = $"Pleasant: {Mathf.Abs(garden1MoodUnpleasantPleasant).ToString()} <sprite=2>";
        }
        else if (garden1MoodUnpleasantPleasant == 0)
        {
            P1UnpleasantPleasantTextDisplay.text = $"Neutral: {Mathf.Abs(garden1MoodUnpleasantPleasant).ToString()} <sprite=1>";
        }

        // set player 2 Unpleasant/Pleasant text        
        if (garden2MoodUnpleasantPleasant < 0)
        {
            P2UnpleasantPleasantTextDisplay.text = $"Unpleasant: {Mathf.Abs(garden2MoodUnpleasantPleasant).ToString()} <sprite=3>";
        }
        else if (garden2MoodUnpleasantPleasant > 0)
        {
            P2UnpleasantPleasantTextDisplay.text = $"Pleasant: {Mathf.Abs(garden2MoodUnpleasantPleasant).ToString()} <sprite=2>";
        }
        else if (garden2MoodUnpleasantPleasant == 0)
        {
            P2UnpleasantPleasantTextDisplay.text = $"Neutral: {Mathf.Abs(garden2MoodUnpleasantPleasant).ToString()} <sprite=1>";
        }
    }

    void getPersonalSocialtext()
    {
        // set player 1 Personal/Social text
        if (garden1MoodPersonalSocial < 0)
        {
            P1PersonalSocialTextDisplay.text = $"Personal: {Mathf.Abs(garden1MoodPersonalSocial).ToString()} <sprite=8>";
        }
        else if (garden1MoodPersonalSocial > 0)
        {
            P1PersonalSocialTextDisplay.text = $"Social: {Mathf.Abs(garden1MoodPersonalSocial).ToString()} <sprite=5>";
        }
        else if (garden1MoodPersonalSocial == 0)
        {
            P1PersonalSocialTextDisplay.text = $"Neutral: {Mathf.Abs(garden1MoodPersonalSocial).ToString()} <sprite=4>";
        }

        // set player 2 Personal/Social text
        if (garden2MoodPersonalSocial < 0)
        {
            P2PersonalSocialTextDisplay.text = $"Personal: {Mathf.Abs(garden2MoodPersonalSocial).ToString()} <sprite=8>";
        }
        else if (garden2MoodPersonalSocial > 0)
        {
            P2PersonalSocialTextDisplay.text = $"Social: {Mathf.Abs(garden2MoodPersonalSocial).ToString()} <sprite=5>";
        }
        else if (garden2MoodPersonalSocial == 0)
        {
            P2PersonalSocialTextDisplay.text = $"Neutral: {Mathf.Abs(garden2MoodPersonalSocial).ToString()} <sprite=4>";
        }
    }

    void getCalmEnergisedtext()
    {
        // set player 1 Calm/Energised text
        if (garden1CalmEnergised < 0)
        {
            P1CalmEnergisedTextDisplay.text = $"Calm: {Mathf.Abs(garden1CalmEnergised).ToString()} <sprite=6>";
        }
        else if (garden1CalmEnergised > 0)
        {
            P1CalmEnergisedTextDisplay.text = $"Energised: {Mathf.Abs(garden1CalmEnergised).ToString()} <sprite=0>";
        }
        else if (garden1CalmEnergised == 0)
        {
            P1CalmEnergisedTextDisplay.text = $"Neutral: {Mathf.Abs(garden1CalmEnergised).ToString()} <sprite=9>";
        }

        // set player 2 Calm/Energised text
        if (garden2CalmEnergised < 0)
        {
            P2CalmEnergisedTextDisplay.text = $"Calm: {Mathf.Abs(garden2CalmEnergised).ToString()} <sprite=6>";
        }
        else if (garden2CalmEnergised > 0)
        {
            P2CalmEnergisedTextDisplay.text = $"Energised: {Mathf.Abs(garden2CalmEnergised).ToString()} <sprite=0>";
        }
        else if (garden2CalmEnergised == 0)
        {
            P2CalmEnergisedTextDisplay.text = $"Neutral: {Mathf.Abs(garden2CalmEnergised).ToString()} <sprite=9>";
        }
    }

    void DisplayCurrentGardenMood()
    {
        displayText.text = $"P1:\n\nP2:";



        getUnpleasantPleasanttext();
        getPersonalSocialtext();
        getCalmEnergisedtext();

        EventsManager.InvokeEvent(EventsManager.EventType.UpdateScore);
    }


    string GetDisplayText(string srt1, int value)
    {
        if (value > 0)
        {
            return $"<b>{srt1}</b> {Mathf.Abs(value).ToString()}     ";
        }
        else if (value < 0)
        {
            return $"<b>{srt1}</b> {Mathf.Abs(value).ToString()}";
        }
        else
        {
            return $"        <b>{srt1}</b> {Mathf.Abs(value).ToString()}";
        }
    }

    private string GetDisplayValue(int val)
    {

        return Mathf.Abs(val).ToString();

    }

    /*
    
    
    void UpdateGarden1MoodIndicators()
    {

        // update the First scale 
        indicatorControls = pleasantUnplesentIndicator.GetComponent<GardenEmotionIndicatorControls>();

        if (garden1MoodUnpleasantPleasant < 0)
        {
            indicatorControls.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.LeftOfScale);
        }
        else if (garden1MoodUnpleasantPleasant > 0)
        {
            indicatorControls.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.RightOfScale);
        }
        else if (garden1MoodUnpleasantPleasant == 0)
        {
            indicatorControls.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.Neutral);
        }

        // update the Second scale 
        indicatorControls = personalSocialIndicator.GetComponent<GardenEmotionIndicatorControls>();

        if (garden1MoodPersonalSocial < 0)
        {
            indicatorControls.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.LeftOfScale);
        }
        else if (garden1MoodPersonalSocial > 0)
        {
            indicatorControls.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.RightOfScale);
        }
        else if (garden1MoodPersonalSocial == 0)
        {
            indicatorControls.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.Neutral);
        }

        // update the Third scale 
        indicatorControls = calmEnergisedIndicator.GetComponent<GardenEmotionIndicatorControls>();

        if (garden1CalmEnergised < 0)
        {
            indicatorControls.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.LeftOfScale);
        }
        else if (garden1CalmEnergised > 0)
        {
            indicatorControls.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.RightOfScale);
        }
        else if (garden1CalmEnergised == 0)
        {
            indicatorControls.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.Neutral);
        }
    }

    void UpdateGarden1MoodIndicators2()
    {

        // update the First scale 
        indicatorControls = pleasantUnplesentIndicator2.GetComponent<GardenEmotionIndicatorControls>();

        if (garden2MoodUnpleasantPleasant < 0)
        {
            indicatorControls.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.LeftOfScale);
        }
        else if (garden2MoodUnpleasantPleasant > 0)
        {
            indicatorControls.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.RightOfScale);
        }
        else if (garden2MoodUnpleasantPleasant == 0)
        {
            indicatorControls.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.Neutral);
        }

        // update the Second scale 
        indicatorControls = personalSocialIndicator2.GetComponent<GardenEmotionIndicatorControls>();

        if (garden2MoodPersonalSocial < 0)
        {
            indicatorControls.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.LeftOfScale);
        }
        else if (garden2MoodPersonalSocial > 0)
        {
            indicatorControls.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.RightOfScale);
        }
        else if (garden2MoodPersonalSocial == 0)
        {
            indicatorControls.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.Neutral);
        }

        // update the Third scale 
        indicatorControls = calmEnergisedIndicator2.GetComponent<GardenEmotionIndicatorControls>();

        if (garden2CalmEnergised < 0)
        {
            indicatorControls.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.LeftOfScale);
        }
        else if (garden2CalmEnergised > 0)
        {
            indicatorControls.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.RightOfScale);
        }
        else if (garden2CalmEnergised == 0)
        {
            indicatorControls.UpdateIndicator(GardenEmotionIndicatorControls.EmotionState.Neutral);
        }
    }

     */


    public Dictionary<Player.PlayerEnum, int[]> GetMoodValuesGardens()
    {
        Dictionary<Player.PlayerEnum, int[]> gardenMoodarray;

        gardenMoodarray = new Dictionary<Player.PlayerEnum, int[]>();

        gardenMoodarray[Player.PlayerEnum.Player0] = new int[3] { garden1MoodUnpleasantPleasant, garden1MoodPersonalSocial, garden1CalmEnergised };
        gardenMoodarray[Player.PlayerEnum.Player1] = new int[3] { garden2MoodUnpleasantPleasant, garden2MoodPersonalSocial, garden2CalmEnergised };

        return gardenMoodarray;
    }

}
