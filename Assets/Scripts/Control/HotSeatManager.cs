﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// created jay 12/02

namespace GameCore
{
    /// <summary>
    /// Tempoary class for managing hotseating the players, managed by <see cref="GameManager"/>. Replaced by <see cref="OnlineTurnManager"/>
    /// </summary>
    [System.Obsolete("This class should no longer be useed as it was developed for MVP only. Replaced by " + nameof(OnlineTurnManager), true)]
    public class HotSeatManager : MonoBehaviour
    {

        // [SerializeField] Button EndTurnButton; // todo refactor away this ref


        [SerializeField, Range(0, 5)] float hotseatSwapTime;

        [SerializeField] GameObject playerPrefab;

        public TurnTracker TurnTracker { get; private set; } = new TurnTracker();

        public Dictionary<Player.PlayerEnum, Player> players = new Dictionary<Player.PlayerEnum, Player>();

        public Player ActivePlayer { get; private set; }
        public bool TurnActive => TurnTracker.TurnActive;


        private void OnEnable()
        {
           // EventsManager.BindEvent(EventsManager.EventType.EndTurn, EndTurn);

            EventsManager.BindEvent(EventsManager.EventType.triedToPlaceOwnObject, TryPlaceOwnObject);
            EventsManager.BindEvent(EventsManager.EventType.triedToPlaceCompanionObject, TryPlaceCompanionObject);
        }

        private void OnDisable()
        {
           // EventsManager.UnbindEvent(EventsManager.EventType.EndTurn, EndTurn);

            EventsManager.UnbindEvent(EventsManager.EventType.triedToPlaceOwnObject, TryPlaceOwnObject);
            EventsManager.UnbindEvent(EventsManager.EventType.triedToPlaceCompanionObject, TryPlaceCompanionObject);

        }

        private void Awake()
        {
            Init();
        }

        // Start is called before the first frame update
        void Start()
        {
            BeginNewTurn();
        }

        private void Init()
        {
            foreach (Player.PlayerEnum playerNumber in System.Enum.GetValues(typeof(Player.PlayerEnum)))
            {
                GameObject playerObject = Instantiate(playerPrefab);
                playerObject.transform.SetParent(transform);
                Player player = playerObject.GetComponent<Player>();
             //   player.Init(playerNumber);
                players.Add(playerNumber, player);
            }
            SetActivePlayer();
        }

        private void SetActivePlayer() => ActivePlayer = players[TurnTracker.PlayerTurn];


        void BeginNewTurn()
        {
            TurnTracker.ProgressTurn();
            SetActivePlayer();
            ActivePlayer.StartTurn();
        //    EventsManager.InvokeEvent(EventsManager.EventType.NewTurnBegin);
        }

        void EndTurn()
        {
            TurnTracker.EndTurn();
            StartCoroutine(nameof(PauseBeforeStartingNextTurn));
        }


        IEnumerator PauseBeforeStartingNextTurn()
        {
            //  EndTurnButton.enabled = false; // todo refactor away this ref

            yield return new WaitForSeconds(hotseatSwapTime);
            // EndTurnButton.enabled = true;

            BeginNewTurn();
        }

        // This can all be moved anywhere after MVP but right now neeeds to know who the active player is
        #region Attempt to complete action
        // Edited Scott 25/02 - Changed attempt to return whether successful or not
        void TryPlaceOwnObject()
        {
            AttemptAction(TurnPoints.PointType.SelfObjectPlace, EventsManager.EventType.PlacedOwnObject);
        }
        void TryPlaceCompanionObject()
        {
            AttemptAction(TurnPoints.PointType.OtherObjectPlace, EventsManager.EventType.PlacedCompanionObject);
        }


        public void AttemptAction(TurnPoints.PointType pointType, EventsManager.EventType action)
        {
            if (TurnActive && ActivePlayer.TurnPoints.HasPointsLeft(pointType))
            {
                EventsManager.InvokeEvent(action);
            }
            else
            {
                EventsManager.InvokeEvent(EventsManager.ParameterEventType.NotEnoughPointsForAction, new EventsManager.EventParams() { EnumData1 = pointType }); // invoke event to inform player they are out of this kind of point
            }

        }

        #endregion

    }
}