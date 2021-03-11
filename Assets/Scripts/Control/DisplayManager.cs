using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// created by Alexander Purvis 04/03
// Edited SJ 10/03
// Edited again Jay 10/03 

public class DisplayManager : MonoBehaviour
{
    //int garden1MoodUnpleasantPleasant = 0;
    //int garden1MoodPersonalSocial = 0;
    //int garden1CalmEnergised = 0;

    //int garden2MoodUnpleasantPleasant = 0;
    //int garden2MoodPersonalSocial = 0;
    //int garden2CalmEnergised = 0;

    MoodAtributes gardenMood1;
    MoodAtributes gardenMood2;

    TMP_Text displayText;

   


    public TMP_Text plesanceDisplayP1;
    public TMP_Text P1PersonalSocialTextDisplay;
    public TMP_Text P1CalmEnergisedTextDisplay;

    public TMP_Text plesanceDisplayP2;
    public TMP_Text P2PersonalSocialTextDisplay;
    public TMP_Text P2CalmEnergisedTextDisplay;



 


    private void Awake()
    {
        displayText = GetComponent<TMP_Text>();
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
                gardenMood1 += moodAtributes;
                break;
            case Player.PlayerEnum.Player1:
                gardenMood2 += moodAtributes;
                break;
        }
        DisplayCurrentGardenMood();
    }

    public void SubtractFromGardenStats(Player.PlayerEnum player, MoodAtributes moodAtributes)
    {
        switch (player)
        {
            case Player.PlayerEnum.Player0:
                gardenMood1 -= moodAtributes;
                break;
            case Player.PlayerEnum.Player1:
                gardenMood2 -= moodAtributes;
                break;
        }
        DisplayCurrentGardenMood();
    }

  

    void GetUnpleasantPleasantText()
    {

        // set player 1 Unpleasant/Pleasant text        
        if (garden1MoodUnpleasantPleasant < 0)
        {
            plesanceDisplayP1.text = $"Unpleasant: {Mathf.Abs(garden1MoodUnpleasantPleasant).ToString()} <sprite=3>";
        }
        else if (garden1MoodUnpleasantPleasant > 0)
        {
            plesanceDisplayP1.text = $"Pleasant: {Mathf.Abs(garden1MoodUnpleasantPleasant).ToString()} <sprite=2>";
        }
        else if (garden1MoodUnpleasantPleasant == 0)
        {
            plesanceDisplayP1.text = $"Neutral: {Mathf.Abs(garden1MoodUnpleasantPleasant).ToString()} <sprite=1>";
        }

        // set player 2 Unpleasant/Pleasant text        
        if (garden2MoodUnpleasantPleasant < 0)
        {
            plesanceDisplayP2.text = $"Unpleasant: {Mathf.Abs(garden2MoodUnpleasantPleasant).ToString()} <sprite=3>";
        }
        else if (garden2MoodUnpleasantPleasant > 0)
        {
            plesanceDisplayP2.text = $"Pleasant: {Mathf.Abs(garden2MoodUnpleasantPleasant).ToString()} <sprite=2>";
        }
        else if (garden2MoodUnpleasantPleasant == 0)
        {
            plesanceDisplayP2.text = $"Neutral: {Mathf.Abs(garden2MoodUnpleasantPleasant).ToString()} <sprite=1>";
        }
    }

    void GetPersonalSocialText()
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

    void GetCalmEnergisedText()
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

        P1UnpleasantPleasantTextDisplay
        P2UnpleasantPleasantTextDisplay
        GetUnpleasantPleasantText();
        GetPersonalSocialText();
        GetCalmEnergisedText();

        EventsManager.InvokeEvent(EventsManager.EventType.UpdateScore);
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

    public Dictionary<Player.PlayerEnum, MoodAtributes> GetMoodValuesGardens()
    {
        Dictionary<Player.PlayerEnum, MoodAtributes> gardenMoodarray;

        gardenMoodarray = new Dictionary<Player.PlayerEnum, MoodAtributes>();

        gardenMoodarray[Player.PlayerEnum.Player0] = gardenMood1;
        gardenMoodarray[Player.PlayerEnum.Player1] = gardenMood2;

        return gardenMoodarray;
    }

}
