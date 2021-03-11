using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script created by Alexander Purvis 04/02/2021
public class InputControls : MonoBehaviour
{


    GameObject heldObject;
    ObjectMovement currentObjectMoving;

    //PlantItem plantPlaced;
    CurrentMood displayManager;

   // MoodAtributes PlantStats;

    public GridManager gridManager;

    // pont at which the object should fire its ray from
    //Vector3 newRayOriginPoint;
    // for storing the demensions of this object to let the tiles know how many of them this object requires
    Vector3 objectSize;

    Vector3 newObjectLocation;

    bool holdingObject = false;

    private void Awake()
    {
        gridManager = GameObject.FindObjectOfType<GridManager>();
        displayManager = GameObject.FindObjectOfType<CurrentMood>();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {

            if (holdingObject == false)
            {
                PickUpObject();
            }
            else if (holdingObject == true)
                PutDownObject();
        }
    }

    private void PickUpObject()
    {
        // fires a raycast downward from the mouse 
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            if (hit.transform != null && hit.transform.gameObject.tag == "Object")
            {

                // turns moving on for the object you hit
                heldObject = hit.transform.gameObject;
                currentObjectMoving = heldObject.GetComponent<ObjectMovement>();
                currentObjectMoving.moving = true;
                currentObjectMoving.isPickedUp = false;
                holdingObject = true;

                var plantPlaced = heldObject.GetComponent<PlantItem>();


                // if the plant we picked up was in a garden
                if (plantPlaced.gardenID != Player.PlayerEnum.Unnasigned)
                {
                    displayManager.SubtractFromGardenStats(plantPlaced.gardenID, plantPlaced.PlantStats);
                    plantPlaced.gardenID = Player.PlayerEnum.Unnasigned; // plant is now not in a garden

                }

                gridManager.ShowGrids(); // make the grid visable


                VacateTiles();

            }
        }
    }

    private void VacateTiles()
    {

        // should consider adding an if object planted condition for below
        var newRayOriginPoint = currentObjectMoving.transform.position;

        objectSize = currentObjectMoving.GetComponent<Collider>().bounds.size;

        newRayOriginPoint.x -= (objectSize.x / 3);
        newRayOriginPoint.y += (objectSize.y / 3);


        // fires a raycast downward from the mouse 
        RaycastHit hit2;

        if (Physics.Raycast(newRayOriginPoint, Camera.main.transform.forward, out hit2, 100.0f))
        {
            if (hit2.transform != null)
            {
                // checks to see if the object hit has the tag istile
                if (hit2.transform.gameObject.tag == "IsTile")
                {
                    var currentTile = hit2.transform.gameObject;

                    if (currentTile.GetComponent<Tile>().curentState == Tile.TileStates.Occupied)
                    {
                        var tileControls = currentTile.GetComponent<Tile>();
                        gridManager.VacateTiles(tileControls.gridID, tileControls.thisTilesRow, tileControls.thisTilesColumn,
                                                        currentObjectMoving.ObjectHeight, currentObjectMoving.ObjectWidth);
                    }
                }
            }
        }
    }

    private void PutDownObject()
    {
        currentObjectMoving = heldObject.GetComponent<ObjectMovement>();
        Vector3 newRayOriginPoint = GetRayOriginPoint();

        // fires a raycast downward from the Plant 
        RaycastHit hit;

        if (Physics.Raycast(newRayOriginPoint, Camera.main.transform.forward, out hit, 100.0f))
        {
            if (hit.transform != null && hit.transform.gameObject.tag == "IsTile")
            {
                // checks to see if the object hit has the tag istile
              
                var currentTile = hit.transform.gameObject;

                if (currentTile.GetComponent<Tile>().curentState == Tile.TileStates.Active)
                {
                    var tileControls = currentTile.GetComponent<Tile>();
                    gridManager.OccupyTiles(tileControls.gridID, tileControls.thisTilesRow, tileControls.thisTilesColumn,
                                                    currentObjectMoving.ObjectHeight, currentObjectMoving.ObjectWidth);

                    newObjectLocation = currentTile.transform.position;

                    if (currentObjectMoving.ObjectWidth > 1)
                    {
                        newObjectLocation.x += (objectSize.x / 4);
                    }


                    if (currentObjectMoving.ObjectHeight > 1)
                    {
                        newObjectLocation.y -= (objectSize.y / 4);
                    }


                    newObjectLocation.z = -2;

                    currentObjectMoving.transform.position = newObjectLocation;

                    currentObjectMoving.moving = false;
                    currentObjectMoving.ObjectNotTransparent();


                    var plantPlaced = heldObject.GetComponent<PlantItem>();
                    // sets is planted to true so that if it is removed its values can be subtracted from the garden

                    plantPlaced.gardenID = GameManager.Instance.CurrentVisibleGarden;

                    displayManager.AddToGardenStats(plantPlaced.gardenID, plantPlaced.PlantStats);
        

                    heldObject = null;
                    holdingObject = false;

                    gridManager.HideGrids();
                }

                

            }

        }
               

    }

    private Vector3 GetRayOriginPoint()
    {
        var newRayOriginPoint = currentObjectMoving.transform.position;

        objectSize = currentObjectMoving.GetComponent<Collider>().bounds.size;

        if (currentObjectMoving.ObjectWidth > 1)
        {
            newRayOriginPoint.x -= (objectSize.x / 2);
        }
        else
        {
            newRayOriginPoint.x -= (objectSize.x / 6);
        }

        if (currentObjectMoving.ObjectHeight > 1)
        {
            newRayOriginPoint.y += (objectSize.y / 2);
        }
        else
        {
            newRayOriginPoint.y += (objectSize.y / 6);
        }

        return newRayOriginPoint;
    }
}
