using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script created by Alexander Purvis 03/02/2021


public class GridControls : MonoBehaviour
{
    // starting information for first grid
   //[SerializeField] 
    Vector3 startingPositionGrid1 = new Vector3(-1.98f, 2.2f, 0.0f);
    float tileSizeGrid1 = 1.0f;
    int columnsGrid1 = 2;
    int rowsGrid1 = 5;

    Vector3 startingPositionGrid2 = new Vector3(0.8f, 2.2f, 0.0f);
    
    float tileSizeGrid2 = 1.0f;
    int columnsGrid2 = 2;
    int rowsGrid2 = 6;

    Vector3 startingPositionGrid3 = new Vector3(1.0f, 12.2f, 0.0f);
    float tileSizeGrid3 = 1.0f;
    int columnsGrid3 = 2;
    int rowsGrid3 = 5;

    Vector3 startingPositionGrid4 = new Vector3(-1.8f, 12.0f, 0.0f);
    float tileSizeGrid4 = 1.0f;
    int columnsGrid4 = 2;
    int rowsGrid4 = 6;

   

    [SerializeField] GameObject grid1P1Gameobject;
    [SerializeField] GameObject grid2P1Gameobject;


    [SerializeField] GameObject grid1P2Gameobject;
    [SerializeField] GameObject grid2P2Gameobject;

    // Update is called once per frame
    void Awake()
    {

        grid1P1Gameobject = new GameObject("Grid1 Player one");
        grid2P1Gameobject = new GameObject("Grid2 Player one");


        grid1P2Gameobject = new GameObject("Grid1 Player Two");
        grid2P2Gameobject = new GameObject("Grid2 Player Two");


        var grid1P1 = grid1P1Gameobject.AddComponent<TheGrid>();
        var grid2P1 = grid2P1Gameobject.AddComponent<TheGrid>();

        var grid1P2 = grid1P2Gameobject.AddComponent<TheGrid>();
        var grid2P2 = grid2P2Gameobject.AddComponent<TheGrid>();

        grid1P1.Init(startingPositionGrid1, tileSizeGrid1, columnsGrid1, rowsGrid1);
        grid2P1.Init(startingPositionGrid2, tileSizeGrid2, columnsGrid2, rowsGrid2);

        grid1P2.Init(startingPositionGrid3, tileSizeGrid3, columnsGrid3, rowsGrid3);
        grid2P2.Init(startingPositionGrid4, tileSizeGrid4, columnsGrid4, rowsGrid4);

    }
}
