using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameCore;

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
    public GameObject myBox;

    

   

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
    public void CloseSeedBag() { 
        
        EventsManager.InvokeEvent(EventsManager.EventType.SeedBagClose); 
        Debug.Log("Closed Bag"); 
    }
    public void Skip() => EventsManager.InvokeEvent(EventsManager.EventType.DialogueSkip);
    public void OkayNext() => EventsManager.InvokeEvent(EventsManager.EventType.DialogueNext);
    public void DialogueBack() => EventsManager.InvokeEvent(EventsManager.EventType.DialoguePrevious);
    public void ConfirmTrowel() => EventsManager.InvokeEvent(EventsManager.EventType.RemovedOwnObject);
    public void CloseBox() => myBox.SetActive(false);
}
