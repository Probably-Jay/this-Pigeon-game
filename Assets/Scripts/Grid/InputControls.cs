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
    }

    void ResetObject()
    {
        // needs better reset position but possible to just delete object instead in next build
        currentObjectMoving.transform.position = new Vector3(6.5f, -3, 2);
    }

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
                        // checks to see if the object hit has the tag istile
                        if (hit.transform.gameObject.tag == "IsTile")
                        {
                            currentTile = hit.transform.gameObject;
                            // checks the current state and if it is open the activate tile
                            if (currentTile.GetComponent<TileControls>().curentState == TileControls.tileStates.Open)
                            {
                                tileControls = currentTile.GetComponent<TileControls>();
                                tileControls.TileOccupied();
                            } // checks the current state and if it is active open the tile
                            else if (currentTile.GetComponent<TileControls>().curentState == TileControls.tileStates.Occupied)
                            {
                                tileControls = currentTile.GetComponent<TileControls>();
                                tileControls.FreeTile();
                            }
                        } else if (hit.transform.gameObject.tag == "Object")
                        {
                            // turns moving on for the object you hit
                            moveableObject = hit.transform.gameObject;
                            currentObjectMoving = moveableObject.GetComponent<ObjectMovement>();
                            currentObjectMoving.moving = true;
                            currentObjectMoving.isPickedUp = false;
                            holdingObject = true;
                          
                            newRayOriginPoint = currentObjectMoving.transform.position;

                            objectSize = currentObjectMoving.GetComponent<Collider>().bounds.size;

                            newRayOriginPoint.x -= (objectSize.x / 2);
                            newRayOriginPoint.y += (objectSize.y / 2);

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
                    }else if (hit.transform.gameObject.tag == "Object")
                    {                       
                        // turns moving on for the object you hit
                        moveableObject = hit.transform.gameObject;
                        currentObjectMoving = moveableObject.GetComponent<ObjectMovement>();
                        currentObjectMoving.moving = true;
                    }
                }
            }
            else if(holdingObject == true)
            {
                currentObjectMoving = moveableObject.GetComponent<ObjectMovement>();

                newRayOriginPoint = currentObjectMoving.transform.position;

                objectSize = currentObjectMoving.GetComponent<Collider>().bounds.size;

                newRayOriginPoint.x -= (objectSize.x / 2);
                newRayOriginPoint.y += (objectSize.y / 2);

                // fires a raycast downward from the mouse 
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
                                newObjectLocation.x += (objectSize.x / 2.6f);
                                newObjectLocation.y -= (objectSize.y / 2.9f);
                                newObjectLocation.z = -2;

                                currentObjectMoving.transform.position = newObjectLocation;
                            }
                            else
                            {
                                ResetObject();
                            }
                        }
                        else
                        {
                            ResetObject();
                        }
                    }
                    else
                    {
                        ResetObject();
                    }
                }
                else
                {
                    ResetObject();
                }

                currentObjectMoving.moving = false;
                currentObjectMoving.ObjectNotTransparent();
                moveableObject = null;
                holdingObject = false;
            }
        }
    }
}
