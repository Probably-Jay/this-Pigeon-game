using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// created by jay 12/02, adapted from https://learn.unity.com/tutorial/create-a-simple-messaging-system-with-events" and https://stackoverflow.com/a/42034899/7711148

/// <summary>
/// <see cref="Singleton{}"/> class to handle game events, adapted from <see href="https://learn.unity.com/tutorial/create-a-simple-messaging-system-with-events"/>
/// </summary>
public class EventsManager : Singleton<EventsManager>
{

    // Update these enum with new events to expand this class' functionality

    // events with no paramaters
    public enum EventType
    {
        EndTurn
        ,  NewTurnBegin


        , PlacedOwnObject
        , PlacedCompanionObject
        , RemovedOwnObject
        , WateredOwnPlant // perhaps rename to "maintained own object"

        , triedToPlaceOwnObject
        , triedToPlaceCompanionObject
        , triedToRemoveOwnObject
        , triedToWaterOwnPlant 


    }

    // events with paramaters
    public enum ParamaterEventType 
    {
        NotEnoughPointsForAction
    }

    // update thsese with more data as needed
    public struct EventParams
    {
        public int IntData;
        public Enum EnumData;
    }

    public static new EventsManager Instance { get=> Singleton<EventsManager>.Instance; } // hide property



    public override void Awake()
    {
        base.InitSingleton();
        if(events == null) Instance.events = new Dictionary<EventType, Action>();
        if(paramaterEvents == null) Instance.paramaterEvents = new Dictionary<ParamaterEventType, Action<EventParams>>();
    }


    #region Non-Paramatized Events
    Dictionary<EventType, Action> events;

    /// <summary>
    /// Add a new action to be triggered when an event is invoked 
    /// </summary>
    /// <param name="eventType">Event that will trigger the action</param>
    /// <param name="action">Delegate to the function that will be called when the action is triggered</param>
    public static void BindEvent(EventType eventType, Action action)
    {
        Action newEvent;
        if (Instance.events.TryGetValue(eventType, out newEvent))
        {
            newEvent += action;
            Instance.events[eventType] = newEvent;
        }
        else
        {
            newEvent += action;
            Instance.events.Add(eventType, newEvent);
        }
    }

    /// <summary>
    /// Remove action from listening for an event. This must be done whenever an object that is listening is disabled
    /// </summary>
    /// <param name="eventType">Event that would have triggered the action</param>
    /// <param name="action">Delegate to the function that will no longer be called when the action is triggered</param>
    public static void UnbindEvent(EventType eventType, Action action)
    {
        if (!InstanceExists) { WarnInstanceDoesNotExist(); return; }
        Action thisEvent;
        Dictionary<EventType, Action> events1 = Instance.events;
        if (events1.TryGetValue(eventType, out thisEvent))
        {
            thisEvent -= action;
            Instance.events[eventType] = thisEvent;
        }
        else Debug.LogWarning($"Unsubscribe failed. Event {eventType.ToString()} is not a member of the events list");
    }

    /// <summary>
    /// Call every function listening for this event
    /// </summary>
    /// <param name="eventType">Type of event to trigger</param>
    public static void InvokeEvent(EventType eventType)
    {
        if (Instance.events.ContainsKey(eventType))
        {
            Instance.events[eventType]?.Invoke();
        }
        else Debug.LogWarning($"Event {eventType.ToString()} was invoked but is unused (no listeners have ever subscribed to it)");
    }
    #endregion


    #region Paramatized Events
    Dictionary<ParamaterEventType, Action<EventParams>> paramaterEvents;

    /// <summary>
    /// Add a new action to be triggered when an event is invoked 
    /// </summary>
    /// <param name="eventType">Event that will trigger the action</param>
    /// <param name="action">Delegate to the function that will be called when the action is triggered</param>
    public static void BindEvent(ParamaterEventType eventType, Action<EventParams> action)
    {
        Action<EventParams> newEvent;
        if (Instance.paramaterEvents.TryGetValue(eventType, out newEvent))
        {
            newEvent += action;
            Instance.paramaterEvents[eventType] = newEvent;
        }
        else
        {
            newEvent += action;
            Instance.paramaterEvents.Add(eventType, newEvent);
        }
    }

    /// <summary>
    /// Remove action from listening for an event
    /// </summary>
    /// <param name="eventType">Event that would have triggered the action</param>
    /// <param name="action">Delegate to the function that will no longer be called when the action is triggered</param>
    public static void UnbindEvent(ParamaterEventType eventType, Action<EventParams> action)
    {
        if (!InstanceExists) { WarnInstanceDoesNotExist(); return; }
        Action<EventParams> thisEvent;
        if (Instance.paramaterEvents.TryGetValue(eventType, out thisEvent))
        {
            thisEvent -= action;
            Instance.paramaterEvents[eventType] = thisEvent;
        }
        else Debug.LogWarning($"Unsubscribe failed. Event {eventType.ToString()} is not a member of the events list");
    }

    /// <summary>
    /// Call every function listening for this event
    /// </summary>
    /// <param name="eventType">Type of event to trigger</param>
    /// <param name="paramaters">Optional: Additional paramaters to be passed to the function</param>
    public static void InvokeEvent(ParamaterEventType eventType, EventParams paramaters)
    {
        //InvokeEvent(eventType);
        if (Instance.paramaterEvents.ContainsKey(eventType))
        {
            Instance.paramaterEvents[eventType]?.Invoke(paramaters);
        }
        else Debug.LogWarning($"Event {eventType.ToString()} was invoked but is unused (no listeners have ever subscribed to it)");
    }
    #endregion


    private void ClearEvents()
    {
        events.Clear();
        paramaterEvents.Clear();
    }
    private void OnDisable()
    {
        ClearEvents();
    }


}
