using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneUI
{
    public class EndTurnButtonParent : MonoBehaviour
    {
        [SerializeField] GameObject button;

        private void OnEnable()
        {
            EventsManager.BindEvent(EventsManager.EventType.CompleteTurn, HideButton);
            EventsManager.BindEvent(EventsManager.EventType.EnterPlayingState, UnHideButton);
            EventsManager.BindEvent(EventsManager.EventType.EnterSpectatingState, HideButton);
        }



        private void OnDisable()
        {
            EventsManager.UnbindEvent(EventsManager.EventType.CompleteTurn, HideButton);
            EventsManager.UnbindEvent(EventsManager.EventType.EnterPlayingState, UnHideButton);
            EventsManager.UnbindEvent(EventsManager.EventType.EnterSpectatingState, HideButton);
        }

        private void HideButton()
        {
            button.gameObject.SetActive(false);
        }

        private void UnHideButton()
        {
            button.gameObject.SetActive(true);
        }

    }
}
