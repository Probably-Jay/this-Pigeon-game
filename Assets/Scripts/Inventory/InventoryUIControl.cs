using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Created Jay 05/02

public class InventoryUIControl : MonoBehaviour
{
    [SerializeField] InventoryList itemList;
    [SerializeField] GameObject UISlotPrefab;
    [SerializeField] GameObject inventoryDisplay;

    [SerializeField] int listOffset = 0;

    List<Transform> slots = new List<Transform>();

    const int numberOfSlots = 5;

    private void Awake()
    {
        // get all slots from children
        for (int i = 0; i < inventoryDisplay.transform.childCount; i++)
        {
            slots.Add(inventoryDisplay.transform.GetChild(i));
        }

        SetInventoryUI();
    }
    /*
    /// <summary>
    /// Set the item slots in the ui ribbon
    /// </summary>
    [System.Obsolete("Use " + nameof(InventoryUIControl.SetInventoryUI),false)]
    private void SetInventoryUIOld()
    {
        //List<GameObject> toDestroy = new List<GameObject>(); // items that are no longer visible
        //List<GameObject> toAdd = new List<GameObject>(); // items that have just become visible
        //for (int i = 0; i < canvas.transform.childCount; i++)
        //{
        //    if (i >= itemList.list.Count) break; // don't overflow the list

        //    var previousChild = canvas.transform.GetChild(i);
        //    var pos = previousChild.transform.position;
        //    toDestroy.Add(previousChild.gameObject);

        //    var slot = GameObject.Instantiate(UISlotPrefab, pos, Quaternion.identity) as GameObject;

        //    slot.GetComponent<InventoryUISlot>().Init(itemList.list[i + listOffset]);

        //    toAdd.Add(slot);

        //}

        //foreach (var child in toDestroy)
        //{
        //    Destroy(child);
        //}

        //foreach (var item in toAdd)
        //{
        //    item.transform.SetParent(canvas.transform, true);
        //    item.transform.SetAsLastSibling();
        //}

    }*/

    private void SetInventoryUI()
    {

        List<GameObject> toDestroy = new List<GameObject>(); // items that are no longer visible (seperate loops for seftey)

        for (int i = 0; i < slots.Count; i++)
        {
            if (i >= itemList.list.Count) break; // don't overflow the list
            var slot = slots[i];

            DestroyChildren(toDestroy, slot);

            var itemButton = GameObject.Instantiate(UISlotPrefab, slot) as GameObject;
            itemButton.GetComponent<InventoryUISlot>().Init(itemList.list[i + listOffset]); // call init to set the item this itemButton holds
        }


        foreach (var child in toDestroy)
        {
            Destroy(child);
        }
    }

    private static void DestroyChildren(List<GameObject> toDestroy, Transform slot)
    {
        if (slot.childCount == 0) return;
        else
        {
            for (int j = 0; j < slot.childCount; j++)
            {
                toDestroy.Add(slot.GetChild(j).gameObject);
            }
        }
    }

    /// <summary>
    /// Used by the buttons to move the visible buttons
    /// </summary>
    /// <param name="value"></param>
    public void ModifyListOffset(int value)
    {
        listOffset = Mathf.Clamp(listOffset + value, 0, itemList.list.Count - numberOfSlots);
        SetInventoryUI();
    }

}
