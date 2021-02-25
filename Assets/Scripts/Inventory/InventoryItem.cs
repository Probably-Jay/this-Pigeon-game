using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// Created Jay 05/02


// object that exists in the project's assets, used by UI to create the correct object, stores metadata about the object
[System.Serializable]
[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item", order = 1)]
public class InventoryItem : ScriptableObject
{

    public string itemName; // easily edit name
    public Sprite inventoryImage; // image shown in the inventory UI
    public GameObject gameObject; // prefab of actual object



    // for xander to change
    public void SpawnObjectRandomPos()
    {
       
        var size = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
        Rect screnDim = new Rect(Vector2.zero, size );
        var screenPos = new Vector2(Random.Range(screnDim.x, screnDim.width), Random.Range(screnDim.y, screnDim.height));
        var pos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, -Camera.main.transform.position.z));
        Instantiate(gameObject, pos, Quaternion.identity);  
    }

    // apply the name to internals
    private void OnEnable()
    {
        itemName = name;
    }
    public void OnValidate()    
    {
        name = itemName;
    }
}
