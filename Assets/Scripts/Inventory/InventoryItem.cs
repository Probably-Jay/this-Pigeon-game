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

    [HideInInspector] public string itemName; 
    public Sprite inventoryImage; // image shown in the inventory UI
    public GameObject itemGameObject; // prefab of actual object



    public void SpawnObjectRandomPos()
    {
        var size = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
        Rect screnDim = new Rect(Vector2.zero, size );
        var screenPos = new Vector2(Random.Range(screnDim.x, screnDim.width), Random.Range(screnDim.y, screnDim.height));
        var pos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, -Camera.main.transform.position.z));
        Instantiate(itemGameObject, pos, Quaternion.identity);  
    } 
    
    public void SpawnObjectMiddleOfScreen()
    {
        var size = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
        Rect screnDim = new Rect(Vector2.zero, size );
        var screenPos = new Vector2(screnDim.width /2f, screnDim.height/2f);
        var pos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, -Camera.main.transform.position.z-1f));
        Instantiate(itemGameObject, pos, Quaternion.identity);  



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
