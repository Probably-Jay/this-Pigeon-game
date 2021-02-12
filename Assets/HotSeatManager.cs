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



    }

    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.EndTurn, EndTurn);



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
}
