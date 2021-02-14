using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// created jay 12/02

// uncomment when needed
//[System.Obsolete("This class should no longer be useed as it was developed for MVP only")]
/// <summary>
/// Tempoary class for managing hotseating the players
/// </summary>
public class HotSeatManager : MonoBehaviour
{

    [SerializeField, Range(0, 5)] float hotseatSwapTime;

    [SerializeField] GameObject playerPrefab;

    TurnTracker turnTracker = new TurnTracker();
    Dictionary<Player.PlayerEnum, Player> players = new Dictionary<Player.PlayerEnum, Player>();

    public Player ActivePlayer { get; private set; }
    public bool TurnActive => turnTracker.TurnActive;


    private void OnEnable()
    {
        EventsManager.BindEvent(EventsManager.EventType.EndTurn, EndTurn);


        EventsManager.BindEvent(EventsManager.EventType.triedToPlaceOwnObject, TryPlaceOwnObject);
        EventsManager.BindEvent(EventsManager.EventType.triedToPlaceCompanionObject, TryPlaceCompanionObject);
        EventsManager.BindEvent(EventsManager.EventType.triedToRemoveOwnObject, TryRemoveOwnObject);
        EventsManager.BindEvent(EventsManager.EventType.triedToWaterOwnPlant, TryWaterOwnObject);

    }

    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.EndTurn, EndTurn);


        EventsManager.UnbindEvent(EventsManager.EventType.triedToPlaceOwnObject, TryPlaceOwnObject);
        EventsManager.UnbindEvent(EventsManager.EventType.triedToPlaceCompanionObject, TryPlaceCompanionObject);
        EventsManager.UnbindEvent(EventsManager.EventType.triedToRemoveOwnObject, TryRemoveOwnObject);
        EventsManager.UnbindEvent(EventsManager.EventType.triedToWaterOwnPlant, TryWaterOwnObject);

    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (Player.PlayerEnum playerNumber in System.Enum.GetValues(typeof(Player.PlayerEnum)))
        {
            var playerObject = Instantiate(playerPrefab);
            var player = playerObject.GetComponent<Player>();
            player.Init(playerNumber);
            players.Add(playerNumber,player);
        }
        BeginNewTurn();
    }

    private void SetActivePlayer() => ActivePlayer = players[turnTracker.PlayerTurn];


    void BeginNewTurn()
    {
        turnTracker.ProgressTurn();
        SetActivePlayer();
        ActivePlayer.StartTurn();
        EventsManager.InvokeEvent(EventsManager.EventType.NewTurnBegin);
    }

    void EndTurn()
    {
        turnTracker.EndTurn();
        StartCoroutine(nameof(PauseBeforeStartingNextTurn));
    }

    IEnumerator PauseBeforeStartingNextTurn()
    {
        yield return new WaitForSeconds(hotseatSwapTime);
        BeginNewTurn();
    }

    // This can all be moveed anywhere after MVP but right now neeeds to know who the active player is
    #region Attempt to complete action

    void TryPlaceOwnObject() => AttemptAction(TurnPoints.PointType.OurObjectPlace, EventsManager.EventType.PlacedOwnObject);
    void TryPlaceCompanionObject() => AttemptAction(TurnPoints.PointType.CompanionPlace, EventsManager.EventType.PlacedCompanionObject);
    void TryRemoveOwnObject() => AttemptAction(TurnPoints.PointType.OurObjectRemove, EventsManager.EventType.RemovedOwnObject);
    void TryWaterOwnObject() => AttemptAction(TurnPoints.PointType.OurWater, EventsManager.EventType.WateredOwnPlant);

    private void AttemptAction(TurnPoints.PointType pointType, EventsManager.EventType action)
    {
        if (ActivePlayer.TurnPoints.HasPointsLeft(pointType))
            EventsManager.InvokeEvent(action);
        else
            EventsManager.InvokeEvent(EventsManager.ParamaterEventType.NotEnoughPointsForAction, new EventsManager.EventParams() { EnumData = pointType }); // invoke event to inform player they are out of this kind of point
     }

    #endregion




    //bool HasPointsLeft(TurnPoints.PointType pointType)
    //{
    //    if(!ActivePlayer.TurnPoints.HasPointsLeft(pointType))
    //        return false;
    //    else
    //    {
    //        ActivePlayer.TurnPoints.Dec
    //    }

    //}

}
