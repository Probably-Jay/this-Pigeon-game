using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameCore;

// jay 05/02

// Edited By Alexander Purvis 06/03/2012

[RequireComponent(typeof(Button))]
public class InventoryUISlot : MonoBehaviour
{

    public InventoryItem item;
    
    public Button button;

    public Button ShowHideInventoyButton;

    HideShowInventory ButtonControls;


    private void Awake()
    {
        button = GetComponent<Button>();

        ShowHideInventoyButton = GameObject.Find("InventoryButton").GetComponent<Button>();


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
        if(GameManager.Instance.PlayerWhosGardenIsCurrentlyVisible == GameManager.Instance.LocalPlayerID) 
        {
            if (GameManager.Instance.LocalPlayer.TurnPoints.HasPointsLeft(TurnPoints.PointType.SelfObjectPlace)) 
            {
                SpawnObject();
            }
            EventsManager.InvokeEvent(EventsManager.EventType.triedToPlaceOwnObject);
        }
        else
        {
            if (GameManager.Instance.LocalPlayer.TurnPoints.HasPointsLeft(TurnPoints.PointType.OtherObjectPlace))
            {
                SpawnObject();
            }
            EventsManager.InvokeEvent(EventsManager.EventType.triedToPlaceCompanionObject);
        }

        HideSelf();
    }

    void SpawnObject()
    {
        item.SpawnObjectMiddleOfScreen();

    }





    void HideSelf()
    {

        ButtonControls = ShowHideInventoyButton.GetComponent<HideShowInventory>();
        ButtonControls.HideInvintory();
    }




}
