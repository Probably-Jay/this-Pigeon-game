using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenDataPacket : MonoBehaviour
{




    public class Plant
    {
        public int plantType;
        public int slotNumber;
        public int stage;

        public bool watering;
        public bool spraying;
        public bool trimming;
      //  public bool action4;

        public Plant(int PlantType,
                     int SlotNumber,
                     int Stage,
                     bool watering,
                     bool spraying,
                     bool trimming
                    )
        {
            plantType = PlantType;
            slotNumber = SlotNumber;
            stage = Stage;

            this.watering = watering;
            this.spraying = spraying;
            this.trimming = trimming;
        
        }
    }

   public Plant[] newestGarden1 = new Plant[10];
   public Plant[] newestGarden2 = new Plant[10];

   //public List<Plant> oldGarden1;
  // public List<Plant> oldGarden2;
}
