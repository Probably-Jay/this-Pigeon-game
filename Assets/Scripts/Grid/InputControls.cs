using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script created by Alexander Purvis 04/02/2021
public class InputControls : MonoBehaviour
{
    GameObject currentTile;
    TileControls tileControls;

    GameObject moveableObject;
    ObjectMovement currentObjectMoving;

    Item PlantPlaced;
    DisplayManager plantManager;

    Vector3 PlantStats;

    public GridManager gridManager;

    // pont at which the object should fire its ray from
    Vector3 newRayOriginPoint;
    // for storing the demensions of this object to let the tiles know how many of them this object requires
    Vector3 objectSize;

    Vector3 newObjectLocation;

    bool holdingObject = false;

    private void Awake()
    {
        gridManager = GameObject.FindObjectOfType<GridManager>();
        plantManager = GameObject.FindObjectOfType<DisplayManager>();
    }

    //void ResetObject()
    //{
    //    // needs better reset position but possible to just delete object instead in next build
    //    currentObjectMoving.transform.position = new Vector3(6.5f, -3, 2);
    //}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {

            if (holdingObject == false)
            {
                // fires a raycast downward from the mouse 
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 100.0f))
                {
                    if (hit.transform != null)
                    {
                        //// checks to see if the object hit has the tag istile
                        //if (hit.transform.gameObject.tag == "IsTile")
                        //{
                        //    currentTile = hit.transform.gameObject;
                        //    // checks the current state and if it is open the activate tile
                        //    if (currentTile.GetComponent<TileControls>().curentState == TileControls.tileStates.Open)
                        //    {
                        //        tileControls = currentTile.GetComponent<TileControls>();
                        //        tileControls.TileOccupied();
                        //    } // checks the current state and if it is active open the tile
                        //    else if (currentTile.GetComponent<TileControls>().curentState == TileControls.tileStates.Occupied)
                        //    {
                        //        tileControls = currentTile.GetComponent<TileControls>();
                        //        tileControls.FreeTile();
                        //    }
                        //} 

                        if (hit.transform.gameObject.tag == "Object")
                        {
                            // turns moving on for the object you hit
                            moveableObject = hit.transform.gameObject;
                            currentObjectMoving = moveableObject.GetComponent<ObjectMovement>();
                            currentObjectMoving.moving = true;
                            currentObjectMoving.isPickedUp = false;
                            holdingObject = true;

                            PlantPlaced = moveableObject.GetComponent<Item>();

                            // if the plant we picked up was in a garden
                            if (PlantPlaced.isPlanted == true)
                            {
                                // gets the plants scores that should be subtracted
                                PlantStats = PlantPlaced.PlantStats;

                                // subtreacts the plants stats from the apropriote garden
                                if (PlantPlaced.gardenID < 2)
                                {
                                    plantManager.SubtractFromGarden1Stats(PlantStats);
                                }
                                else
                                {
                                    plantManager.ASubtractFromGarden2Stats(PlantStats);
                                }

                            }

                            gridManager.ShowGrids();

                            newRayOriginPoint = currentObjectMoving.transform.position;

                            objectSize = currentObjectMoving.GetComponent<Collider>().bounds.size;

                            //newRayOriginPoint.x -= (objectSize.x / 5);
                            //newRayOriginPoint.y += (objectSize.y / 5);


                            if (currentObjectMoving.ObjectWidth > 1)
                            {
                                newRayOriginPoint.x -= (objectSize.x / 5);
                            }
                            else
                            {
                                newRayOriginPoint.x -= (objectSize.x / 8);
                            }

                            if (currentObjectMoving.ObjectHeight > 1)
                            {
                                newRayOriginPoint.y += (objectSize.y / 5);
                            }
                            else
                            {
                                newRayOriginPoint.y += (objectSize.y / 8);
                            }


                            // fires a raycast downward from the mouse 
                            RaycastHit hit2;

                            if (Physics.Raycast(newRayOriginPoint, Camera.main.transform.forward, out hit2, 100.0f))
                            {
                                if (hit2.transform != null)
                                {
                                    // checks to see if the object hit has the tag istile
                                    if (hit2.transform.gameObject.tag == "IsTile")
                                    {
                                        currentTile = hit2.transform.gameObject;

                                        if (currentTile.GetComponent<TileControls>().curentState == TileControls.tileStates.Occupied)
                                        {
                                            tileControls = currentTile.GetComponent<TileControls>();
                                            gridManager.VacateTiles(tileControls.gridID, tileControls.thisTilesRow, tileControls.thisTilesColumn,
                                                                           currentObjectMoving.ObjectHeight, currentObjectMoving.ObjectWidth);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if(holdingObject == true)
            {
                currentObjectMoving = moveableObject.GetComponent<ObjectMovement>();

                newRayOriginPoint = currentObjectMoving.transform.position;

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

               

                // fires a raycast downward from the Plant 
                RaycastHit hit;

                if (Physics.Raycast(newRayOriginPoint, Camera.main.transform.forward, out hit, 100.0f))
                {
                    if (hit.transform != null)
                    {
                        // checks to see if the object hit has the tag istile
                        if (hit.transform.gameObject.tag == "IsTile")
                        {
                            currentTile = hit.transform.gameObject;
                          
                            if (currentTile.GetComponent<TileControls>().curentState == TileControls.tileStates.Active)
                            {
                                tileControls = currentTile.GetComponent<TileControls>();
                                gridManager.OccupyTiles(tileControls.gridID, tileControls.thisTilesRow, tileControls.thisTilesColumn,
                                                               currentObjectMoving.ObjectHeight, currentObjectMoving.ObjectWidth);

                                newObjectLocation = currentTile.transform.position;

                                //  currentObjectMoving.ObjectHeight, currentObjectMoving.ObjectWidth
                                if (currentObjectMoving.ObjectWidth  > 1)
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

                            
                                PlantPlaced = moveableObject.GetComponent<Item>();
                                // sets is planted to true so that if it is removed its values can be subtracted from the garden
                                PlantPlaced.isPlanted = true;

                                PlantPlaced.gardenID = tileControls.gridID;


                                // adds the plants scores to the display
                                PlantStats = PlantPlaced.PlantStats;

                                if (tileControls.gridID < 2)
                                {
                                    plantManager.AddtoGarden1Stats(PlantStats);
                                }
                                else
                                {
                                    plantManager.AddtoGarden2Stats(PlantStats);
                                }
                                                                                               
                                moveableObject = null;
                                holdingObject = false;

                                gridManager.HideGrids();
                            }
                                //    else
                                //    {
                                //        ResetObject();
                                //    }
                        }
                            //else
                            //{
                            //    ResetObject();
                            //}
                    }
                        //else
                        //{
                        //    ResetObject();
                        //}
                }
                    //else
                    //{
                    //    ResetObject();
                    //}              

            }
        }
    }
}
