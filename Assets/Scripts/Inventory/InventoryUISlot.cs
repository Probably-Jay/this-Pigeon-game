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

    public void ItemSelected()
    {
        if(GameManager.Instance.CurrentVisibleGarden == GameManager.Instance.ActivePlayer.PlayerEnumValue) // our turn
        {
            if (GameManager.Instance.ActivePlayer.TurnPoints.HasPointsLeft(TurnPoints.PointType.SelfObjectPlace)) 
            {
                SpawnObject();
            }
            EventsManager.InvokeEvent(EventsManager.EventType.triedToPlaceOwnObject);
        }
        else
        {
            if (GameManager.Instance.ActivePlayer.TurnPoints.HasPointsLeft(TurnPoints.PointType.OtherObjectPlace))
            {
                SpawnObject();
            }
            EventsManager.InvokeEvent(EventsManager.EventType.triedToPlaceCompanionObject);
        }
    }

    void SpawnObject()
    {
        item.SpawnObjectMiddleOfScreen();
    }

}
