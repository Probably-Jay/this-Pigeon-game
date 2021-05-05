using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SceneInterface
{

    public class SettingsController : MonoBehaviour
    {
        [SerializeField] Text text;
        public void ResetTutorials()
        {
            PlayerPrefs.DeleteAll();
            text.text = "Done";
        }

        public void ToMainMenu()
        {
            SceneChangeController.Instance.ChangeScene(SceneChangeController.Scenes.MainMenu);
        }
    }
}