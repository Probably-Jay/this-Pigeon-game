using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// jay 18/02

[RequireComponent(typeof(Animator))]
public class CameraMovementControl : MonoBehaviour
{

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        EventsManager.BindEvent(EventsManager.EventType.EndTurn, SwapPlayers);
    }

    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.EndTurn, SwapPlayers);
    }

    void DoNotingFirstTime()
    {
        EventsManager.BindEvent(EventsManager.EventType.NewTurnBegin, SwapPlayers);
        EventsManager.UnbindEvent(EventsManager.EventType.NewTurnBegin, DoNotingFirstTime);
    }

    void SwapPlayers()
    {
        switch (GameManager.Instance.ActivePlayer.PlayerEnumValue)
        {
            case Player.PlayerEnum.Player0:
                Debug.Log("1to2");
                animator.SetTrigger("SwapToPlayerTwo");
                break;
            case Player.PlayerEnum.Player1:
                Debug.Log("TwoToOne");
                animator.SetTrigger("SwapToPlayerOne");
                break;
        }
    }

}
