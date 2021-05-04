using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneUI
{

    public class SeedBagButtonParentScript : MonoBehaviour
    {
        [SerializeField] GameObject seedBag;
        private void OnEnable()
        {
            EventsManager.BindEvent(EventsManager.EventType.StartNewGame, UnHideBag);
            EventsManager.BindEvent(EventsManager.EventType.EnterPlayingState, UnHideBag);
            EventsManager.BindEvent(EventsManager.EventType.EnterSpectatingState, HideBag);
        }

        private void OnDisable()
        {
            EventsManager.UnbindEvent(EventsManager.EventType.StartNewGame, UnHideBag);
            EventsManager.UnbindEvent(EventsManager.EventType.EnterPlayingState, UnHideBag);
            EventsManager.UnbindEvent(EventsManager.EventType.EnterSpectatingState, HideBag);
        }

        private void UnHideBag()
        {
            seedBag.SetActive(true);
        }

        private void HideBag()
        {
            seedBag.SetActive(false);
        }

    }
}