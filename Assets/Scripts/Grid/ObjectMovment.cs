using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script created by Alexander Purvis 11/02/2021

public class ObjectMovment : MonoBehaviour
{
    // used to turn moving on and off
    public bool moving = false;

    public bool isplace = false;

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
        }       
    }
}