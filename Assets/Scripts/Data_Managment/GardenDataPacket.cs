using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenDataPacket : MonoBehaviour
{
    public struct Plant
    {
        public int plantType;
        public int slotNumber;
        public int stage;

        public bool action1;
        public bool action2;
        public bool action3;
        public bool action4;

        public Plant(int PlantType,
                     int SlotNumber,
                     int Stage,
                     bool Action1,
                     bool Action2,
                     bool Action3,
                     bool Action4)
        {

            plantType = PlantType;
            slotNumber = SlotNumber;
            stage = Stage;

            action1 = Action1;
            action2 = Action2;
            action3 = Action3;
            action4 = Action4;
        }
      
    }

   public List<Plant> newestGarden1;
   public List<Plant> newestGarden2;

   public List<Plant> oldGarden1;
   public List<Plant> oldGarden2;
}
