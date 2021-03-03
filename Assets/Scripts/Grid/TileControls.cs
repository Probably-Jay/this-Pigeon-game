using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script created by Alexander Purvis 05/02/2021

public class TileControls : MonoBehaviour
{

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

    private void Awake()
    {
        objectsSprite = this.GetComponent<SpriteRenderer>();
       
        objectColourValues = objectsSprite.material.color;

        objectColourValues.a = 0.6f;
 

        objectsSprite.material.color = objectColourValues;
    }


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
       
        objectColourValues = objectsSprite.material.color;

        objectColourValues.r = 1.0f;
        objectColourValues.g = 1.0f;
        objectColourValues.b = 1.0f;

        objectsSprite.material.color = objectColourValues;

        // switches out the sprite to reflect the that this tile is free
        // this.gameObject.GetComponent<SpriteRenderer>().sprite = tileDefault;
        // sets the tile state to open
        curentState = tileStates.Open;
    }

    public void TileDenied()
    {
        // switches out the sprite to reflect the that this tile is occupied
        // this.gameObject.GetComponent<SpriteRenderer>().sprite = tileBlocked;

       
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
       
        objectColourValues = objectsSprite.material.color;

        objectColourValues.r = 3.5f;
        objectColourValues.g = 1.0f;
        objectColourValues.b = 0.1f;

        objectsSprite.material.color = objectColourValues;

        // sets the tile state to taken
        curentState = tileStates.Occupied;     
    }
}
