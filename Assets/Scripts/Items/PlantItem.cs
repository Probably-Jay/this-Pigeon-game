using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created Jay 05/02
// Edited Scott 24/02
// Edited Alexander Purvis 04/03
// Added plant enum Jay 13/03

/// <summary>
/// Script all plants will have
/// </summary>
public class PlantItem : MonoBehaviour
{

    public enum PlantName
    {
        Rine
        ,Vlum
        ,Adrinque
        ,Zove
        ,Vlufraisy
        ,Eisower
        ,Phess
        ,Brovlary
        ,Aesron
        ,Phodetta
    }


   // public string objectName;
    public PlantName plantname;

    [SerializeField, Range(-1, 1)] private int pleasance = 0;
    [SerializeField, Range(-1, 1)] private int sociability = 0;
    [SerializeField, Range(-1, 1)] private int energy = 0;

    public Player plantOwner;
    public bool inLocalGarden;


   // public bool isPlanted = false;
    public Player.PlayerEnum gardenID = Player.PlayerEnum.Unnasigned;


    private void OnEnable() // todo fix this
    {
        // Get current player
        plantOwner = GameManager.Instance.ActivePlayer; // Load system will break here

        // Set if in local or other garden
        if (plantOwner.PlayerEnumValue == 0)
        { // = true if local (placed by player 1)
            inLocalGarden = true;
        }
        else
        { // = false if not
            inLocalGarden = false;
        }

    }

   
    public MoodAtributes PlantStats => new MoodAtributes(pleasance, sociability, energy);
}

