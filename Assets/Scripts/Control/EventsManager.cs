using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Events;

// created by jay 12/02, adapted from https://learn.unity.com/tutorial/create-a-simple-messaging-system-with-events" and https://stackoverflow.com/a/42034899/7711148

/// <summary>
/// <see cref="Singleton{}"/> class to handle game events, adapted from <see href="https://learn.unity.com/tutorial/create-a-simple-messaging-system-with-events"/>
/// </summary>
public class EventsManager : Singleton<EventsManager>
{




    // Update these enum with new events to expand this class' functionality

    // events with no parameters
    public enum EventType
    {
        EndTurn
        , NewTurnBegin

        , BeginSceneLoad
        , SceneLoadComplete
        , EnterNewScene
        //, CrossfadeAnimationBegin
        , CrossfadeAnimationEnd

     
        , PlacedOwnObject
        , PlacedOwnObjectMoodRelevant
        , PlacedCompanionObject
        , RemovedOwnObject
        , WateredOwnPlant // perhaps rename to "maintained own object"

        , triedToPlaceOwnObject
        , triedToPlaceCompanionObject
        , triedToRemoveOwnObject
        , triedToWaterOwnPlant 

        , UpdateScore
       // , UpdatePlants

        , PlantChangedStats
        , GardenStatsUpdated

        , StartGame
        , GameOver

        , ToolBoxOpen
        , ToolBoxClose
        , SeedBagOpen
        , SeedBagClose

        , OnDialogueOpen
        , DialogueSkip
        , DialogueNext
        , DialoguePrevious
        , PlantingBegin

       // , AddedToEmotionGoal
        ,ToolDropped

        , PlantReadyToGrow


        #region NetCodeEvents

        ,PostLogout
        #endregion

        , QuitGame
    }

    // events with parameters
    public enum ParameterEventType 
    {
        NotEnoughPointsForAction // enum variable
        , SwappedGardenView // enum variable
        , AcheivedGoal // enum variable
        , OnPerformedTendingAction // enum varailble
    }

    // update thsese with more data as needed
    public struct EventParams
    {
        public int IntData;
        public Enum EnumData;
    }

    protected static new EventsManager Instance { get=> Singleton<EventsManager>.Instance; } // hide property



    //public override void Awake()
    //{
    //    base.InitSingleton();
    //    if (events == null) Instance.events = new Dictionary<EventType, Action>();
    //    if (parameterEvents == null) Instance.parameterEvents = new Dictionary<ParameterEventType, Action<EventParams>>();
    //}

    public override void Initialise()
    {
        base.InitSingleton();
        if (events == null) Instance.events = new Dictionary<EventType, Action>();
        if (parameterEvents == null) Instance.parameterEvents = new Dictionary<ParameterEventType, Action<EventParams>>();
        BindEvent(EventType.EnterNewScene, CleanEvents);
    }


