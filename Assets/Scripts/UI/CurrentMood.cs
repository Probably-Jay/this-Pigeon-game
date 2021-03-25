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
 

    MoodAttributes gardenMood1 = new MoodAttributes(0,0,0);
    MoodAttributes gardenMood2 = new MoodAttributes(0, 0, 0);

    TMP_Text displayText;

   
    public TMP_Text P1PleasanceDisplay;
    public TMP_Text P1SociabilityDisplay;
    public TMP_Text P1EnergyTextDisplay;

    public TMP_Text P2PleasanceDisplay;
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

   
    public void AddToGardenStats(Player.PlayerEnum player, MoodAttributes moodAttributes)
    {
        switch (player)
        {
            case Player.PlayerEnum.Player0:
                gardenMood1 += moodAttributes;
                break;
            case Player.PlayerEnum.Player1:
                gardenMood2 += moodAttributes;
                break;
        }
        DisplayCurrentGardenMood();
    }

    public void SubtractFromGardenStats(Player.PlayerEnum player, MoodAttributes moodAtributes)
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

        P1PleasanceDisplay.text = gardenMood1.GetDisplayWithImage(MoodAttributes.Scales.Pleasance);
        P2PleasanceDisplay.text = gardenMood2.GetDisplayWithImage(MoodAttributes.Scales.Pleasance);

        P1SociabilityDisplay.text = gardenMood1.GetDisplayWithImage(MoodAttributes.Scales.Sociability);
        P2SociabilityDisplay.text = gardenMood2.GetDisplayWithImage(MoodAttributes.Scales.Sociability);

        P1EnergyTextDisplay.text = gardenMood1.GetDisplayWithImage(MoodAttributes.Scales.Energy);
        P2EnergyTextDisplay.text = gardenMood2.GetDisplayWithImage(MoodAttributes.Scales.Energy);

        EventsManager.InvokeEvent(EventsManager.EventType.UpdateScore);
    }

   
    public Dictionary<Player.PlayerEnum, MoodAttributes> GetMoodValuesGardens()
    {
        Dictionary<Player.PlayerEnum, MoodAttributes> gardenMoodarray;

        gardenMoodarray = new Dictionary<Player.PlayerEnum, MoodAttributes>();

        gardenMoodarray[Player.PlayerEnum.Player0] = gardenMood1;
        gardenMoodarray[Player.PlayerEnum.Player1] = gardenMood2;

        return gardenMoodarray;
    }

}
