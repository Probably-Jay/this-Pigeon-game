using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// created Scott 18/02
// Class to handle items in garden (mostly plants)

public class PlantManager : MonoBehaviour
{
    public List<Item> gardenPlants = new List<Item>();

    [SerializeField] int goalMoodPride;
    [SerializeField] int goalMoodEnergy;
    [SerializeField] int goalMoodSocial;

    public int curMoodPride;
    public int curMoodEnergy;
    public int curMoodSocial;

    Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateList();
        //LogAllPlants();
        UpdateScore();
    }

    void Update() {
        UpdateScore();
        UpdateList();
        if (text) { 
            text.text = $"Current Mood Values: P {curMoodPride.ToString()},  E {curMoodEnergy.ToString()},  S {curMoodSocial.ToString()}, \n Goal: P {goalMoodPride.ToString()},  E {goalMoodEnergy.ToString()},  S {goalMoodSocial.ToString()}";
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

    private void LogPlant(int i) {
        Debug.Log("Item #" + i + ":" + gardenPlants[i].objectName);
        Debug.Log("Mood (Energy): " + gardenPlants[i].moodEnergy);
        Debug.Log("Mood (Social): " + gardenPlants[i].moodSocial);
        Debug.Log("Mood (Pride): " + gardenPlants[i].moodPride);
    }

    private void LogAllPlants()
    {
        for (int i = 0; i < gardenPlants.Count; i++)
        {
            LogPlant(i);
        };
    }

    public void UpdateList()
    {
        GetPlants();
    }

    private void GetPlants()
    {
        var foundPlants = FindObjectsOfType<Item>();
        gardenPlants.Clear();
        for (int i = 0; i < foundPlants.Length; i++) {
            gardenPlants.Add(foundPlants[i]);
        };
    }

    public void UpdateScore() {

        curMoodEnergy = 0;
        curMoodSocial = 0;
        curMoodPride = 0;

        for (int i = 0; i < gardenPlants.Count; i++)
        {
            curMoodEnergy += gardenPlants[i].moodEnergy;
            curMoodSocial += gardenPlants[i].moodSocial;
            curMoodPride += gardenPlants[i].moodPride;
        };

        if ((curMoodEnergy == goalMoodEnergy) && (curMoodPride == goalMoodPride) && (curMoodSocial == goalMoodSocial)) {
            Debug.Log("Mood Condition Met!");
            // Add actual user feedback here
        }
    }
}
