using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created Jay 05/02
// Edited Scott 24/02
// Edited Alexander Purvis 04/03
// Added plant enum Jay 13/03
// Added stages of growth Scott 24/03

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

    public enum PlantSize // ToDo Later
    {
        Wide,
        Tall,
        Single
    }

    public enum PlantGrowthStage { 
        Seed,
        Sprout,
        Bloom
    }


    // public string objectName;
    public PlantName plantname;

    public PlantGrowthStage plantGrowthState = PlantGrowthStage.Seed;
    public int moodMult = 0;

    [SerializeField, Range(-1, 1)] private int pleasance = 0;
    [SerializeField, Range(-1, 1)] private int sociability = 0;
    [SerializeField, Range(-1, 1)] private int energy = 0;

    [SerializeField] public int growthGoal = 1;
    public int currGrowth = 0;

    public Player plantOwner;
    public bool inLocalGarden;

    //Get the Renderer component for changing colours (temp, replace with actual different sprites later)
    Renderer matRenderer;

    // public bool isPlanted = false;
    public Player.PlayerEnum gardenID = Player.PlayerEnum.Unassigned;


    private void OnEnable() // hack, todo fix this
    {
        matRenderer = GetComponent<Renderer>();

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

    public void UpdateSprite() {

        switch (plantGrowthState) {
            case PlantGrowthStage.Seed:
                matRenderer.material.SetColor("_Color", Color.red); 
                break;

            case PlantGrowthStage.Sprout:
                matRenderer.material.SetColor("_Color", Color.yellow); 
                break;

            case PlantGrowthStage.Bloom:
                matRenderer.material.SetColor("_Color", Color.green); 
                break;
        }

    }
   
    public MoodAttributes PlantStats => new MoodAttributes(pleasance * moodMult, sociability * moodMult, energy * moodMult);
}

