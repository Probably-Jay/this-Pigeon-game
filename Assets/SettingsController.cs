using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Localisation;

namespace SceneInterface
{

    public class SettingsController : MonoBehaviour
    {
        [SerializeField] Text text;

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
    }
}