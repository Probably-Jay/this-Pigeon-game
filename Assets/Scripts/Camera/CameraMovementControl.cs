using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// jay 18/02

[RequireComponent(typeof(Animator))]
public class CameraMovementControl : MonoBehaviour
{

    Animator animator;
    public Player.PlayerEnum CurrentGardenView { get; private set; }

    public Player.PlayerEnum OtherGardenVeiw { get { return CurrentGardenView == Player.PlayerEnum.Player1 ? Player.PlayerEnum.Player2 : Player.PlayerEnum.Player1; } }


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    //private void OnEnable()
    //{
    //    EventsManager.BindEvent(EventsManager.EventType.EndTurn, SwitchToActivePlayer);
    //}

    //private void OnDisable()
    //{
    //    EventsManager.UnbindEvent(EventsManager.EventType.EndTurn, SwitchToActivePlayer);
    //}


    private void Start()
    {
        switch (GameCore.GameManager.Instance.LocalPlayerEnumID)
        {
            case Player.PlayerEnum.Player1:
                CurrentGardenView = Player.PlayerEnum.Player1;
                SwapVeiwTo(CurrentGardenView);
                break;
            case Player.PlayerEnum.Player2:
                CurrentGardenView = Player.PlayerEnum.Player2;
                SwapVeiwTo(CurrentGardenView);
                break;;
        }
    }

    //void DoNotingFirstTime()
    //{
    //    EventsManager.BindEvent(EventsManager.EventType.NewTurnBegin, SwitchToActivePlayer);
    //    EventsManager.UnbindEvent(EventsManager.EventType.NewTurnBegin, DoNotingFirstTime);
    //}

    //void SwitchToActivePlayer()
    //{

    //    //  if(currentGardenVeiw == Player.PlayerEnum.Player0 &&)


    //    //Player.PlayerEnum player = GameCore.GameManager.Instance.ActivePlayerID == Player.PlayerEnum.Player1? Player.PlayerEnum.Player2 : Player.PlayerEnum.Player1; ;
    //    SwapVeiwTo(player);
    //}

    public void SwapPlayers() => SwapVeiwTo(OtherGardenVeiw);

    private void SwapVeiwTo(Player.PlayerEnum player)
    {
        //if (player == CurrentGardenView) return;

        switch (player)
        {
            case Player.PlayerEnum.Player1:
                animator.SetBool("VeiwPlayerOne",true);
                CurrentGardenView = Player.PlayerEnum.Player1;
                EventsManager.InvokeEvent(EventsManager.ParameterEventType.SwappedGardenView, new EventsManager.EventParams() { EnumData1 = CurrentGardenView });
                break;
            case Player.PlayerEnum.Player2:
                animator.SetBool("VeiwPlayerOne",false);
                CurrentGardenView = Player.PlayerEnum.Player2;
                EventsManager.InvokeEvent(EventsManager.ParameterEventType.SwappedGardenView, new EventsManager.EventParams() { EnumData1 = CurrentGardenView });
                break;
        }
    }

  

}
