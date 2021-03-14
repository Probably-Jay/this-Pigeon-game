using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// jay 13/03


public class DataCollector : MonoBehaviour
{

    private LiveGameData data;


    public void AddPlant(PlantItem plant)
    {
        data.plants.Add(plant);
    }
    

    //public static SaveGameData ConvertLiveDataToSaveableForm(LiveGameData liveData)
    //{

    //}  
    
    //public static SaveGameData ConvertSaveDataToLiveForm(LiveGameData liveData)
    //{

    //}

}
