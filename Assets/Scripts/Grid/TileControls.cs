using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script created by Alexander Purvis 05/02/2021

public class TileControls : MonoBehaviour
{
    // sprites that reflect the current state of this tile
    public Sprite tileActive;
    public Sprite tileDefault;
    public Sprite tileOccupied;
    public Sprite tileBlocked;

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
        this.gameObject.GetComponent<SpriteRenderer>().sprite = tileActive;
        // sets the tile state to tile
        curentState = tileStates.Active;
        timePassedForActiveTiles = 0.0f;
    }

    public void FreeTile()
    {
        // switches out the sprite to reflect the that this tile is free
        this.gameObject.GetComponent<SpriteRenderer>().sprite = tileDefault;
        // sets the tile state to open
        curentState = tileStates.Open;
    }

    public void TileDenied()
    {
        // switches out the sprite to reflect the that this tile is occupied
        this.gameObject.GetComponent<SpriteRenderer>().sprite = tileBlocked;
        // sets the tile state to taken
        curentState = tileStates.Blocked;
        timePassedForOccupiedTiles = 0.0f;
    }

    public void TileOccupied()
    {
        // switches out the sprite to reflect the that this tile is occupied
        this.gameObject.GetComponent<SpriteRenderer>().sprite = tileOccupied;
        // sets the tile state to taken
        curentState = tileStates.Occupied;     
    }
}