    private void OnDisable()
    {
        UnbindEvent(EventType.EnterNewScene, CleanEvents); // not needed, just to make it easier to keep track of bind/unbind
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
            newEvent += action; // this is now the out paramater
            Instance.events[eventType] = newEvent;
        }
        else
        {
            newEvent += action; // this was empty before, defualt initialised by the "trygetvalue()"
            Instance.events.Add(eventType, newEvent); // add new key-value
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
        Dictionary<EventType, Action> instanceEvents = Instance.events;
        if (instanceEvents.TryGetValue(eventType, out thisEvent))
        {
            thisEvent -= action;

            if(thisEvent == null)
            {
                instanceEvents.Remove(eventType);
            }
            else
            {
                instanceEvents[eventType] = thisEvent;
            }

            
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
        else Debug.LogWarning($"Event {eventType.ToString()} was invoked in scene {SceneManager.GetActiveScene().name} but is unused (no listeners have ever subscribed to it)");
    }
    #endregion


    #region Paramatized Events
    Dictionary<ParameterEventType, Action<EventParams>> parameterEvents;

    /// <summary>
    /// Add a new action to be triggered when an event is invoked 
    /// </summary>
    /// <param name="eventType">Event that will trigger the action</param>
    /// <param name="action">Delegate to the function that will be called when the action is triggered</param>
    public static void BindEvent(ParameterEventType eventType, Action<EventParams> action)
    {
        Action<EventParams> newEvent;
        if (Instance.parameterEvents.TryGetValue(eventType, out newEvent))
        {
            newEvent += action;
            Instance.parameterEvents[eventType] = newEvent;
        }
        else
        {
            newEvent += action;
            Instance.parameterEvents.Add(eventType, newEvent);
        }
    }

    /// <summary>
    /// Remove action from listening for an event
    /// </summary>
    /// <param name="eventType">Event that would have triggered the action</param>
    /// <param name="action">Delegate to the function that will no longer be called when the action is triggered</param>
    public static void UnbindEvent(ParameterEventType eventType, Action<EventParams> action)
    {
        if (!InstanceExists) { WarnInstanceDoesNotExist(); return; }
        Action<EventParams> thisEvent;
        Dictionary<ParameterEventType, Action<EventParams>> instanceEvents = Instance.parameterEvents;
        if (instanceEvents.TryGetValue(eventType, out thisEvent))
        {
            thisEvent -= action;

            if (thisEvent == null)
            {
                instanceEvents.Remove(eventType);
            }
            else
            {
                instanceEvents[eventType] = thisEvent;
            }
        }
        else Debug.LogWarning($"Unsubscribe failed. Event {eventType.ToString()} is not a member of the events list");
    }

    /// <summary>
    /// Call every function listening for this event
    /// </summary>
    /// <param name="eventType">Type of event to trigger</param>
    /// <param name="parameters">Optional: Additional parameters to be passed to the function</param>
    public static void InvokeEvent(ParameterEventType eventType, EventParams parameters)
    {
        //InvokeEvent(eventType);
        if (Instance.parameterEvents.ContainsKey(eventType))
        {
            Instance.parameterEvents[eventType]?.Invoke(parameters);
        }
        else Debug.LogWarning($"Event {eventType.ToString()} was invoked in scene {SceneManager.GetActiveScene().name} but is unused (no listeners have ever subscribed to it)");
    }
    #endregion


    private void ClearEvents()
    {
        events.Clear();
        parameterEvents.Clear();
    }

    private void CleanEvents()
    {

        //paramaterless
        List<(EventType, Action)> toUnbind = new List<(EventType, Action)>();

        GatherDanglingEvets(toUnbind);
        UnbindDanglingEvents(toUnbind);


        //paramatised
        List<(ParameterEventType, Action<EventParams>)> toUnbindParamatized = new List<(ParameterEventType, Action<EventParams>)>();

        GatherDanglingEvets(toUnbindParamatized);
        UnbindDanglingEvents(toUnbindParamatized);

    }



    private void GatherDanglingEvets(List<(EventType, Action)> toUnbind)
    {
        foreach (KeyValuePair<EventType, Action> ourEvent in events)
        {
            Action methods = ourEvent.Value;
            foreach (Action method in methods?.GetInvocationList())
            {
                if (method.Target.Equals(null))
                {
                    toUnbind.Add((ourEvent.Key, method));
                    continue;
                }
            }
        }
    }

    private void GatherDanglingEvets(List<(ParameterEventType, Action<EventParams>)> toUnbindParamatized)
    {
        foreach (KeyValuePair<ParameterEventType, Action<EventParams>> ourEvent in parameterEvents)
        {
            Action<EventParams> methods = ourEvent.Value;
            foreach (Action<EventParams> method in methods?.GetInvocationList())
            {
                if (method.Target.Equals(null))
                {
                    toUnbindParamatized.Add((ourEvent.Key, method));
                }
            }
        }
    }
    private void UnbindDanglingEvents(List<(EventType, Action)> toUnbind)
    {
        foreach (var item in toUnbind)
        {
            Debug.LogWarning($"The object owning event action method: {item.Item2.Method.Name} in class {item.Item2.Method.DeclaringType.Name} " +
                       $"has been destroyed, but the method has not been unsubscribed.");
            Debug.Log("Unsubscribing from event");
            UnbindEvent(item.Item1, item.Item2);
        }
        toUnbind.Clear();
    }

    private void UnbindDanglingEvents(List<(ParameterEventType, Action<EventParams>)> toUnbindParamatized)
    {
        foreach (var item in toUnbindParamatized)
        {
            Debug.LogWarning($"The object owning event action method: {item.Item2.Method.Name} in class {item.Item2.Method.DeclaringType.Name} " +
                       $"has been destroyed, but the method has not been unsubscribed.");
            Debug.Log("Unsubscribing from event");
            UnbindEvent(item.Item1, item.Item2);
        }
        toUnbindParamatized.Clear();
    }


    private void OnDestroy()
    {
        ClearEvents();
    }

}
