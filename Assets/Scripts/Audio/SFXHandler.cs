using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Scott Jarvis, 21/04/21


public class SFXHandler : MonoBehaviour
{

    public AudioClip[] clipArray;
    public AudioSource clipSource;

    void OnEnable()
    {
        EventsManager.BindEvent(EventsManager.EventType.SeedBagOpen, Bag);
        EventsManager.BindEvent(EventsManager.EventType.SeedBagClose, Bag);

        EventsManager.BindEvent(EventsManager.EventType.ToolBoxOpen, Tools);
        EventsManager.BindEvent(EventsManager.EventType.ToolBoxClose, Tools);

        EventsManager.BindEvent(EventsManager.EventType.OnDialogueOpen, Meow);

        EventsManager.BindEvent(EventsManager.EventType.PlacedOwnObject, Rake);
        EventsManager.BindEvent(EventsManager.EventType.PlacedCompanionObject, Rake);
      //  EventsManager.BindEvent(EventsManager.EventType.PlacedOwnObjectMoodRelevant, Rake);

        EventsManager.BindEvent(EventsManager.ParameterEventType.OnPerformedTendingAction, TendedPlant);

       

        // No Events Assigned

        //EventsManager.BindEvent(EventsManager.EventType.Sparkle, Sparkle);

        //EventsManager.BindEvent(EventsManager.EventType.Cut, Cut);

        //EventsManager.BindEvent(EventsManager.EventType.Pop, Pop);

        //EventsManager.BindEvent(EventsManager.EventType.Rake, Rake);

        //EventsManager.BindEvent(EventsManager.EventType.Stake, Stake);
    }

 

    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.SeedBagOpen, Bag);
        EventsManager.UnbindEvent(EventsManager.EventType.SeedBagClose, Bag);
                      
        EventsManager.UnbindEvent(EventsManager.EventType.ToolBoxOpen, Tools);
        EventsManager.UnbindEvent(EventsManager.EventType.ToolBoxClose, Tools);

        EventsManager.UnbindEvent(EventsManager.EventType.OnDialogueOpen, Meow);


        EventsManager.UnbindEvent(EventsManager.EventType.PlacedOwnObject, Rake);
        EventsManager.UnbindEvent(EventsManager.EventType.PlacedCompanionObject, Rake);
        EventsManager.UnbindEvent(EventsManager.EventType.PlacedOwnObjectMoodRelevant, Rake);
                      
        EventsManager.UnbindEvent(EventsManager.ParameterEventType.OnPerformedTendingAction, TendedPlant);

    }

    // Play sound effect at specified index
    public void PlaySound(int idx)
    {
        clipSource.pitch = 1.0f;
        clipSource.pitch += UnityEngine.Random.Range(-0.1f, 0.1f);
        clipSource.PlayOneShot(clipArray[idx]);
    }

    private void TendedPlant(EventsManager.EventParams eventParams)
    {
        var action = (Plants.PlantActions.TendingActions)eventParams.EnumData1;

        switch (action)
        {
            case Plants.PlantActions.TendingActions.Watering:
                Water();
                break;
            case Plants.PlantActions.TendingActions.Spraying:
                Spray();
                break;
            case Plants.PlantActions.TendingActions.Trimming:
                
                break;
        }
    }

    /*
    0 - BAG
    1 - SPARKLE
    2 - SPRAY
    3 - TOOLS
    4 - CUT
    5 - POP
    6 - RAKE1
    7 - RAKE2
    8 - RAKE3
    9 - RAKE4
    10 - STAKE
    11 - WATER
    12 - MEOW

    13 - SINGLE
    14 - TALL
    15 - WIDE
    */

    void Bag() {
        PlaySound(0);
    }

    void Sparkle()
    {
        PlaySound(1);
    }


    void Spray()
    {
        PlaySound(2);
    }

    void Tools()
    {
        PlaySound(3);
    }

    void Cut()
    {
        PlaySound(4);
    }

    void Pop()
    {
        PlaySound(5);
    }

    void Rake()
    {
        int type = UnityEngine.Random.Range(0, 3);
        PlaySound(6 + type);
    }

    void Stake()
    {
        PlaySound(10);
    }

    void Water()
    {
        PlaySound(11);
    }

    void Meow()
    {
        PlaySound(12);
    }

    void Plant()
    {
        int type = UnityEngine.Random.Range(0, 2);
        PlaySound(13);
    }
}
