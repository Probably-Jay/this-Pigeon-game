using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// created by Alexander Purvis 04/03

public class DisplayManager : MonoBehaviour
{      
    int garden1MoodUnpleasantPleasant = 0;
    int garden1MoodPersonalSocial = 0;
    int garden1CalmEnergised = 0;

    int garden2MoodUnpleasantPleasant = 0;
    int garden2MoodPersonalSocial = 0;
    int garden2CalmEnergised = 0;

    MoodAtributes garden1;
    MoodAtributes garden2;

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


    
    public Text P1UnpleasantPleasantTextDisplay;
    public Text P1PersonalSocialTextDisplay;
    public Text P1CalmEnergisedTextDisplay;


    
    public Text P2UnpleasantPleasantDisplay;
    public Text P2PersonalSocialDisplay;
    public Text P2CalmEnergisedDisplay;



    string p1UnpleasantPleasant_Text = "";
    string p1PersonalSocial_Text = "";
    string p1CalmEnergised_Text = "";

    string p2UnpleasantPleasant_Text = "";
    string p2PersonalSocial_Text = "";
    string p2CalmEnergised_Text = "";


    private void Awake()
    {
        displayText = GetComponent<Text>();
    }

    //Start is called before the first frame update
    void Start()
    {
        DisplayCurrentGardenMood();
    }

    public void AddToGardenStats(Player.PlayerEnum player, MoodAtributes moodAtributes)
    {
        switch (player)
        {
            case Player.PlayerEnum.Player0:
                garden1 += moodAtributes;
                break;
            case Player.PlayerEnum.Player1:
                garden2 += moodAtributes;
                break;
        }
        DisplayCurrentGardenMood();
    }

    public void SubtractFromGardenStats(Player.PlayerEnum player, MoodAtributes moodAtributes)
    {
        switch (player)
        {
            case Player.PlayerEnum.Player0:
                garden1 -= moodAtributes;
                break;
            case Player.PlayerEnum.Player1:
                garden2 -= moodAtributes;
                break;
        }
        DisplayCurrentGardenMood();
    }

    // depracated add and subtract
    /*
    public void AddtoGarden1Stats(Vector3 UpdateToStats) {

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
    */

    void getUnpleasantPleasanttext()
    {
        
        // set player 1 Unpleasant/Pleasant text        
        if (garden1MoodUnpleasantPleasant < 0)
        {
             P1UnpleasantPleasantTextDisplay.text = $"Unpleasant {Mathf.Abs(garden1MoodUnpleasantPleasant).ToString()}";
        }
        else if (garden1MoodUnpleasantPleasant > 0)
        {
            P1UnpleasantPleasantTextDisplay.text = $"Pleasant {Mathf.Abs(garden1MoodUnpleasantPleasant).ToString()}";
        }
        else if (garden1MoodUnpleasantPleasant == 0)
        {
            P1UnpleasantPleasantTextDisplay.text = $"Neutral {Mathf.Abs(garden1MoodUnpleasantPleasant).ToString()}";
        }

        // set player 2 Unpleasant/Pleasant text        
        if (garden2MoodUnpleasantPleasant < 0)
        {
            P2UnpleasantPleasantDisplay.text = $"Unpleasant {Mathf.Abs(garden2MoodUnpleasantPleasant).ToString()}";
        }
        else if (garden2MoodUnpleasantPleasant > 0)
        {
            P2UnpleasantPleasantDisplay.text = $"Pleasant {Mathf.Abs(garden2MoodUnpleasantPleasant).ToString()}";
        }
        else if (garden2MoodUnpleasantPleasant == 0)
        {
            P2UnpleasantPleasantDisplay.text = $"Neutral {Mathf.Abs(garden2MoodUnpleasantPleasant).ToString()}";
        }
    }

    void getPersonalSocialtext()
    {
        // set player 1 Personal/Social text
        if (garden1MoodPersonalSocial < 0)
        {
            P1PersonalSocialTextDisplay.text = $"Personal {Mathf.Abs(garden1MoodPersonalSocial).ToString()}";
        }
        else if (garden1MoodPersonalSocial > 0)
        {
            P1PersonalSocialTextDisplay.text = $"Social {Mathf.Abs(garden1MoodPersonalSocial).ToString()}";
        }
        else if (garden1MoodPersonalSocial == 0)
        {
            P1PersonalSocialTextDisplay.text = $"Neutral {Mathf.Abs(garden1MoodPersonalSocial).ToString()}";
        }

        // set player 2 Personal/Social text
        if (garden2MoodPersonalSocial < 0)
        {
            P2PersonalSocialDisplay.text = $"Personal {Mathf.Abs(garden2MoodPersonalSocial).ToString()}";
        }
        else if (garden2MoodPersonalSocial > 0)
        {
            P2PersonalSocialDisplay.text = $"Social {Mathf.Abs(garden2MoodPersonalSocial).ToString()}";
        }
        else if (garden2MoodPersonalSocial == 0)
        {
            P2PersonalSocialDisplay.text = $"Neutral {Mathf.Abs(garden2MoodPersonalSocial).ToString()}";
        }
    }
    void getCalmEnergisedtext()
    {
        // set player 1 Calm/Energised text
        if (garden1CalmEnergised < 0)
        {
            P1CalmEnergisedTextDisplay.text = $"Calm {Mathf.Abs(garden1CalmEnergised).ToString()}";
        }
        else if (garden1CalmEnergised > 0)
        {
            P1CalmEnergisedTextDisplay.text = $"Energised {Mathf.Abs(garden1CalmEnergised).ToString()}";
        }
        else if (garden1CalmEnergised == 0)
        {
            P1CalmEnergisedTextDisplay.text = $"Neutral {Mathf.Abs(garden1CalmEnergised).ToString()}";
        }


        // set player 2 Calm/Energised text

        if (garden2CalmEnergised < 0)
        {
            P2CalmEnergisedDisplay.text = $"Calm {Mathf.Abs(garden2CalmEnergised).ToString()}";
        }
        else if (garden2CalmEnergised > 0)
        {
            P2CalmEnergisedDisplay.text = $"Energised {Mathf.Abs(garden2CalmEnergised).ToString()}";
        }
        else if (garden2CalmEnergised == 0)
        {
            P2CalmEnergisedDisplay.text = $"Neutral {Mathf.Abs(garden2CalmEnergised).ToString()}";
        }
    }
  

     void DisplayCurrentGardenMood()
     {
        displayText.text = $"P1:\n\n P2:";



        getUnpleasantPleasanttext();
        getPersonalSocialtext();
        getCalmEnergisedtext();

 
        UpdateGarden1MoodIndicators();
        UpdateGarden1MoodIndicators2();

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


    public Dictionary<Player.PlayerEnum, MoodAtributes> GetMoodValuesGardens()
    {
        Dictionary<Player.PlayerEnum, MoodAtributes> gardenMoodarray;

        gardenMoodarray = new Dictionary<Player.PlayerEnum, MoodAtributes>();

        gardenMoodarray[Player.PlayerEnum.Player0] = new MoodAtributes( garden1MoodUnpleasantPleasant, garden1MoodPersonalSocial, garden1CalmEnergised );
        gardenMoodarray[Player.PlayerEnum.Player1] = new MoodAtributes( garden2MoodUnpleasantPleasant, garden2MoodPersonalSocial, garden2CalmEnergised );

        return gardenMoodarray;     
    }

}
