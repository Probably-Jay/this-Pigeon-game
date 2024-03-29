﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Script created by Alexander Purvis 26/04/2021
namespace NetSystem
{

    [System.Serializable]
    public class GardenDataPacket
    {
        [System.Serializable]
        public class Plant
        {
            public int plantType;
            public int slotNumber;
            public int stage;

            public bool watering;
            public bool spraying;
            public bool trimming;
            //  public bool action4;

            public Plant()
            {
                plantType = -1;
            }

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

            public bool Initilised => plantType != -1;
            public void Uninitialise() => plantType = -1;
        }

        [SerializeField] public Plant[] newestGarden1 = new Plant[10] { new Plant(), new Plant(), new Plant(), new Plant(), new Plant(), new Plant(), new Plant(), new Plant(), new Plant(), new Plant() };
        [SerializeField] public Plant[] newestGarden2 = new Plant[10] { new Plant(), new Plant(), new Plant(), new Plant(), new Plant(), new Plant(), new Plant(), new Plant(), new Plant(), new Plant() };

        //public List<Plant> oldGarden1;
        // public List<Plant> oldGarden2;
    }
}