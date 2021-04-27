using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SaveSystem;

namespace NetSystem
{
    /// <summary>
    /// Takes the gatherd data and sends it to the server
    /// </summary>
    public class GameDataSaver : MonoBehaviour
    {

        GardenDataPacket gardenData;
        PlayerDataPacket playerData;



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
            NetworkGame.RawData rawData = new NetworkGame.RawData()
            {
                turnBelongsTo = playerData.turnOwner,
                turnComplete = JsonUtility.ToJson(playerData.turnComplete), // might work
                gardenData = JsonUtility.ToJson(gardenData),
                playerData = JsonUtility.ToJson(playerData)
            };

          // SaveSystemInternal.SaveDataUtility.GetSerialisableSaveGameStruct

            var callbacks = new APIOperationCallbacks(SendSaveDataSucess, SendSaveDataFailure);

            NetworkHandler.Instance.SendData(callbacks, rawData);
        }

        private void SendSaveDataSucess()
        {
            throw new NotImplementedException();
        }

        private void SendSaveDataFailure(FailureReason reason)
        {
            throw new NotImplementedException();
        }
    }
}