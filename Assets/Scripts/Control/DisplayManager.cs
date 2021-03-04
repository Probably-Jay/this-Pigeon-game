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

    int currentGarden2MoodPride = 0;
    int currentGarden2MoodEnergy = 0;
    int currentGarden2Social = 0;

    Text displayText;

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
        displayText.text = $"Current Mood Values:\n" +
                       $"Garden 1: Unpleasant/Pleasant { currentGarden1MoodPride.ToString()},  Personal/Social  { currentGarden1MoodEnergy.ToString()},  Calm/ Energised { currentGarden1Social.ToString()}, \n" +
                        $"Garden 2: Unpleasant/Pleasant {currentGarden2MoodPride.ToString()},  Personal/Social {currentGarden2MoodEnergy.ToString()},   Calm/ Energised {currentGarden2Social.ToString()}, \n";     
   
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
