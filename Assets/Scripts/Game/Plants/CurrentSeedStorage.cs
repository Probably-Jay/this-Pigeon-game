using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentSeedStorage : MonoBehaviour
{
    public GameObject CurrentPlant;
    public bool isStoringSeed = false;


    public GameObject GetCurrentPlant()
    {
        return CurrentPlant; 
    }

    public void SetCurrentPlant(GameObject plant)
    {
        CurrentPlant = plant;
    }
}
