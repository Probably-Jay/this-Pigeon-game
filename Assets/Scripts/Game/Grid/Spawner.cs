using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script created by Alexander Purvis 03/02/2021
public class Spawner : MonoBehaviour
{
    // allows the tile prefab to be added to this script in the edditor
    public GameObject tilePrefab;
    private GameObject newTile;
  

    public GameObject SpawnTile(GameObject parent)
    {
        // instantiates a tile with the position and rotation of the spawner
        newTile = Instantiate(tilePrefab, this.transform.position, this.transform.rotation);
        newTile.transform.SetParent(parent.transform);
        // returns the new tile so that it can be added to an array of all tiles that make up the grid
        return newTile;
    }
}
