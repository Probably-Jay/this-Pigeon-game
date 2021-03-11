using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInList
{
    public GameObject[,] tile;

    public int thisGridsWidth = 0;
    public int thisGridsHeight = 0;
    Player.PlayerEnum playersGarden;

    public GridInList(GameObject[,] newGrid, int gridColumns, int gridRows, Player.PlayerEnum garden)
    {
        tile = new GameObject[gridColumns, gridRows];
        tile = newGrid;
       
        thisGridsWidth = gridColumns;
        thisGridsHeight = gridRows;

        playersGarden = garden;
    }
}