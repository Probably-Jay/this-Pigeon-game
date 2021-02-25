using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;

// created jay 12/02

public class TurnPointsUI : MonoBehaviour
{


    [SerializeField] GameObject placeOwnPlantPoints;
    [SerializeField] GameObject placeCompanionPlantPoints;
    [SerializeField] GameObject removeOwnPlantPoints;
    [SerializeField] GameObject waterOwnPoints;

    Text textPlaceOwnPlantPoints;
    Text textPlaceCompanionPlantPoints;
    Text textRemoveOwnPlantPoints;
    Text textWaterOwnPoints;

    Animator animatorPlaceOwnPlantPoints;
    Animator animatorPlaceCompanionPlantPoints;
    Animator animatorRemoveOwnPlantPoints;
    Animator animatorWaterOwnPoints;




    private void Awake()
    {
        textPlaceOwnPlantPoints         = placeOwnPlantPoints.GetComponent<Text>();
        textPlaceCompanionPlantPoints   = placeCompanionPlantPoints.GetComponent<Text>();
        textRemoveOwnPlantPoints        = removeOwnPlantPoints.GetComponent<Text>();
        textWaterOwnPoints              = waterOwnPoints.GetComponent<Text>(); 
        
        animatorPlaceOwnPlantPoints         = placeOwnPlantPoints.GetComponent<Animator>();
        animatorPlaceCompanionPlantPoints   = placeCompanionPlantPoints.GetComponent<Animator>();
        animatorRemoveOwnPlantPoints        = removeOwnPlantPoints.GetComponent<Animator>();
        animatorWaterOwnPoints              = waterOwnPoints.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateText();

    }

    private void OnEnable()
    {
        EventsManager.BindEvent(EventsManager.EventType.NewTurnBegin, FlashGreen);
        EventsManager.BindEvent(EventsManager.ParameterEventType.NotEnoughPointsForAction, FlashRed);

        EventsManager.BindEvent(EventsManager.EventType.PlacedOwnObject, PopOwnPlace);
        EventsManager.BindEvent(EventsManager.EventType.PlacedCompanionObject, PopCompPlace);
        EventsManager.BindEvent(EventsManager.EventType.RemovedOwnObject, PopOwnRemove);
        EventsManager.BindEvent(EventsManager.EventType.WateredOwnPlant, PopWater);
    }
    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.NewTurnBegin, FlashGreen);
        EventsManager.UnbindEvent(EventsManager.ParameterEventType.NotEnoughPointsForAction, FlashRed);

        EventsManager.UnbindEvent(EventsManager.EventType.PlacedOwnObject, PopOwnPlace);
        EventsManager.UnbindEvent(EventsManager.EventType.PlacedCompanionObject, PopCompPlace);
        EventsManager.UnbindEvent(EventsManager.EventType.RemovedOwnObject, PopOwnRemove);
        EventsManager.UnbindEvent(EventsManager.EventType.WateredOwnPlant, PopWater);
    }

    private void UpdateText()
    {
        var activePlayerPoints = GameManager.Instance.HotSeatManager.ActivePlayer.TurnPoints;
        textPlaceOwnPlantPoints.text = $"Place objects points:\n{activePlayerPoints.GetPoints(TurnPoints.PointType.SelfObjectPlace)}";
        textPlaceCompanionPlantPoints.text = $"Place companion's objects points:\n{activePlayerPoints.GetPoints(TurnPoints.PointType.OtherObjectPlace)}";
        textRemoveOwnPlantPoints.text = $"Remove objects points:\n{activePlayerPoints.GetPoints(TurnPoints.PointType.SelfObjectRemove)}";
        textWaterOwnPoints.text = $"Water objects points:\n{activePlayerPoints.GetPoints(TurnPoints.PointType.SelfAddWater)}";
    }

    void FlashGreen()
    {
        animatorPlaceOwnPlantPoints.SetTrigger("FlashGreen");
        animatorPlaceCompanionPlantPoints.SetTrigger("FlashGreen");
        animatorRemoveOwnPlantPoints.SetTrigger("FlashGreen");
        animatorWaterOwnPoints.SetTrigger("FlashGreen");
    }

    void PopOwnPlace() => Pop(animatorPlaceOwnPlantPoints);
    void PopCompPlace() => Pop(animatorPlaceCompanionPlantPoints);
    void PopOwnRemove() => Pop(animatorRemoveOwnPlantPoints);
    void PopWater() => Pop(animatorWaterOwnPoints);

    private void Pop(Animator animator)
    {
        animator.SetTrigger("Pop");
    }

    void FlashRed(EventsManager.EventParams param)
    {
        switch ((TurnPoints.PointType)param.EnumData)
        {
            case TurnPoints.PointType.SelfObjectPlace:
                animatorPlaceOwnPlantPoints.SetTrigger("FlashRed");
                break;
            case TurnPoints.PointType.OtherObjectPlace:
                animatorPlaceCompanionPlantPoints.SetTrigger("FlashRed");
                break;
            case TurnPoints.PointType.SelfObjectRemove:
                animatorRemoveOwnPlantPoints.SetTrigger("FlashRed");
                break;
            case TurnPoints.PointType.SelfAddWater:
                animatorWaterOwnPoints.SetTrigger("FlashRed");
                break;
            default:
                break;
        }
    }
}
