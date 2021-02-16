using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovment : MonoBehaviour
{

    public bool moving = false;
    
    // public bool resetting = false;
    // public bool isplace = false;


    private Vector3 GetMouseWorldPosition()
    {
        // pixel coordinates(xy)
        Vector3 mousePosition = Input.mousePosition;

        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        mousePosition.z = 2;

        return mousePosition;
    }

  
    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
           
            transform.localPosition = GetMouseWorldPosition();
        }       
    }
}