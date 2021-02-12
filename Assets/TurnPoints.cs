using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnPoints : MonoBehaviour
{
    [Header("Start each round with the following number of points")]
    [SerializeField] int placeOwnPlantPointsInitial;
    [SerializeField] int placeCompanionPlantPointsInitial;
    [SerializeField] int removeOwnPlantPointsInitial;
    [SerializeField] int waterOwnPlantPointsInitial;

    public int OwnPlantPlacePoints { get; private set; }
    public int CompanionPlacePoints { get; private set; }
    public int OwnRemovePoints { get; private set; }
    public int OwnWaterPoints { get; private set; }

    private void OnEnable()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartTurn()
    {
        OwnPlantPlacePoints = placeOwnPlantPointsInitial;
        CompanionPlacePoints = placeCompanionPlantPointsInitial;
        OwnRemovePoints = removeOwnPlantPointsInitial;
        OwnWaterPoints = waterOwnPlantPointsInitial;
    }
}
