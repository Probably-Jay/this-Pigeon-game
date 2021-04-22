﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//created Zap 26/03
// edited Xander 22/04/2021
public class UICommands : MonoBehaviour
{
    //[SerializeField] Button button;

    private void OnEnable()
    {

    }
    private void OnDisable()
    {

    }


    public void OpenToolBox() => EventsManager.InvokeEvent(EventsManager.EventType.ToolBoxOpen);
    public void CloseToolBox() => EventsManager.InvokeEvent(EventsManager.EventType.ToolBoxClose);

    

    public void OpenSeedBag()
    {
        // Xanders edit ~ fixes bug that allows you to plant unlimited plants in the other players garden if
        // the active player has not placed their plant yet and not allow any planting in the other players garden if they have
         

        // for planting in active players own garden 

        if (GameManager.Instance.InOwnGarden && GameManager.Instance.ActivePlayer.TurnPoints.HasPointsLeft(TurnPoints.PointType.SelfObjectPlace))
        {
            EventsManager.InvokeEvent(EventsManager.EventType.SeedBagOpen);
        }
        else if (GameManager.Instance.InOwnGarden)
        {
            EventsManager.InvokeEvent(EventsManager.ParameterEventType.NotEnoughPointsForAction, new EventsManager.EventParams() { EnumData = TurnPoints.PointType.SelfObjectPlace });
        }


        // for tha active player planting in the non-active players garden 
        if (!GameManager.Instance.InOwnGarden && GameManager.Instance.ActivePlayer.TurnPoints.HasPointsLeft(TurnPoints.PointType.OtherObjectPlace))
        {
            EventsManager.InvokeEvent(EventsManager.EventType.SeedBagOpen);
        }else if(!GameManager.Instance.InOwnGarden)
        {
            EventsManager.InvokeEvent(EventsManager.ParameterEventType.NotEnoughPointsForAction, new EventsManager.EventParams() { EnumData = TurnPoints.PointType.OtherObjectPlace });
        }
        // end of Xanders edit



        //bool canPlaceInOwnGarden = (GameManager.Instance.ActivePlayer.TurnPoints.HasPointsLeft(TurnPoints.PointType.SelfObjectPlace) && GameManager.Instance.InOwnGarden);
        //bool canPlaceInCompanionsGarden = (!GameManager.Instance.InOwnGarden && GameManager.Instance.ActivePlayer.TurnPoints.HasPointsLeft(TurnPoints.PointType.SelfObjectPlace));


        //if (canPlaceInOwnGarden || canPlaceInCompanionsGarden)
        //{
        //    EventsManager.InvokeEvent(EventsManager.EventType.SeedBagOpen);
        //}
        //else if(GameManager.Instance.InOwnGarden)
        //{
        //    EventsManager.InvokeEvent(EventsManager.ParameterEventType.NotEnoughPointsForAction, new EventsManager.EventParams() { EnumData = TurnPoints.PointType.SelfObjectPlace });
        //}
        //else if(!GameManager.Instance.InOwnGarden)
        //{
        //    EventsManager.InvokeEvent(EventsManager.ParameterEventType.NotEnoughPointsForAction, new EventsManager.EventParams() { EnumData = TurnPoints.PointType.OtherObjectPlace });
        //}


    }
    public void CloseSeedBag() { EventsManager.InvokeEvent(EventsManager.EventType.SeedBagClose); Debug.Log("Closed Bag"); }
    public void OkayNext() => EventsManager.InvokeEvent(EventsManager.EventType.DialogueNext);
    public void DialogueBack() => EventsManager.InvokeEvent(EventsManager.EventType.DialoguePrevious);
}
