using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Localisation;
using TMPro;

namespace SceneInterface
{

    public class SettingsController : MonoBehaviour
    {
        [SerializeField] Text text;
        [SerializeField] TMP_Dropdown dropdown;

        private void Start()
        {
            text.text = Localiser.GetText(TextID.Settings_ResetTutorials);
        }

        public void ResetTutorials()
        {
            PlayerPrefs.DeleteAll();
            text.text = Localiser.GetText(TextID.Settings_Done);
        }

        public void ToMainMenu()
        {
            SceneChangeController.Instance.ChangeScene(SceneChangeController.Scenes.MainMenu);
        }

        public void ChangeLanguage()
        {
            var language = dropdown.value;
            Localiser.SetLanguage((Language)language);
            SceneChangeController.Instance.ChangeScene(SceneChangeController.Scenes.Settings);
        }

    }
}