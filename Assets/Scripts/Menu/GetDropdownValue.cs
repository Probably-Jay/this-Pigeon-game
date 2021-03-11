using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Scott Jarvis, 16/02/2021



public class GetDropdownValue : MonoBehaviour
{

    //Attach to Dropdown GameObject
    Dropdown dropdownObj;

    //Background image to be dragged in
    [SerializeField] Image background;
    [SerializeField] Color prideColour;
    [SerializeField] Color anxiosColour;
    [SerializeField] Color contentColour;

    // Which Garden are you choosing a mood goal for?
    // Only temporary, as when we switch from hotseat play to networked play you won't have multiple players on the same instance of the game
    [SerializeField] bool gardenToggle;

    //This is the index value of the Dropdown
    static int dropdownIndex;

    void Start()
    {
        //Fetch the DropDown component from the GameObject
        dropdownObj = GetComponent<Dropdown>();
    }

    void Update()
    {
        //Keep the current index of the Dropdown in a variable
        dropdownIndex = dropdownObj.value;

        Color color;

        switch (dropdownIndex) {
            case 0: color = prideColour; break; // Pride
            case 1: color = anxiosColour; break; // Care
            case 2: color = contentColour; break; // Calm
            default: color = Color.red; break;
        }

        background.color = color;
    }
}

// Saves data to external file - can be added to later to store information about garden beyond "big mood"
//public class GardenInfo
//{
//    public int bigMoodGoal;

//}