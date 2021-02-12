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

    public enum EventType
    {

    }

    Dictionary<EventType, Action<EventData>> events;

    public static new EventsManager Instance { get=> Singleton<EventsManager>.Instance; } //not needed but easier to find refs

    public override void Awake()
    {
        base.InitSingleton();
        if(events!= null) Instance.events = new Dictionary<EventType, Action<EventData>>();
    }

    /// <summary>
    /// Add a new action to be triggered when an event is invoked 
    /// </summary>
    /// <param name="eventType">Event that will trigger the action</param>
    /// <param name="action">Delegate to the function that will be called when the action is triggered</param>
    public static void BindEvent(EventType eventType, Action<EventData> action)
    {
        Action<EventData> newEvent;
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
    /// Remove action from listening for an event
    /// </summary>
    /// <param name="eventType">Event that would have triggered the action</param>
    /// <param name="action">Delegate to the function that will no longer be called when the action is triggered</param>
    public static void UnbindEvent(EventType eventType, Action<EventData> action)
    {
        if (!InstanceExists) { WarnInstanceDoesNotExist(); return; }
        Action<EventData> thisEvent;
        if (Instance.events.TryGetValue(eventType, out thisEvent))
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
    /// <param name="paramaters">Optional: Additional paramaters to be passed to the function</param>
    public static void InvokeEvent(EventType eventType, EventData paramaters = default)
    {
        if (Instance.events.ContainsKey(eventType))
        {
            Instance.events[eventType]?.Invoke(paramaters);
        }
        else Debug.LogError($"Invoke failed. Event {eventType.ToString()} is not a member of the events list");
    }


    public struct EventData
    {
        public int IntData;
    }

}
