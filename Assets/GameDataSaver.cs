using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem {

    public class GameDataSaver : MonoBehaviour
    {
        private void OnEnable()
        {
            EventsManager.BindEvent(EventsManager.EventType.SaveGatheredData, SaveGame);
        }


        private void OnDisable()
        {
            EventsManager.BindEvent(EventsManager.EventType.SaveGatheredData, SaveGame);
        }
        private void SaveGame()
        {
            throw new NotImplementedException();
        }
    }
}