using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// jay 02/05

namespace SceneUI
{
    public class GoalSliderPositionerOnline : MonoBehaviour
    {

        [SerializeField] RectTransform garden1Transform;
        [SerializeField] RectTransform garden2Transform;
        [SerializeField] GameObject pannel;

        private void OnEnable()
        {
            EventsManager.BindEvent(EventsManager.EventType.EnterPlayingState, PositionSliderAboveMe);
            EventsManager.BindEvent(EventsManager.EventType.EnterSpectatingState, DisableSlider);
        }

        private void OnDisable()
        {
            EventsManager.UnbindEvent(EventsManager.EventType.EnterPlayingState, PositionSliderAboveMe);
            EventsManager.UnbindEvent(EventsManager.EventType.EnterSpectatingState, DisableSlider);
        }


        private void DisableSlider()
        {
            pannel.SetActive(false);
        }
        private void EnableSlider()
        {
            pannel.SetActive(true);
        }

        private void PositionSliderAboveMe()
        {
            EnableSlider();
            switch (GameCore.GameManager.Instance.LocalPlayerEnumID)
            {
                case Player.PlayerEnum.Player1:
                    PositionSliderUnder(garden1Transform);
                    break;
                case Player.PlayerEnum.Player2:
                    PositionSliderUnder(garden2Transform);
                    break;
            }
        }

        private void PositionSliderUnder(RectTransform parentTransform)
        {
            transform.SetParent(parentTransform);
            transform.localPosition = Vector3.zero;
        }
    }
}