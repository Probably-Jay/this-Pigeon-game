using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// jay 27/03

public class PlayerGardenSignControl : MonoBehaviour
{

    [SerializeField] TMP_Text player1GardenText;
    [SerializeField] TMP_Text player2GardenText;

    [SerializeField] string yourGarden = "Your Garden";
    [SerializeField] string yourCompanionsGarden = "Companion's Garden";

    [SerializeField, Range(0, 1)] float turnUpdateDelay;


    private void Start()
    {
        UpdateSigns();
    }

    private void OnEnable()
    {
        EventsManager.BindEvent(EventsManager.EventType.NewTurnBegin, UpdateSigns);
    }

    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.NewTurnBegin, UpdateSigns);
    }

    private void UpdateSigns() => StartCoroutine(UpdateSignsDelayed());



    private IEnumerator UpdateSignsDelayed()
    {
        if(turnUpdateDelay>0) yield return new WaitForSeconds(turnUpdateDelay);

        switch (GameManager.Instance.ActivePlayerID)
        {
            case Player.PlayerEnum.Player1:
                player1GardenText.text = yourGarden;
                player2GardenText.text = yourCompanionsGarden;
                break;
            case Player.PlayerEnum.Player2:
                player1GardenText.text = yourCompanionsGarden;
                player2GardenText.text = yourGarden;
                break;
            default:
                player1GardenText.text = "-";
                player2GardenText.text = "-";
                break;
        }
    }
}
