using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script created by Alexander Purvis 11/02/2021

public class ObjectMovement : MonoBehaviour
{
    // used to turn moving on and off
    public bool moving = false;
    public bool isplace = false;
    bool isPickedUp = false;


    // used time space out the raycast that are used track the tiles that this object is hovering over
    float timePassed = 0;
    float triggerTime = 1;

    // pont at which the object should fire its ray from
    Vector3 rayOriginPoint;

    // used for knowing the state of the tiles that are under it 
    TileControls tileControls;
    // for know what tile this object is on
    GameObject currentTile;

   // for storing the demensions of this object to let the tiles know how many of them this object requires
    int objectHeight = 2;
    int objectLength = 3;

    // to change the alpha of the object that is moving
    SpriteRenderer objectsSprite;
    Color objectColourValues;

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
                objectsSprite = this.GetComponent<SpriteRenderer>();
                objectColourValues = objectsSprite.material.color;

                objectColourValues.a = 0.3f;

                objectsSprite.material.color = objectColourValues;
            }
            
        }       
    }
}