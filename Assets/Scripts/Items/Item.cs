﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created Jay 05/02
// Edited Scott 24/02
// Edited Alexander Purvis 04/03

/// <summary>
/// Script all items will have
/// </summary>
public class Item : MonoBehaviour
{
    [SerializeField] public string objectName;

    [SerializeField, Range(-1, 1)] public int scale1UnpleasantPleasant = 0;
    [SerializeField, Range(-1, 1)] public int scale2PersonalSocial = 0;
    [SerializeField, Range(-1, 1)] public int scale3CalmEnergised = 0;

    public Player plantOwner;
    public bool inLocalGarden;


    public bool isPlanted = false;
    public int gardenID = 0;


    private void OnEnable()
    {
        // Get current player
        plantOwner = GameManager.Instance.ActivePlayer;

        // Set if in local or other garden
        if (plantOwner.PlayerEnumValue == 0)
        { // = true if local (placed by player 1)
            inLocalGarden = true;
        }
        else
        { // = false if not
            inLocalGarden = false;
        }

       // Debug.Log("Plant is in Garden #" + inLocalGarden);
    }

   
    public Vector3 PlantStats => new Vector3(scale1UnpleasantPleasant, scale2PersonalSocial, scale3CalmEnergised);
}
