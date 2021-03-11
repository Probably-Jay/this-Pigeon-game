using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// created by Alexander Purvis 04/03
// Edited SJ 10/03
// Edited again Jay 10/03 

public class CurrentMood : MonoBehaviour // re-named from DisplayManager
{
 

    MoodAtributes gardenMood1 = new MoodAtributes(0,0,0);
    MoodAtributes gardenMood2 = new MoodAtributes(0, 0, 0);

    TMP_Text displayText;

   


    public TMP_Text P1PlesanceDisplay;
    public TMP_Text P1SociabilityDisplay;
    public TMP_Text P1EnergyTextDisplay;

    public TMP_Text P2PlesanceDisplay;
    public TMP_Text P2SociabilityDisplay;
    public TMP_Text P2EnergyTextDisplay;



 


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

  
    void DisplayCurrentGardenMood()
    {
        displayText.text = $"P1:\n\nP2:";

        P1PlesanceDisplay.text = gardenMood1.GetDisplayWithImage(MoodAtributes.Scales.Pleasance);
        P2PlesanceDisplay.text = gardenMood2.GetDisplayWithImage(MoodAtributes.Scales.Pleasance);

        P1SociabilityDisplay.text = gardenMood1.GetDisplayWithImage(MoodAtributes.Scales.Sociability);
        P2SociabilityDisplay.text = gardenMood2.GetDisplayWithImage(MoodAtributes.Scales.Sociability);

        P1EnergyTextDisplay.text = gardenMood1.GetDisplayWithImage(MoodAtributes.Scales.Energy);
        P2EnergyTextDisplay.text = gardenMood2.GetDisplayWithImage(MoodAtributes.Scales.Energy);

        EventsManager.InvokeEvent(EventsManager.EventType.UpdateScore);
    }

   
    public Dictionary<Player.PlayerEnum, MoodAtributes> GetMoodValuesGardens()
    {
        Dictionary<Player.PlayerEnum, MoodAtributes> gardenMoodarray;

        gardenMoodarray = new Dictionary<Player.PlayerEnum, MoodAtributes>();

        gardenMoodarray[Player.PlayerEnum.Player0] = gardenMood1;
        gardenMoodarray[Player.PlayerEnum.Player1] = gardenMood2;

        return gardenMoodarray;
    }

}
