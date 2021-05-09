using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Localisation;

namespace SceneUI
{
    [RequireComponent(typeof(Animator))]
    public class StateTextController : MonoBehaviour
    {
        [SerializeField] Animator animator;
        [SerializeField] TMP_Text bigText;
        [SerializeField] TMP_Text subText;


        string spectatingBig = Localiser.GetText(TextID.GameScene_SpectatingText_1); //"Spectating";
        string spectatingSmall = Localiser.GetText(TextID.GameScene_SpectatingText_2); // "It is your companion's turn";

        string playingBig = Localiser.GetText(TextID.GameScene_PlayingText_1); //"Playing";
        string playingSmall = Localiser.GetText(TextID.GameScene_PlayingText_2); // "It is your turn";

       
        
        

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