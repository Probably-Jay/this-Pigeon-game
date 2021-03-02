using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Created Jay 05/02

public class InventoryUIControl : MonoBehaviour
{
    [SerializeField] InventoryList itemList;
    [SerializeField] GameObject UISlotPrefab;
    [SerializeField] GameObject canvas;

    [SerializeField, Range(0, 200)] float spacing;
    [SerializeField, Range(0, 50)] float padding;

    [SerializeField] int listOffset = 0;

    List<Transform> slots;

    const int numberOfSlots = 5;

    private void Awake()
    {

        // get all slots from children
        for (int i = 0; i < transform.childCount; i++)
        {
            slots.Add(transform.GetChild(i));
        }

        SetInventoryUI();

    }

    /// <summary>
    /// Set the item slots in the ui ribbon
    /// </summary>
    private void SetInventoryUI()
    {
        List<GameObject> toDestroy = new List<GameObject>(); // items that are no longer visible
        List<GameObject> toAdd = new List<GameObject>(); // items that have just become visible
        for (int i = 0; i < canvas.transform.childCount; i++)
        {
            if (i >= itemList.list.Count) break;

            var previousChild = canvas.transform.GetChild(i);
            var pos = previousChild.transform.position;
            toDestroy.Add(previousChild.gameObject);

            var slot = GameObject.Instantiate(UISlotPrefab, pos, Quaternion.identity) as GameObject;

            slot.GetComponent<InventoryUISlot>().Init(itemList.list[i+ listOffset]);

            toAdd.Add(slot);

        }

        foreach (var child in toDestroy)
        {
            Destroy(child);
        }

        foreach (var item in toAdd)
        {
            item.transform.SetParent(canvas.transform, true);
            item.transform.SetAsLastSibling();
        }

    }

    public void ModifyListOffset(int value)
    {
        listOffset = Mathf.Clamp(listOffset + value, 0, itemList.list.Count - numberOfSlots);
        SetInventoryUI();
    }
   // public void DecreaseListOffset() => listOffset = Mathf.Clamp(listOffset-1, 0, itemList.list.Count - numberOfSlots);

}
