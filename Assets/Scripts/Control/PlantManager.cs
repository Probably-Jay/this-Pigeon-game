using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// created Scott 18/02
// Class to handle items in garden (mostly plants)
// Updated 24/02

public class PlantManager : MonoBehaviour
{
    public List<Item>[] gardenPlants = new List<Item>[2]; // Holds both gardens in same var

    
    // todo make this a little neater
    /*[SerializeField]*/
    int goalMoodPride;
    /*[SerializeField]*/
    int goalMoodEnergy;
    /*[SerializeField]*/
    int goalMoodSocial;
    GameManager.Goal goal;

    // public int curMoodPride;
    // public int curMoodEnergy;
    // public int curMoodSocial;

    public int[,] curMood = new int[2, 3];

    Text text;

    private void Awake()
    {
        for (int i = 0; i < 2; i++)
        {
            gardenPlants[i] = new List<Item>();
        }
        text = GetComponent<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
       
        UpdateList();
        //LogAllPlants();
        UpdateScore();
    }



    void Update()
    {
        UpdateScore();
        UpdateList();
        if (text)
        {
            text.text = $"Current Mood Values:\n" +
                        $"Garden 1: P {curMood[0, 0].ToString()},  E {curMood[0, 1].ToString()},  S {curMood[0, 2].ToString()}, \n" +
                        $"Garden 2: P {curMood[1, 0].ToString()},  E {curMood[1, 1].ToString()},  S {curMood[1, 2].ToString()}, \n" +
                        $"G1 Goal: {GameManager.Instance.CurrentGoal.ToString()}";
        }
    }

    private void OnEnable()
    {
        EventsManager.BindEvent(EventsManager.EventType.UpdateScore, UpdateScore);
        EventsManager.BindEvent(EventsManager.EventType.UpdatePlants, UpdateList);
    }

    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.UpdateScore, UpdateScore);
        EventsManager.UnbindEvent(EventsManager.EventType.UpdatePlants, UpdateList);
    }


    private void LogAllPlants()
    {
        for (int c = 0; c < 2; c++)
        {
            for (int i = 0; i < gardenPlants[c].Count; i++)
            {
                LogPlant(i);
            };
        };
    }

    private void LogPlant(int i)
    {
        Debug.Log("Item #" + i + ":" + gardenPlants[0][i].objectName);
        Debug.Log("Mood (Energy): "  + gardenPlants[0][i].moodEnergy);
        Debug.Log("Mood (Social): "  + gardenPlants[0][i].moodSocial);
        Debug.Log("Mood (Pride): "   + gardenPlants[0][i].moodPride);
    }

    public void UpdateList()
    {
        GetPlants();
    }


    // xander interface here
    private void GetPlants()
    {
        var foundPlants = FindObjectsOfType<Item>();

        gardenPlants[0].Clear();
        gardenPlants[1].Clear();

        for (int i = 0; i < foundPlants.Length; i++)
        {
            if (foundPlants[i].inLocalGarden == true)
            {
                gardenPlants[0].Add(foundPlants[i]);
            }
            else
            {
                gardenPlants[1].Add(foundPlants[i]);
            }
        };
    }


    public void UpdateScore()
    {

        for (int c = 0; c < 2; c++)
        {
            int tempEnergy = 0;
            int tempSocial = 0;
            int tempPride = 0;


            for (int x = 0; x < gardenPlants[c].Count; x++)
            {
                tempEnergy += gardenPlants[c][x].moodEnergy;
                tempSocial += gardenPlants[c][x].moodSocial;
                tempPride  += gardenPlants[c][x].moodPride;
            };

            curMood[c, 0] = tempEnergy;
            curMood[c, 1] = tempSocial;
            curMood[c, 2] = tempPride;
        }

        for (int n = 0; n < 2; n++) { 
            if ((curMood[n, 0] == goalMoodEnergy) && (curMood[n, 1] == goalMoodSocial) && (curMood[n, 2] == goalMoodPride))
            {
               // Debug.Log("Garden #" + n + "Mood Condition Met!");
                // Add actual user feedback here later
                // Right now, both gardens share same mood goal - will be changed
            }
        }
    }
}
