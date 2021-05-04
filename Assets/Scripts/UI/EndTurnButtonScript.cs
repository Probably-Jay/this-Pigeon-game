using System;
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