using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Scott Jarvis, 16/02/2021

// Saves data to external file - can be added to later to store information about garden beyond "big mood"
public class GardenInfo
{
    public int bigMoodGoal;
    
}

public class getDropdownValue : MonoBehaviour
{

    //Attach to Dropdown GameObject
    Dropdown dropdownObj;

    //Attach to Camera GameObject
    Camera cameraObj;

    //This is the index value of the Dropdown
    static int dropdownIndex;

    void Start()
    {
        //Fetch the DropDown component from the GameObject
        dropdownObj = GetComponent<Dropdown>();
        cameraObj = GetComponent<Camera>();
    }

    void Update()
    {
        //Keep the current index of the Dropdown in a variable
        dropdownIndex = dropdownObj.value;
        Debug.Log("Dropdown Value : " + dropdownIndex);

        Color color;

        switch (dropdownIndex) {
            case 0: color = Color.yellow; break; // Pride
            case 1: color = Color.green; break; // Care
            case 2: color = Color.blue; break; // Calm
            default: color = Color.red; break;
        }

        cameraObj.backgroundColor = color;
    }
}
