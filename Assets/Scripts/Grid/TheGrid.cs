using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script created by Alexander Purvis 03/02/2021

[System.Serializable]
public class TheGrid : MonoBehaviour   
{
   // a two dimensional array of game objects that will store individual tiles. Note: Tiles should probibly be stored outside this script in future
    [SerializeField] private GameObject[,] tileGrid;

    // stores the spawner using the find object of type fuction every time a new grid is made
    public Spawner spawner;
    // used to store all grids 
    public GridManager gridManager;

    int SpriteNumber = 0;

    private void Awake()
    {
        spawner = FindObjectOfType<Spawner>();
        gridManager = FindObjectOfType<GridManager>();
    }

    // theGrid fuction  is called by other script whenever a new grid is required
    // it is passed a Vector 3 to know where to start spawning the tiles that make up the grid, a size for the tiles and both the number of rows and columns that make up the grid
    public void Init(Vector3 startingPosition, float tilesize, int gridColumns, int gridRows)
    {    
        // creates a new array that is the size fo the new grid to store the new grid
        tileGrid = new GameObject[gridColumns, gridRows];
        SpriteNumber = 0;
        // nested for loop that will run for the number of rows and colums 
        for (int row = 0; row < gridRows; row++)
        {
            for (int column = 0; column < gridColumns; column++)
            {
                // moves the spawner to the starting position then up or down as necessary for that tile.
                spawner.transform.position = new Vector3(startingPosition.x + (tilesize * column), startingPosition.y - (tilesize * row), startingPosition.z);
                
               // calls spawnTile on the spawner then stores that new tile in the grid array
                tileGrid[column, row] = spawner.SpawnTile(gameObject);
               
                tileGrid[column, row].GetComponent<TileControls>().gridID = gridManager.gridID;
                tileGrid[column, row].GetComponent<TileControls>().thisTilesRow = row;
                tileGrid[column, row].GetComponent<TileControls>().thisTilesColumn = column;

                tileGrid[column, row].GetComponent<TileControls>().SetSprite(SpriteNumber);

                SpriteNumber++;
                if (SpriteNumber >= 5)
                {
                    SpriteNumber = 0;
                }
            }
        }

        gridManager.RegisterNewGrid(tileGrid, gridColumns, gridRows);
    }
}
