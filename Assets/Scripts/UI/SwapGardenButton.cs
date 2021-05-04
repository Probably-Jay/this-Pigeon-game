using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace SceneUI
{
    [RequireComponent(typeof(Button), typeof(Animator))]
    public class SwapGardenButton : MonoBehaviour
    {

        [SerializeField]GardenSlotDirectory gardenSlotDirectory;

        Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();

        }


        private void OnEnable()
        {
            EventsManager.BindEvent(EventsManager.ParameterEventType.SwappedGardenView, RotateButton);
        }

        private void OnDisable()
        {
            EventsManager.UnbindEvent(EventsManager.ParameterEventType.SwappedGardenView, RotateButton);
        }

        private void RotateButton(EventsManager.EventParams paramaters)
        {
            switch ((Player.PlayerEnum)paramaters.EnumData1)
            {
                case Player.PlayerEnum.Player1:
                    gardenSlotDirectory.HideAllSlotsAndHideIndicators();
                    animator.SetTrigger("SwitchToPlayerOne");
                    break;
                case Player.PlayerEnum.Player2:
                    gardenSlotDirectory.HideAllSlotsAndHideIndicators();
                    animator.SetTrigger("SwitchToPlayerTwo");
                    break;
                default:
                    break;
            }
        }
    }
}