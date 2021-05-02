using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace SceneUI
{
    [RequireComponent(typeof(Animator))]
    public class StateTextController : MonoBehaviour
    {
        [SerializeField] Animator animator;
        [SerializeField] TMP_Text bigText;
        [SerializeField] TMP_Text subText;


        const string spectatingBig = "Spectating";
        const string spectatingSmall = "It is your companion's turn";
        const string playingBig = "Playing";
        const string playingSmall = "It is your turn";

        private void OnEnable()
        {
            EventsManager.BindEvent(EventsManager.EventType.EnterPlayingState, Playing);
            EventsManager.BindEvent(EventsManager.EventType.EnterSpectatingState, Spectating);
        }
        private void OnDisable()
        {
            EventsManager.UnbindEvent(EventsManager.EventType.EnterPlayingState, Playing);
            EventsManager.UnbindEvent(EventsManager.EventType.EnterSpectatingState, Spectating);
        }

        private void Spectating()
        {
            Appear(spectatingBig, spectatingSmall);
        }


        private void Playing()
        {
            Appear(playingBig, playingSmall);
        }

        private void Appear(string big, string sub)
        {
            bigText.text = big;
            subText.text = sub;
            animator.SetTrigger("Appear");
        }

    }
}