using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// jay 05/02

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

    // for xander to change
    public void ItemSelected() => item.SpawnObjectRandomPos();


}
