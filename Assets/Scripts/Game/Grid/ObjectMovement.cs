using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script created by Alexander Purvis 11/02/2021

public class ObjectMovement : MonoBehaviour
{
    // used to turn moving on and off
    public bool moving = false;
    public bool isplace = false;
    public bool isPickedUp = false;

    public GridManager gridManager;

    // used time space out the raycast that are used track the tiles that this object is hovering over
    float timePassed = 0;
    float triggerTime = 0.08f;

    // pont at which the object should fire its ray from
    Vector3 newRayOriginPoint;

    // used for knowing the state of the tiles that are under it 
    Tile tileControls;
    // for know what tile this object is on
    GameObject currentTile;

    // for storing the demensions of this object to let the tiles know how many of them this object requires
    Vector3 objectSize;

    // for the height and width of the object that is moving
    public int ObjectHeight = 2;
    public int ObjectWidth = 4;

    // to change the alpha of the object that is moving
    SpriteRenderer objectsSprite;
    Color objectColourValues;


    private void Awake()
    {
        gridManager = GameObject.FindObjectOfType<GridManager>();
    }

    // returns a vector 3 that is the position of the mouse converted from pixel coordinates to world coordinates
    private Vector3 GetMouseWorldPosition()
    {
        // mouse position in pixel coordinates
        Vector3 mousePosition = Input.mousePosition;

        // converts the mouse pos to world pos
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        //sets the zed value to 2 so that the object moving wiil be infrount on any other objects in the scene 
        mousePosition.z = -2;

        return mousePosition;
    }


    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            // moves the object this script is attached to to the curent mouse position 
            transform.localPosition = GetMouseWorldPosition();

            if (isPickedUp == false)
            {
                ObjectTransparent();
                isPickedUp = true;
            }

            if (timePassed >= triggerTime)
            {
                // sets the 
                newRayOriginPoint = this.transform.position;

                objectSize = GetComponent<Collider>().bounds.size;

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
                            tileControls = currentTile.GetComponent<Tile>();
                                
                                gridManager.HoveringOverTiles(tileControls.gridID, tileControls.thisTilesRow, tileControls.thisTilesColumn,
                                                                ObjectHeight, ObjectWidth);                          
                        }
                    }
                }
                timePassed = 0;               
            }
            else
            {
                timePassed += Time.deltaTime;
            }
        }
    }

  void ObjectTransparent()
  {
        objectsSprite = this.GetComponent<SpriteRenderer>();
        objectColourValues = objectsSprite.material.color;

        objectColourValues.a = 0.5f;

        objectsSprite.material.color = objectColourValues;
  }

    public void ObjectNotTransparent()
    {
        objectsSprite = this.GetComponent<SpriteRenderer>();
        objectColourValues = objectsSprite.material.color;

        objectColourValues.a = 1.0f;

        objectsSprite.material.color = objectColourValues;
    }

}