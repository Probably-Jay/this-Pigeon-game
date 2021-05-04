using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameCore;

public class GiftBasketSwap : MonoBehaviour
{

    public Text giftText;

    void Start() { 
        giftText.text = "Get A Seed";
    }

    private void OnEnable()
    {
        EventsManager.BindEvent(EventsManager.EventType.GardenSwapped, SwapText);
        giftText.text = "Get A Seed";
    }

    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.GardenSwapped, SwapText);
    }    
    


    void SwapText() {
        if (GameManager.Instance.PlayerWhosGardenIsCurrentlyVisible == Player.PlayerEnum.Player2)
        {
            giftText.text = "Get Seed";
        }
        else {
            giftText.text = "Gift Seed";
        }
    
    }
}
