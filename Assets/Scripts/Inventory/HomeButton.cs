using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneUI
{
    public class HomeButton : MonoBehaviour
    {
        [SerializeField] GameObject HomeMenu;
        public void Cancel() => HomeMenu.SetActive(false);

        public void OpenQuitMenuButton()
        {
            HomeMenu.SetActive(true);
            EventsManager.InvokeEvent(EventsManager.EventType.OnMenuOpened);
        }

        public void QuitToMenu()
        {
            GameCore.GameManager.Instance.QuitToMenu();
        }
    }
}