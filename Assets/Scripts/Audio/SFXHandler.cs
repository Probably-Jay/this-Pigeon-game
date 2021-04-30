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
                      
        EventsManager.BindEvent(EventsManager.EventType.EndTurn, Sparkle); // Still not got a specific use for Sparkle, using this as temp
                      
        EventsManager.BindEvent(EventsManager.EventType.GameOver, Pop); // Still not got a specific use for Pop, using this as temp
                      
        EventsManager.BindEvent(EventsManager.EventType.SeedbagShuffle, Seeds);
                      
        EventsManager.BindEvent(EventsManager.EventType.SeedBagOpen, Bag);
        EventsManager.BindEvent(EventsManager.EventType.SeedBagClose, Bag);
                      
        EventsManager.BindEvent(EventsManager.EventType.ToolBoxOpen, Tools);
        EventsManager.BindEvent(EventsManager.EventType.ToolBoxClose, Tools);
                      
        EventsManager.BindEvent(EventsManager.EventType.OnDialogueOpen, PetSounds);
        EventsManager.BindEvent(EventsManager.EventType.DialogueNext, PetSounds);
        EventsManager.BindEvent(EventsManager.EventType.DialoguePrevious, PetSounds);
        EventsManager.BindEvent(EventsManager.EventType.PokePet, PetSounds);

        EventsManager.BindEvent(EventsManager.EventType.PlacedSmallPlant, SmallPlant);
        EventsManager.BindEvent(EventsManager.EventType.PlacedTallPlant, TallPlant);
                      
        EventsManager.BindEvent(EventsManager.ParameterEventType.OnPerformedTendingAction, TendedPlant);
    }

 

    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.SeedBagOpen, Bag);
        EventsManager.UnbindEvent(EventsManager.EventType.SeedBagClose, Bag);
                      
        EventsManager.UnbindEvent(EventsManager.EventType.QuitGame, Sparkle); // Still not got a specific use for Sparkle, using this as temp
                      
        EventsManager.UnbindEvent(EventsManager.EventType.GameOver, Pop); // Still not got a specific use for Pop, using this as temp
                      
        EventsManager.UnbindEvent(EventsManager.EventType.SeedbagShuffle, Seeds);
                      
        EventsManager.UnbindEvent(EventsManager.EventType.SeedBagOpen, Bag);
        EventsManager.UnbindEvent(EventsManager.EventType.SeedBagClose, Bag);
                      
        EventsManager.UnbindEvent(EventsManager.EventType.ToolBoxOpen, Tools);
        EventsManager.UnbindEvent(EventsManager.EventType.ToolBoxClose, Tools);
                      
        EventsManager.UnbindEvent(EventsManager.EventType.OnDialogueOpen, PetSounds);

        EventsManager.UnbindEvent(EventsManager.EventType.PlacedSmallPlant, SmallPlant);
        EventsManager.UnbindEvent(EventsManager.EventType.PlacedTallPlant, TallPlant);
                      
        EventsManager.UnbindEvent(EventsManager.ParameterEventType.OnPerformedTendingAction, TendedPlant);

    }

    // Play sound effect at specified index
    private void PlaySound(int idx, bool rnd)
    {
        clipSource.pitch = 1.0f;
        if (rnd) {
            clipSource.pitch += UnityEngine.Random.Range(-0.15f, 0.15f);
        }
        clipSource.PlayOneShot(clipArray[idx]);
    }

    void PetSounds() {
         // Swap if pets are not longer tied to p1 / p2
        if (GameManager.Instance.PlayerWhosGardenIsCurrentlyVisible == Player.PlayerEnum.Player1) {
            Meow();
        } else {
            Bark();
        }
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
                Cut();
                break;
            //case Plants.PlantActions.TendingActions.Staking:
           //     Stake();
           //     break;
            case Plants.PlantActions.TendingActions.Removing:
                Rake();
                break;

            // case Plants.PlantActions.TendingActions.Hose: // Was told to include hose noise, we don't seem to have a hose?
            //     Hose();
            //     break;

            // This shouldn't happen
            default:
                TestNoise();
                break;
        }
    }

    /*
    0: TEST
    1: BAG
    2-4: BARK
    5: DING / SPARKLE
    6: HOSE
    7-10: MEOW
    11: CUT
    12: POP
    13-16: RAKE 
    17-19: SEED SHUFFLE
    20: SMALL PLANT
    21: SPRAY
    22: STAKE
    23: TALL PLANT
    24: TOOLBOX
    25: WATER
    */

    void TestNoise()
    {
        PlaySound(0, false);
    }

    void Bag()
    {
        PlaySound(1, true);
    }

    void Bark()
    {
        int type = UnityEngine.Random.Range(0, 3);
        PlaySound(2 + type, true);
    }

    void Sparkle()
    {
        PlaySound(5, false);
    }

    void Hose()
    {
        PlaySound(6, true);
    }

    void Meow()
    {
        int type = UnityEngine.Random.Range(0, 3);
        PlaySound(7 + type, true);
    }

    void Cut()
    {
        PlaySound(11, true);
    }

    void Pop()
    {
        PlaySound(12, false);
    }

    void Rake()
    {
        int type = UnityEngine.Random.Range(0, 3);
        PlaySound(13 + type, false);
    }

    void Seeds()
    {
        int type = UnityEngine.Random.Range(0, 2);
        PlaySound(17 + type, true);
    }

    void SmallPlant()
    {
        PlaySound(20, false);
    }

    void Spray()
    {
        PlaySound(21, true);
    }

    void Stake()
    {
        PlaySound(22, true);
    }

    void TallPlant()
    {
        PlaySound(23, false);
    }

    void Tools()
    {
        PlaySound(24, true);
    }

    void Water()
    {
        PlaySound(25, true);
    }
}
