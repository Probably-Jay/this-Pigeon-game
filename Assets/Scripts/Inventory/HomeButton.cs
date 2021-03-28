using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneUI
{
    public class HomeButton : MonoBehaviour
    {
        [SerializeField] GameObject HomeMenu;
        public void OpenQuitMenuButton() => HomeMenu.SetActive(true);
        public void Cancel() => HomeMenu.SetActive(false);
        public void QuitToMenu() => SceneChangeController.Instance.ChangeScene(SceneChangeController.Scenes.MainMenu);

    }
}