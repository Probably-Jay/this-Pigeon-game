using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script created by Alexander Purvis 23/02/2021

public class GridManager : MonoBehaviour
{
   public int gridID = 0; 
   public List<GridInList> gridList = new List<GridInList>();  
   bool abortObjectPlacement = false;

    public GameObject PlayerOneGarden;
    public GameObject PlayerTwoGarden;

    public void RegisterNewGrid(GameObject [,]grid, int gridColumns, int gridRows)
    {
       gridList.Add(new GridInList(grid, gridColumns, gridRows));

        gridID++;
    }

    public void HoveringOverTiles(int gridNumber, int tileCoordinateX, int tileCoordinateY, 
                                    int objectHeight, int objectWidth)
    {
        for (int row = 0; row < objectHeight; row++)
        {
            for (int column = 0; column < objectWidth; column++)
            {
                if (tileCoordinateX + row >= gridList[gridNumber].thisGridsHeight || tileCoordinateY + column >= gridList[gridNumber].thisGridsWidth)
                {
                    SignalInappropriatePlacement(gridNumber, tileCoordinateX, tileCoordinateY, objectHeight, objectWidth);
                }
                else if (gridList[gridNumber].tile[tileCoordinateY + column, tileCoordinateX + row].GetComponent<TileControls>().curentState == TileControls.tileStates.Occupied)
                {
                    SignalInappropriatePlacement(gridNumber, tileCoordinateX, tileCoordinateY, objectHeight, objectWidth);
                }
                else
                {
                    gridList[gridNumber].tile[tileCoordinateY + column, tileCoordinateX + row].GetComponent<TileControls>().ActivateTile();                                  
                }

                if (abortObjectPlacement)
                {
                    break;
                }
            }

            if (abortObjectPlacement)
            {
                abortObjectPlacement = false;
                break;
            }
        }
    }

    void SignalInappropriatePlacement(int gridNumber, int tileCoordinateX, int tileCoordinateY,
                                    int objectHeight, int objectWidth)
    {
        for (int row = 0; row < objectHeight; row++)
        {
            for (int column = 0; column < objectWidth; column++)
            {

                if (tileCoordinateX + row < gridList[gridNumber].thisGridsHeight && tileCoordinateY + column < gridList[gridNumber].thisGridsWidth)
                {
                    if (gridList[gridNumber].tile[tileCoordinateY + column, tileCoordinateX + row].GetComponent<TileControls>().curentState != TileControls.tileStates.Occupied)
                    {
                        gridList[gridNumber].tile[tileCoordinateY + column, tileCoordinateX + row].GetComponent<TileControls>().TileDenied();
                    }
                }
            }
        }
        abortObjectPlacement = true;
    }

    public void OccupyTiles(int gridNumber, int tileCoordinateX, int tileCoordinateY,
                                    int objectHeight, int objectWidth)
    {
        for (int row = 0; row < objectHeight; row++)
        {
            for (int column = 0; column < objectWidth; column++)
            {              
                        gridList[gridNumber].tile[tileCoordinateY + column, tileCoordinateX + row].GetComponent<TileControls>().TileOccupied();                
            }
        }
    }

    public void VacateTiles(int gridNumber, int tileCoordinateX, int tileCoordinateY,
                                   int objectHeight, int objectWidth)
    {
        for (int row = 0; row < objectHeight; row++)
        {
            for (int column = 0; column < objectWidth; column++)
            {
                gridList[gridNumber].tile[tileCoordinateY + column, tileCoordinateX + row].GetComponent<TileControls>().FreeTile();
            }
        }
    }

    public void ShowGrids()
    {
        for (int gridNumber = 0; gridNumber < gridList.Count; gridNumber++)
        {          
            for (int rows = 0; rows < gridList[gridNumber].thisGridsHeight; rows++)
            {
                for (int columns = 0; columns < gridList[gridNumber].thisGridsWidth; columns++)
                {
                    gridList[gridNumber].tile[columns, rows ].GetComponent<TileControls>().ShowTile();
                }
            }                    
        }
    }

    public void HideGrids()
    {
        for (int gridNumber = 0; gridNumber < gridList.Count; gridNumber++)
        {           
            for (int rows = 0; rows < gridList[gridNumber].thisGridsHeight; rows++)
            {
                for (int columns = 0; columns < gridList[gridNumber].thisGridsWidth; columns++)
                {
                    gridList[gridNumber].tile[columns, rows].GetComponent<TileControls>().HideTiles();
                }
            }
        }
    }

}
