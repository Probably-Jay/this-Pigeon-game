using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentSeedStorage : MonoBehaviour
{
    public GameObject CurrentPlant;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public GameObject GetCurrentPlant()
    {
        return CurrentPlant; 
    }
    public void SetCurrentPlant(GameObject plant)
    {
        CurrentPlant = plant;
    }
}
