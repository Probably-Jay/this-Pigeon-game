using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// created Scott 18/02
// Class to handle items in garden (mostly plants)
// Updated 24/02


[System.Obsolete("This class has been replaced by " + nameof(DisplayManager) +" and " + nameof(GoalManagerScript) ,true)]
public class PlantManager : MonoBehaviour
{
    public List<Item>[] gardenPlants = new List<Item>[2]; // Holds both gardens in same var


    // todo make this a little neater
    /*[SerializeField]*/
    int goalMoodPride;
    /*[SerializeField]*/
    int goalMoodEnergy;
    /*[SerializeField]*/
    //int goalMoodSocial;
    //GameManager.Goal goal;

    //public bool gameWon = false;
    //// public int curMoodPride;
    //// public int curMoodEnergy;
    //// public int curMoodSocial;

    //public int[,] curMood = new int[2, 3];
    //public int[,] goalMood = new int[2, 3];

    //Text text;

    //private void Awake()
    //{
    //    for (int i = 0; i < 2; i++)
    //    {
    //        gardenPlants[i] = new List<Item>();
    //    }
    //    text = GetComponent<Text>();
    //         if (GameManager.Instance.CurrentGoal == GameManager.Goal.Anxious) { goalMood[0, 0] = 4; goalMood[0, 1] = 0; goalMood[0, 2] = 7; }
    //    else if (GameManager.Instance.CurrentGoal == GameManager.Goal.Content) { goalMood[0, 0] = 0; goalMood[0, 1] = 5; goalMood[0, 2] = 5; }
    //    else if (GameManager.Instance.CurrentGoal == GameManager.Goal.Proud)   { goalMood[0, 0] = 3; goalMood[0, 1] = 10; goalMood[0, 2] = 0; }

    //         if (GameManager.Instance.AlternateGoal == GameManager.Goal.Anxious) { goalMood[1, 0] = 4; goalMood[1, 1] = 0; goalMood[1, 2] = 7; }
    //    else if (GameManager.Instance.AlternateGoal == GameManager.Goal.Content) { goalMood[1, 0] = 0; goalMood[1, 1] = 5; goalMood[1, 2] = 5; }
    //    else if (GameManager.Instance.AlternateGoal == GameManager.Goal.Proud)   { goalMood[1, 0] = 3; goalMood[1, 1] = 10; goalMood[1, 2] = 0; }
    //}

    //// Start is called before the first frame update
    //void Start()
    //{
    //}

    //public int[,] getGardenMood() {
    //    return curMood;
    //}

    //void Update()
    //{
    //    if (!gameWon)
    //    {
    //        UpdateList();
    //        if (text)
    //        {
    //            Player.PlayerEnum turnPlayer;
    //            GameManager.Goal turnGoal;
    //            turnPlayer = GameManager.Instance.ActivePlayer.PlayerEnumValue;

    //            if (turnPlayer == 0)
    //            {
    //                turnGoal = GameManager.Instance.CurrentGoal;
    //            }
    //            else
    //            {
    //                turnGoal = GameManager.Instance.AlternateGoal;
    //            }

    //            text.text = $"Current Mood Values:\n" +
    //                        $"Garden 1: P {curMood[0, 0].ToString()},  E {curMood[0, 1].ToString()},  S {curMood[0, 2].ToString()}, \n" +
    //                        $"Garden 2: P {curMood[1, 0].ToString()},  E {curMood[1, 1].ToString()},  S {curMood[1, 2].ToString()}, \n" +
    //                        $"Mood Goal: {turnGoal.ToString()}";
    //        }
    //        UpdateScore();
    //    }
    //}

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
        Debug.Log("Mood (Energy): " + gardenPlants[0][i].moodEnergy);
        Debug.Log("Mood (Social): " + gardenPlants[0][i].moodSocial);
        Debug.Log("Mood (Pride): " + gardenPlants[0][i].moodPleasant);
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
        //for (int c = 0; c < 2; c++)
        //{
        //    int tempEnergy = 0;
        //    int tempSocial = 0;
        //    int tempPride = 0;


        //    for (int x = 0; x < gardenPlants[c].Count; x++)
        //    {
        //        tempEnergy += gardenPlants[c][x].moodEnergy;
        //        tempSocial += gardenPlants[c][x].moodSocial;
        //        tempPride += gardenPlants[c][x].moodPleasant;
        //    };

        //    curMood[c, 0] = tempEnergy;
        //    curMood[c, 1] = tempSocial;
        //    curMood[c, 2] = tempPride;

        //}

        //if (curMood == goalMood || Input.GetKeyDown("space"))
        //{
        //    gameWon = true;
        //    // Add actual user feedback, end condition here later
        //    // Right now, both gardens share same mood goal - will be changed
        //}
    }
}
