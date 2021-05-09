using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SceneUI
{

    public class LiveUpdateGamesSettingsButton : MonoBehaviour
    {
        private static bool DoLiveUpdate => GameCore.GameManager.DoLiveUpdate;
        private const string liveUpdatePrefsKey = GameCore.GameManager.LiveUpdatePrefsKey;
        [SerializeField] Text buttonText;
        [SerializeField] Text warningText;

        private void Start()
        {
            if (!PlayerPrefs.HasKey(liveUpdatePrefsKey))
            {
                TurnOff();
                return;
            }
            UpdateText();
        }

        public void Swap()
        {
            if (DoLiveUpdate)
            {
                TurnOff();
            }
            else
            {
                TurnOn();
            }
        }

        private void UpdateText()
        {
            if (DoLiveUpdate)
            {
                TurnOn();
            }
            else
            {
                TurnOff();
            }
        }


        private void TurnOff()
        {
            PlayerPrefs.SetInt(liveUpdatePrefsKey, 0);
            buttonText.text = Localisation.Localiser.GetText(Localisation.TextID.Settings_LiveUpdateOff);
            warningText.gameObject.SetActive(false);
        }

        private void TurnOn()
        {
            PlayerPrefs.SetInt(liveUpdatePrefsKey, 1);
            buttonText.text = Localisation.Localiser.GetText(Localisation.TextID.Settings_LiveUpdateOn);
            warningText.gameObject.SetActive(true);

        }
    }
}
