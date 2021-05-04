using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Plants;

public class SeedIndicator : MonoBehaviour
{

    public GameObject SeedSelected;
    
    // to change the alpha of the sprites
    SpriteRenderer IndicatorSprite;
    SpriteRenderer SeedSelectedSprite;
    Color slotColourValues;

    public List<Sprite> SeedPackets;

    Plant plantSelected;

    private void Awake()
    {

        // establish variables for indicator 
        IndicatorSprite = this.GetComponent<SpriteRenderer>();
        slotColourValues = IndicatorSprite.material.color;
        slotColourValues.a = 0.0f;
        IndicatorSprite.material.color = slotColourValues;


        // establish variables for the selected seed packet 
        SeedSelectedSprite = SeedSelected.GetComponent<SpriteRenderer>();
        slotColourValues = SeedSelectedSprite.material.color;
        slotColourValues.a = 0.0f;
        SeedSelectedSprite.material.color = slotColourValues;
    }


    public void ShowIndicator(GameObject selectedPlant)
    {
        // Adrinque = 0, Rhine = 1, Eisower = 2, Vlufraisy = 3, Zove = 4, Vlum =5
        plantSelected = selectedPlant.GetComponent<Plant>();

        switch (plantSelected.plantname)
        {
            case Plant.PlantName.Adrinque:
                SeedSelectedSprite.sprite = SeedPackets[0];
                break;
            case Plant.PlantName.Rine:
                SeedSelectedSprite.sprite = SeedPackets[1];
                break;
            case Plant.PlantName.Eisower:
                SeedSelectedSprite.sprite = SeedPackets[2];
                break;
            case Plant.PlantName.Vlufraisy:
                SeedSelectedSprite.sprite = SeedPackets[3];
                break;
            case Plant.PlantName.Zove:
                SeedSelectedSprite.sprite = SeedPackets[4];
                break;
            case Plant.PlantName.Vlum:
                SeedSelectedSprite.sprite = SeedPackets[5];
                break;
            default:             
                break;
        }


        slotColourValues = IndicatorSprite.material.color;
        slotColourValues.a = 1.0f;
        IndicatorSprite.material.color = slotColourValues;

        slotColourValues = SeedSelectedSprite.material.color;
        slotColourValues.a = 1.0f;
        SeedSelectedSprite.material.color = slotColourValues;
    }


    public void HideIndicator()
    {
        slotColourValues = IndicatorSprite.material.color;
        slotColourValues.a = 0.0f;
        IndicatorSprite.material.color = slotColourValues;

        slotColourValues = SeedSelectedSprite.material.color;
        slotColourValues.a = 0.0f;
        SeedSelectedSprite.material.color = slotColourValues;
    }
}
