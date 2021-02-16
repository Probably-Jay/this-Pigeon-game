using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script created by Alexander Purvis 04/02/2021
public class InputControls : MonoBehaviour
{

    GameObject currentTile;
    TileControls tileControls;

    GameObject moveableObject;
    ObjectMovment objectMovment;
   


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
        
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
                        if (currentTile.GetComponent<TileControls>().curentState == TileControls.tileStates.OPEN)
                        {                           
                            tileControls = currentTile.GetComponent<TileControls>();
                            tileControls.ActivateTile();
                        } // checks the current state and if it is active open the tile
                        else if (currentTile.GetComponent<TileControls>().curentState == TileControls.tileStates.ACTIVE)
                        {
                            tileControls = currentTile.GetComponent<TileControls>();
                            tileControls.FreeTile();
                        }
                    }else if (hit.transform.gameObject.tag == "Object")
                    {                       
                        // turns moving on for the object you hit
                        moveableObject = hit.transform.gameObject;
                        objectMovment = moveableObject.GetComponent<ObjectMovment>();
                        objectMovment.moving = true;
                    }
                }
            }           
        }
    }

}
