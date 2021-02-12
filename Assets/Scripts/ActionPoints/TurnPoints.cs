using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// created jay 12/02

public class TurnPoints : MonoBehaviour
{

    [Header("Start each round with the following number of points")]
    [SerializeField] int placeOwnPlantPointsInitial;
    [SerializeField] int placeCompanionPlantPointsInitial;
    [SerializeField] int removeOwnPlantPointsInitial;
    [SerializeField] int waterOwnPlantPointsInitial;

    public enum PointType
    {
        OurObjectPlace
        , CompanionPlace
        , OurObjectRemove
        , OurWater

    }



    public void StartTurn()
    {
        points[PointType.OurObjectPlace] = placeOwnPlantPointsInitial;
        points[PointType.CompanionPlace] = placeCompanionPlantPointsInitial;
        points[PointType.OurObjectRemove] = removeOwnPlantPointsInitial;
        points[PointType.OurWater] = waterOwnPlantPointsInitial;
    }


    Dictionary<PointType, int> points;



    private void Start()
    {
        points = new Dictionary<PointType, int>();
        foreach (PointType type in System.Enum.GetValues(typeof(PointType)))
        {
            points.Add(type, 0);
        }
    }


    public int GetPoints(PointType type) => points[type];
    public bool HasPointsLeft(PointType type) => GetPoints(type) > 0;
    private void SetPoints(PointType type, int value) => points[type] = value >= 0 ? value : 0;
    private void DecreasePoints(PointType type) => SetPoints(type, GetPoints(type) - 1);


    private void OnEnable()
    {
        EventsManager.BindEvent(EventsManager.EventType.PlaceOwnObject, DecreaseOurPlacePoints);
        EventsManager.BindEvent(EventsManager.EventType.PlaceCompanionObject, DecreaseCompanionPlacePoints);
        EventsManager.BindEvent(EventsManager.EventType.RemoveOwnObject, DecreaseOurRemovePoints);
        EventsManager.BindEvent(EventsManager.EventType.WaterOwnPlant, DecreaseOurWaterPoints);
    }

    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.PlaceOwnObject, DecreaseOurPlacePoints);
        EventsManager.UnbindEvent(EventsManager.EventType.PlaceCompanionObject, DecreaseCompanionPlacePoints);
        EventsManager.UnbindEvent(EventsManager.EventType.RemoveOwnObject, DecreaseOurRemovePoints);
        EventsManager.UnbindEvent(EventsManager.EventType.WaterOwnPlant, DecreaseOurWaterPoints);
    }



    private void DecreaseOurPlacePoints() => DecreasePoints(PointType.OurObjectPlace);
    private void DecreaseCompanionPlacePoints() => DecreasePoints(PointType.CompanionPlace);
    private void DecreaseOurRemovePoints() => DecreasePoints(PointType.OurObjectRemove);
    private void DecreaseOurWaterPoints() => DecreasePoints(PointType.OurWater);




}

