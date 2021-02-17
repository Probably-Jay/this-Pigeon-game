using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script created by Alexander Purvis 03/02/2021

public class GridControls : MonoBehaviour
{
    // starting information for first grid
   [SerializeField] Vector3 startingPosition = new Vector3(-7.0f, 4.0f, 0.0f);
   [SerializeField] float tileSize = 1.0f;
   [SerializeField] int columns = 10;
   [SerializeField] int rows = 8;


    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(1))
        {
            // creates a new grid passing it the starting information
            TheGrid grid = new TheGrid(startingPosition, tileSize, columns, rows);
        }
    }
}
