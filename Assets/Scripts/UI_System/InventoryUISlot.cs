﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class InventoryUISlot : MonoBehaviour
{

    public InventoryItem item;
    
    Button button;



    private void Awake()
    {
        button = GetComponent<Button>();
       
    }

    public void Init(InventoryItem inventoryItem)
    {
        item = inventoryItem;
        button.GetComponent<Image>().sprite = item.inventoryImage;
        
    }

    void Start()
    {
        if (!item)
            Debug.LogWarning($"{nameof(InventoryUISlot)}: {name} does not have an {nameof(InventoryItem)}, was it initialised?");

    }

    public void SpawnItemObjectAtRandomPosition() => item.SpawnObjectRandomPos();


}
