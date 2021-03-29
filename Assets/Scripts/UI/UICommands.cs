using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//created Zap 26/03
public class UICommands : MonoBehaviour
{
    private void OnEnable()
    {
    }
    private void OnDisable()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OpenToolBox() => EventsManager.InvokeEvent(EventsManager.EventType.ToolBoxOpen);
    public void CloseToolBox() => EventsManager.InvokeEvent(EventsManager.EventType.ToolBoxClose);
    public void OpenSeedBag()
    {
        if ((GameManager.Instance.ActivePlayer.TurnPoints.HasPointsLeft(TurnPoints.PointType.SelfObjectPlace)&&GameManager.Instance.InOwnGarden) || (!GameManager.Instance.InOwnGarden&&GameManager.Instance.ActivePlayer.TurnPoints.HasPointsLeft(TurnPoints.PointType.SelfObjectPlace)))
            {
            EventsManager.InvokeEvent(EventsManager.EventType.SeedBagOpen);
        }
        else if(GameManager.Instance.InOwnGarden)
        {
            EventsManager.InvokeEvent(EventsManager.ParameterEventType.NotEnoughPointsForAction, new EventsManager.EventParams() { EnumData = TurnPoints.PointType.SelfObjectPlace });
        }
        else if(!GameManager.Instance.InOwnGarden)
        {
            EventsManager.InvokeEvent(EventsManager.ParameterEventType.NotEnoughPointsForAction, new EventsManager.EventParams() { EnumData = TurnPoints.PointType.OtherObjectPlace });
        }
    }

    public void OkayNext() => EventsManager.InvokeEvent(EventsManager.EventType.DialogueNext);
    public void DialogueBack() => EventsManager.InvokeEvent(EventsManager.EventType.DialoguePrevious);

}
