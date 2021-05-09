using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Script created by Alexander Purvis 04/05/2021

public class MoodSelectionButtons : MonoBehaviour
{
    public GameObject controller;
    MoodSelectController moodControls;
    public GameObject buttonManager;
    ButtonManager buttonListControls;

    public int buttonID_MoodValue;
    public int selectedColourR;
    public int selectedColourG;
    public int selectedColourB;


    Color32 originalColour;
    // Start is called before the first frame update
    void Start()
    {
        moodControls = controller.GetComponent<MoodSelectController>();
        buttonListControls = buttonManager.GetComponent<ButtonManager>();
        originalColour = GetComponent<Image>().color;
    }

    public void SetMoodChoice()
    {
        moodControls.playersMoodChoice = buttonID_MoodValue;      
        ChangeToSelectedColour();
    }

    void ChangeToSelectedColour()
    {
        GetComponent<Image>().color = new Color32((byte)selectedColourR, (byte)selectedColourG, (byte)selectedColourB, 255);
        buttonListControls.ChangeMoodSelection(buttonID_MoodValue);
    }

    public void resetColour()
    {
        GetComponent<Image>().color = originalColour;
    }
}
