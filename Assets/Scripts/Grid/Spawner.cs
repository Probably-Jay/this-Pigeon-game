using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject newTile;
  

    public GameObject SpawnTile()
    {
        newTile = Instantiate(tilePrefab, this.transform.position, this.transform.rotation);
        return newTile;
    }
}
