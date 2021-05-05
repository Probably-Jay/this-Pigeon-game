using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneInterface
{

    public class MenuController : MonoBehaviour
    {


        public void ConnectToGameServers()
        {
            SceneChangeController.Instance.ChangeScene(SceneChangeController.Scenes.ConnectingScene);
        }

        public void ConnectToGameServersAsDebugAccount()
        {
            NetSystem.NetworkHandler.Instance._useDebugAcountLogin = true;
            ConnectToGameServers();
        }

        //public void GoToMyGarden()
        //{
        //    // Change to metagarden after metagarden added
        //    SceneChangeController.Instance.ChangeScene(SceneChangeController.Scenes.MoodSelectScreen1);
        //}

        //public void GoToLastGarden()
        //{
        //    // Change to last garden once multiple are available
        //    SceneChangeController.Instance.ChangeScene(SceneChangeController.Scenes.MoodSelectScreen1);
        //}

      

        public void GotToSettings()
        {
            SceneChangeController.Instance.ChangeScene(SceneChangeController.Scenes.Settings);
        }

     

        public void QuitGame()
        {
            EventsManager.InvokeEvent(EventsManager.EventType.QuitGame);
        }
    }
}