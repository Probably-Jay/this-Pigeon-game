﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore;
using System;

// created jay 12/02

/// <summary>
/// Class which keeps track of the number of actions a player can perform in one turn, is responsible for providing and setting these values
/// </summary>
public class TurnPoints : MonoBehaviour
{

    [Header("Start each round with the following number of each points")]
    [SerializeField] int placeOwnPlantPointsInitial;
    [SerializeField] int placeCompanionPlantPointsInitial;
    //[SerializeField] int removeOwnPlantPointsInitial;
   // [SerializeField] int waterOwnPlantPointsInitial;

    /// <summary>
    /// Needed for serialisation
    /// </summary>
    public const int NumberOfPointTypes = 2; // KEEP UP TO DATE!
    public enum PointType
    {
          SelfObjectPlace
        , OtherObjectPlace
    //    , SelfObjectRemove
    //    , SelfAddWater

    }

    Dictionary<PointType, int> points;


    public void Initialise()
    {
        points[PointType.SelfObjectPlace] = placeOwnPlantPointsInitial;
        points[PointType.OtherObjectPlace] = placeCompanionPlantPointsInitial;
        GameManager.Instance.DataManager.ResetPlayer1ActionPoints();
        GameManager.Instance.DataManager.ResetPlayer2ActionPoints();

    }

    public void Resume(Player.PlayerEnum playerWeAre)
    {
        PlayerDataPacket data = NetSystem.NetworkHandler.Instance.NetGame.CurrentNetworkGame.usableData.playerData;
        int selfActionPlayer1 = data.player1SelfActions;
        int otherActionPlayer1 = data.player1OtherActions;

        int selfActionPlayer2 = data.player2SelfActions;
        int otherActionPlayer2 = data.player2OtherActions;

        GameManager.Instance.DataManager.SetPlayer1ActionPoints(selfActionPlayer1, otherActionPlayer1);
        GameManager.Instance.DataManager.SetPlayer2ActionPoints(selfActionPlayer2, otherActionPlayer2);

        switch (playerWeAre)
        {
            case Player.PlayerEnum.Player1:
                points[PointType.SelfObjectPlace] = selfActionPlayer1;
                points[PointType.OtherObjectPlace] = otherActionPlayer1;
                break;
            case Player.PlayerEnum.Player2:
                points[PointType.SelfObjectPlace] = selfActionPlayer2;
                points[PointType.OtherObjectPlace] = otherActionPlayer2;
                break;
        }
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
    }

    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.PlacedOwnObject, DecreaseOurPlacePoints);
        EventsManager.UnbindEvent(EventsManager.EventType.PlacedCompanionObject, DecreaseCompanionPlacePoints);
    }

    private void DecreaseOurPlacePoints()
    {
        DecreasePoints(PointType.SelfObjectPlace);
        switch (GameCore.GameManager.Instance.LocalPlayer.EnumID)
        {
            case Player.PlayerEnum.Player1:
                GameCore.GameManager.Instance.DataManager.SpendPlayer1SelfAction();
                break;
            case Player.PlayerEnum.Player2:
                GameCore.GameManager.Instance.DataManager.SpendPlayer2SelfAction();
                break;
        }
    }

    

    private void DecreaseCompanionPlacePoints()
    {
        DecreasePoints(PointType.OtherObjectPlace);
        switch (GameCore.GameManager.Instance.LocalPlayer.EnumID)
        {
            case Player.PlayerEnum.Player1:
                GameCore.GameManager.Instance.DataManager.SpendPlayer1OtherAction();
                break;
            case Player.PlayerEnum.Player2:
                GameCore.GameManager.Instance.DataManager.SpendPlayer2OtherAction();
                break;
        }
    }
   



}

