using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script created by Alexander Purvis 05/02/2021

public class TileControls : MonoBehaviour
{
    // sprites that reflect the current state of this tile
    public Sprite tileActive;
    public Sprite tileDefault;
    public Sprite tileTaken;

    // enums that control the state of this tile
    public enum tileStates
    {
        ACTIVE,
        TAKEN,
        OPEN
    };

    // sets the current state to open as default
    public tileStates curentState = tileStates.OPEN;

  
    public void ActivateTile()
    {
        // switches out the sprite to reflect the that this tile is now active
        this.gameObject.GetComponent<SpriteRenderer>().sprite = tileActive;
        // sets the tile state to tile
        curentState = tileStates.ACTIVE;
    }

    public void FreeTile()
    {
        // switches out the sprite to reflect the that this tile is free
        this.gameObject.GetComponent<SpriteRenderer>().sprite = tileDefault;
        // sets the tile state to open
        curentState = tileStates.OPEN;
    }

    public void OccupyTile()
    {
        // switches out the sprite to reflect the that this tile is occupied
        this.gameObject.GetComponent<SpriteRenderer>().sprite = tileTaken;
        // sets the tile state to taken
        curentState = tileStates.TAKEN;
    }
}
