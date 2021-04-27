using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    PlayerDataPacket playerData;
    GardenDataPacket gardenData;


    void Awake()
    {
        playerData = GetComponent<PlayerDataPacket>();
        gardenData = GetComponent<GardenDataPacket>();
    }


   public void SetPlayer1Name(string newID)
   {
        playerData.player1ID = newID;
   }

   public void SetPlayer2Name(string newID)
   {
       playerData.player1ID = newID;
   }

   public void AddPlantToGarden1(
    int plantType,
    int slotNumber,
    int stage,
    bool action1,
    bool action2,
    bool action3,
    bool action4)
   {
        gardenData.newestGarden1.Add(new GardenDataPacket.Plant(plantType, slotNumber, stage, action1, action2, action3, action4));
   }

   public void AddPlantToGarden2()
   {

   }

}
