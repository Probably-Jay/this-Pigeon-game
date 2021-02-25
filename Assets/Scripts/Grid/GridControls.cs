using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script created by Alexander Purvis 03/02/2021


public class GridControls : MonoBehaviour
{
    // starting information for first grid
   //[SerializeField] 
    Vector3 startingPositionGrid1 = new Vector3(-7.0f, 3.2f, 0.0f);
    float tileSizeGrid1 = 1.0f;
    int columnsGrid1 = 10;
    int rowsGrid1 = 7;

    Vector3 startingPositionGrid2 = new Vector3(-7.0f, 13.2f, 0.0f);
    float tileSizeGrid2 = 1.0f;
    int columnsGrid2 = 10;
    int rowsGrid2 = 7;

    // Update is called once per frame
    void Awake()
    {      
            // creates a new grid passing it the starting information
            TheGrid gridP1 = new TheGrid(startingPositionGrid1, tileSizeGrid1, columnsGrid1, rowsGrid1);
       
            TheGrid gridP2 = new TheGrid(startingPositionGrid2, tileSizeGrid2, columnsGrid2, rowsGrid2);

    }
}
