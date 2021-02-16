using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileControls : MonoBehaviour
{

    public Sprite tileActive;
    public Sprite tileDefault;
    public Sprite tileTaken;

    public enum tileStates
    {
        ACTIVE,
        TAKEN,
        OPEN
    };

    public tileStates curentState = tileStates.OPEN;

    public void ActivateTile()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = tileActive;
        curentState = tileStates.ACTIVE;
    }

    public void FreeTile()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = tileDefault;
        curentState = tileStates.OPEN;
    }

    public void OccupyTile()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = tileTaken;
        curentState = tileStates.TAKEN;
    }
}
