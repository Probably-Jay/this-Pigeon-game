using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TheGrid
{   
   private GameObject[,] tileGrid;
   public Spawner spawner = GameObject.FindObjectOfType<Spawner>();

    public TheGrid(Vector3 startingPosition, float tilesize, int gridColumns, int gridRows)
    {    
        tileGrid = new GameObject[gridColumns, gridRows];

        for (int row = 0; row < gridRows; row++)
        {
            for (int column = 0; column < gridColumns; column++)
            {
                spawner.transform.position = new Vector3(startingPosition.x + (tilesize * column), startingPosition.y - (tilesize * row), startingPosition.z); 

                tileGrid[column, row] = spawner.SpawnTile();
            }
        }
    }
}
