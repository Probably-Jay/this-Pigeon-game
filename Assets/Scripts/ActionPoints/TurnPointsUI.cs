using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// created jay 12/02

public class TurnPointsUI : MonoBehaviour
{


    [SerializeField] Text placeOwnPlantPoints;
    [SerializeField] Text placeCompanionPlantPoints;
    [SerializeField] Text removeOwnPlantPoints;
    [SerializeField] Text waterOwnPoints;

    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var activePlayerPoints = GameManager.Instance.HotSeatManager.ActivePlayer.TurnPoints;
        placeOwnPlantPoints.text = $"Place objects points:\n{activePlayerPoints.GetPoints(TurnPoints.PointType.OurObjectPlace)}";
        placeCompanionPlantPoints.text = $"Place companion's objects points:\n{activePlayerPoints.GetPoints(TurnPoints.PointType.CompanionPlace)}";
        removeOwnPlantPoints.text = $"Remove objects points:\n{activePlayerPoints.GetPoints(TurnPoints.PointType.OurObjectRemove)}";
        waterOwnPoints.text = $"Remove objects points:\n{activePlayerPoints.GetPoints(TurnPoints.PointType.OurWater)}";

    }
}
