using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script created by Alexander Purvis 05/02/2021

public class TileControls : MonoBehaviour
{
    // sprites that reflect the current state of this tile
    public Sprite TileLightBorder01;
    public Sprite TileLightBorder02;
    public Sprite TileLightBorder03;
    public Sprite TileLightBorder04;
    public Sprite TileLightBorder05;

    // enums that control the state of this tile
    public enum tileStates
    {
        Active,
        Occupied,
        Open,
        Blocked
    };

    // sets the current state to open as default
    public tileStates curentState = tileStates.Open;

    // to change the alpha of the object that is moving
    SpriteRenderer objectsSprite;
    Color objectColourValues;


    public int gridID = 0;
    public int thisTilesRow;
    public int thisTilesColumn;

    // used time space out the raycast that are used track the tiles that this object is hovering over
    float timePassedForActiveTiles = 0;
    float timePassedForOccupiedTiles = 0;
    float triggerTime = 0.1f;


    private void Update()
    {
        if (curentState == tileStates.Active)
        {
            if (timePassedForActiveTiles >= triggerTime)
            {
                FreeTile();
            }
            else
            {
                timePassedForActiveTiles += Time.deltaTime;            
            }
        }

        if (curentState == tileStates.Blocked)
        {
            if (timePassedForOccupiedTiles >= triggerTime)
            {
                FreeTile();
            }
            else
            {              
                timePassedForOccupiedTiles += Time.deltaTime;
            }
        }
    }

    public void ActivateTile()
    {
        // switches out the sprite to reflect the that this tile is now active
        //this.gameObject.GetComponent<SpriteRenderer>().sprite = tileActive;

        objectsSprite = this.GetComponent<SpriteRenderer>();
        objectColourValues = objectsSprite.material.color;

        objectColourValues.r = 0.3f;
        objectColourValues.g = 1.5f;
        objectColourValues.b = 0.1f;

        objectsSprite.material.color = objectColourValues;

        // sets the tile state to tile
        curentState = tileStates.Active;
        timePassedForActiveTiles = 0.0f;
    }

    public void FreeTile()
    {
        objectsSprite = this.GetComponent<SpriteRenderer>();
        objectColourValues = objectsSprite.material.color;

        objectColourValues.r = 1.0f;
        objectColourValues.g = 1.0f;
        objectColourValues.b = 1.0f;

        objectsSprite.material.color = objectColourValues;


        // switches out the sprite to reflect the that this tile is free
        //this.gameObject.GetComponent<SpriteRenderer>().sprite = tileDefault;
        // sets the tile state to open
        curentState = tileStates.Open;
    }

    public void TileDenied()
    {
        // switches out the sprite to reflect the that this tile is occupied
        // this.gameObject.GetComponent<SpriteRenderer>().sprite = tileBlocked;

        objectsSprite = this.GetComponent<SpriteRenderer>();
        objectColourValues = objectsSprite.material.color;

        objectColourValues.r = 2.5f;
        objectColourValues.g = 0.1f;
        objectColourValues.b = 0.0f;

        objectsSprite.material.color = objectColourValues;

        // sets the tile state to taken
        curentState = tileStates.Blocked;
        timePassedForOccupiedTiles = 0.0f;
    }

    public void TileOccupied()
    {
        // switches out the sprite to reflect the that this tile is occupied
        //this.gameObject.GetComponent<SpriteRenderer>().sprite = tileOccupied;
        objectsSprite = this.GetComponent<SpriteRenderer>();
        objectColourValues = objectsSprite.material.color;

        objectColourValues.r = 3.5f;
        objectColourValues.g = 1.0f;
        objectColourValues.b = 0.1f;

        objectsSprite.material.color = objectColourValues;

        // sets the tile state to taken
        curentState = tileStates.Occupied;     
    }

    public void SetSprite(int SpriteNumber)
    {

        switch (SpriteNumber)
        {
            case 0:
                this.gameObject.GetComponent<SpriteRenderer>().sprite = TileLightBorder01;
                break;
            case 1:
                this.gameObject.GetComponent<SpriteRenderer>().sprite = TileLightBorder02;
                break;
            case 2:
                this.gameObject.GetComponent<SpriteRenderer>().sprite = TileLightBorder03;
                break;
            case 3:
                this.gameObject.GetComponent<SpriteRenderer>().sprite = TileLightBorder04;
                break;
            case 4:
                this.gameObject.GetComponent<SpriteRenderer>().sprite = TileLightBorder05;
                break;
            default:

                break;

        }
    }
}
