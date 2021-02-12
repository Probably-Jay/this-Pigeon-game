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

    private void OnEnable()
    {
        EventsManager.BindEvent(EventsManager.EventType.EndTurn, ProgressTurn);
        EventsManager.BindEvent(EventsManager.EventType.NewTurnBegin, BeginNewTurn);
    }

    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.EndTurn, ProgressTurn);
        EventsManager.UnbindEvent(EventsManager.EventType.NewTurnBegin, BeginNewTurn);

    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (Player.PlayerEnum playerNumber in System.Enum.GetValues(typeof(Player.PlayerEnum)))
        {
            var player = Instantiate(playerPrefab);
            players.Add(playerNumber, player.GetComponent<Player>());
        }
        SetActivePlayer();
    }

    private void SetActivePlayer() => ActivePlayer = players[turnTracker.PlayerTurn];


    void BeginNewTurn()
    {
        turnTracker.ProgressTurn();
        SetActivePlayer();
    }

    void ProgressTurn()
    {
        StartCoroutine(nameof(PauseBeforeStartingNextTurn));
    }

    IEnumerator PauseBeforeStartingNextTurn()
    {
        yield return new WaitForSeconds(hotseatSwapTime);
        EventsManager.InvokeEvent(EventsManager.EventType.NewTurnBegin);
    }
}
