using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// created jay 12/02

public class TurnPoints : MonoBehaviour
{

    [Header("Start each round with the following number of each points")]
    [SerializeField] int placeOwnPlantPointsInitial;
    [SerializeField] int placeCompanionPlantPointsInitial;
    [SerializeField] int removeOwnPlantPointsInitial;
    [SerializeField] int waterOwnPlantPointsInitial;

    public enum PointType
    {
          SelfObjectPlace
        , OtherObjectPlace
        , SelfObjectRemove
        , SelfAddWater

    }

    Dictionary<PointType, int> points;


    public void StartTurn()
    {
        points[PointType.SelfObjectPlace] = placeOwnPlantPointsInitial;
        points[PointType.OtherObjectPlace] = placeCompanionPlantPointsInitial;
        points[PointType.SelfObjectRemove] = removeOwnPlantPointsInitial;
        points[PointType.SelfAddWater] = waterOwnPlantPointsInitial;
    }

    private void Awake()
    {
        points = new Dictionary<PointType, int>();
        foreach (PointType type in System.Enum.GetValues(typeof(PointType)))
        {
            points.Add(type, 0);
        }
    }


    public int GetPoints(PointType type) => points[type];
    public bool HasPointsLeft(PointType type) => GetPoints(type) > 0;
    public void SetPoints(PointType type, int value) => points[type] = value >= 0 ? value : 0;
    public void DecreasePoints(PointType type) => SetPoints(type, GetPoints(type) - 1);


    private void OnEnable()
    {
        EventsManager.BindEvent(EventsManager.EventType.PlacedOwnObject, DecreaseOurPlacePoints);
        EventsManager.BindEvent(EventsManager.EventType.PlacedCompanionObject, DecreaseCompanionPlacePoints);
        EventsManager.BindEvent(EventsManager.EventType.RemovedOwnObject, DecreaseOurRemovePoints);
        EventsManager.BindEvent(EventsManager.EventType.WateredOwnPlant, DecreaseOurWaterPoints);
    }

    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.PlacedOwnObject, DecreaseOurPlacePoints);
        EventsManager.UnbindEvent(EventsManager.EventType.PlacedCompanionObject, DecreaseCompanionPlacePoints);
        EventsManager.UnbindEvent(EventsManager.EventType.RemovedOwnObject, DecreaseOurRemovePoints);
        EventsManager.UnbindEvent(EventsManager.EventType.WateredOwnPlant, DecreaseOurWaterPoints);
    }

    private void DecreaseOurPlacePoints() => DecreasePoints(PointType.SelfObjectPlace);
    private void DecreaseCompanionPlacePoints() => DecreasePoints(PointType.OtherObjectPlace);
    private void DecreaseOurRemovePoints() => DecreasePoints(PointType.SelfObjectRemove);
    private void DecreaseOurWaterPoints() => DecreasePoints(PointType.SelfAddWater);




}

