using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button),typeof(Animator))]
public class SwapGardenButton : MonoBehaviour
{

    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        EventsManager.BindEvent(EventsManager.ParameterEventType.SwappedGardenVeiw, RotateButton);
    }

    private void RotateButton(EventsManager.EventParams paramaters)
    {
        switch ((Player.PlayerEnum)paramaters.EnumData)
        {
            case Player.PlayerEnum.Player0:
                animator.SetTrigger("SwitchToPlayerOne");
                break;
            case Player.PlayerEnum.Player1:
                animator.SetTrigger("SwitchToPlayerTwo");
                break;
            default:
                break;
        }
    }
}
