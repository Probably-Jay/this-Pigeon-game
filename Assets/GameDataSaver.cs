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
            GardenDataPacket gardenData = GameCore.GameManager.Instance.DataManager.GardenData;
            PlayerDataPacket playerData = GameCore.GameManager.Instance.DataManager.PlayerData;

            NetworkGame.RawData rawData = new NetworkGame.RawData()
            {
                //turnBelongsTo = JsonUtility.ToJson(playerData.turnOwner),
                turnComplete = playerData.turnComplete  ? "true" : "false", // might work
                gardenData = JsonUtility.ToJson(gardenData),
                playerData = JsonUtility.ToJson(playerData)
            };

          // SaveSystemInternal.SaveDataUtility.GetSerialisableSaveGameStruct

            var callbacks = new APIOperationCallbacks(SendSaveDataSucess, SendSaveDataFailure);

            NetworkHandler.Instance.SendData(callbacks, rawData);
        }

        private void SendSaveDataSucess()
        {
            Debug.Log("Sent sucess");
        }

        private void SendSaveDataFailure(FailureReason reason)
        {
            Debug.LogError($"Failed {reason}");
        }
    }
}