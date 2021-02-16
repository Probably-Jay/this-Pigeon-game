using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script created by Alexander Purvis 03/02/2021

[System.Serializable]
public class TheGrid
{
   // a two dimensional array of game objects that will store individual tiles. Note: Tiles should probibly be stored outside this script in future
   private GameObject[,] tileGrid;
    // stores the spawner using the find object of type fuction every time a new grid is made
   public Spawner spawner = GameObject.FindObjectOfType<Spawner>();

    // theGrid fuction  is called by other script whenever a new grid is required
    // it is passed a Vector 3 to know where to start spawning the tiles that make up the grid, a size for the tiles and both the number of rows and columns that make up the grid
    public TheGrid(Vector3 startingPosition, float tilesize, int gridColumns, int gridRows)
    {    
        // creates a new array that is the size fo the new grid to store the new grid
        tileGrid = new GameObject[gridColumns, gridRows];

        // nested for loop that will run for the number of rows and colums 
        for (int row = 0; row < gridRows; row++)
        {
            for (int column = 0; column < gridColumns; column++)
            {
                // moves the spawner to the starting position then up or down as necessary for that tile.
                spawner.transform.position = new Vector3(startingPosition.x + (tilesize * column), startingPosition.y - (tilesize * row), startingPosition.z); 

                // calls spawnTile on the spawner then stores that new tile in the grid array
                tileGrid[column, row] = spawner.SpawnTile();
            }
        }
    }
}
