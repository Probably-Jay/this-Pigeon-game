using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameCore;
using Localisation;

public class GiftBasketSwap : MonoBehaviour
{

    public Text giftText;

    void Start() {
        giftText.text = Localiser.GetText(TextID.GameScene_SeedBag_GetASeed);//"Get A Seed";
    }

    private void OnEnable()
    {
        EventsManager.BindEvent(EventsManager.EventType.GardenSwapped, SwapText);
        giftText.text = Localiser.GetText(TextID.GameScene_SeedBag_GetASeed);
    }

    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.GardenSwapped, SwapText);
    }    
    


    void SwapText() {
        if (GameManager.Instance.PlayerWhosGardenIsCurrentlyVisible == Player.PlayerEnum.Player2)
        {
            giftText.text = Localiser.GetText(TextID.GameScene_SeedBag_GetASeed);
        }
        else {
            giftText.text = Localiser.GetText(TextID.GameScene_SeedBag_GiftASeed);
        }
    
    }
}
