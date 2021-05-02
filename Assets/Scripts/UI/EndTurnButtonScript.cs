﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameCore;

namespace SceneUI
{
    [RequireComponent(typeof(Button))]
    public class EndTurnButtonScript : MonoBehaviour
    {

        [SerializeField] GardenSlotDirectory gardenSlotDirectory;
        

      


        public void CallEndTurn() {
            gardenSlotDirectory.HideAllSlotsAndHideIndicators();

            GameManager.Instance.EndTurn();

          //  throw new NotImplementedException("Above line was removed in hotseat removal and has not been re-implimented");

        }

        Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
        }

        private void Start()
        {
            if (GameCore.GameManager.Instance.Spectating)
            {
                HideButton();
            }
        }


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

        private void EnableButton()
        {
            button.interactable = true;
        }

        private void DisableButton()
        {
            button.interactable = false;
        }
    }
}