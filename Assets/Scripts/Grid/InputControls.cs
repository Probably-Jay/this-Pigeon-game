using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.transform != null)
                {                
                    if (hit.transform.gameObject.tag == "IsTile")
                    {
                        currentTile = hit.transform.gameObject;

                        if (currentTile.GetComponent<TileControls>().curentState == TileControls.tileStates.OPEN)
                        {                           
                            tileControls = currentTile.GetComponent<TileControls>();
                            tileControls.ActivateTile();
                        }
                        else if (currentTile.GetComponent<TileControls>().curentState == TileControls.tileStates.ACTIVE)
                        {
                            tileControls = currentTile.GetComponent<TileControls>();
                            tileControls.FreeTile();
                        }
                    }else if (hit.transform.gameObject.tag == "Object")
                    {                       
                        moveableObject = hit.transform.gameObject;
                        objectMovment = moveableObject.GetComponent<ObjectMovment>();
                        objectMovment.moving = true;
                    }
                }
            }           
        }
    }

    //testing
    private void PrintName(GameObject detectedObject)
    {
      
        Debug.Log(detectedObject.name + " was hit");
    }
    //~
}
