using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Created by Alexander Purvis 06/03/2021

public class HideShowInventory : MonoBehaviour
{

   public Canvas Inventory;

   bool inventoyOpen = false;


   public void InventoryButtonPressed()
   {
        if (inventoyOpen)
        {
            HideInvintory();
        }
        else {

            ShowInvintory();
        }

    }



   public  void HideInvintory()
   {
        Inventory.gameObject.SetActive(false);
        inventoyOpen = false;
    }


    public void ShowInvintory()
    {
        Inventory.gameObject.SetActive(true);
        inventoyOpen = true;
    }
}
