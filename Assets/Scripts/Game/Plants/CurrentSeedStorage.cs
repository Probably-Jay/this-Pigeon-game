using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentSeedStorage : MonoBehaviour
{
    public GameObject CurrentPlant { get; private set; }
    public bool isStoringSeed => CurrentPlant != null;


    public GameObject GetCurrentPlant()
    {
        return CurrentPlant; 
    }

    public void SetCurrentPlant(GameObject plant)
    {
        CurrentPlant = plant;
    }    
    
    public void ClearCurrentPlant()
    {
        CurrentPlant = null;
    }
}
