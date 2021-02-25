using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// jay 18/02

[RequireComponent(typeof(Animator))]
public class CameraMovementControl : MonoBehaviour
{

    Animator animator;
    Player.PlayerEnum currentGardenVeiw;

    public Player.PlayerEnum OtherGardenVeiw { get { return currentGardenVeiw == Player.PlayerEnum.Player0 ? Player.PlayerEnum.Player1 : Player.PlayerEnum.Player0; } }


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        EventsManager.BindEvent(EventsManager.EventType.EndTurn, SwitchToActivePlayer);
    }

    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.EndTurn, SwitchToActivePlayer);
    }


    private void Start()
    {
        currentGardenVeiw = GameManager.Instance.ActivePlayer.PlayerEnumValue;
    }
    void DoNotingFirstTime()
    {
        EventsManager.BindEvent(EventsManager.EventType.NewTurnBegin, SwitchToActivePlayer);
        EventsManager.UnbindEvent(EventsManager.EventType.NewTurnBegin, DoNotingFirstTime);
    }

    void SwitchToActivePlayer()
    {

        //  if(currentGardenVeiw == Player.PlayerEnum.Player0 &&)


        Player.PlayerEnum player = GameManager.Instance.ActivePlayer.PlayerEnumValue == Player.PlayerEnum.Player0? Player.PlayerEnum.Player1 : Player.PlayerEnum.Player0; ;
        SwapVeiwTo(player);
    }

    public void SwapPlayers() => SwapVeiwTo(OtherGardenVeiw);

    private void SwapVeiwTo(Player.PlayerEnum player)
    {
        if (player == currentGardenVeiw) return;

        switch (player)
        {
            case Player.PlayerEnum.Player0:
                animator.SetTrigger("SwapToPlayerOne");
                currentGardenVeiw = Player.PlayerEnum.Player0;
                EventsManager.InvokeEvent(EventsManager.ParameterEventType.SwappedGardenVeiw, new EventsManager.EventParams() { EnumData = currentGardenVeiw });
                break;
            case Player.PlayerEnum.Player1:
                animator.SetTrigger("SwapToPlayerTwo");
                currentGardenVeiw = Player.PlayerEnum.Player1;
                EventsManager.InvokeEvent(EventsManager.ParameterEventType.SwappedGardenVeiw, new EventsManager.EventParams() { EnumData = currentGardenVeiw });
                break;
        }
    }

  

}
