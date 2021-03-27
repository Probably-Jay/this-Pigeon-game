﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mood;
using Plants.PlantActions;
using System.Collections.ObjectModel;
using System;


// Created Jay 05/02
// Edited Scott 24/02
// Edited Alexander Purvis 04/03
// Added plant enum Jay 13/03
// Added stages of growth Scott 24/03

namespace Plants {
    /// <summary>
    /// Script all plants will have
    /// </summary>
    [RequireComponent(typeof(PlantGrowth))]
    public class Plant : MonoBehaviour
    {
        public enum PlantName
        {
            Rine
            , Vlum
            , Adrinque
            , Zove
            , Vlufraisy
            , Eisower
            , Phess
            , Brovlary
            , Aesron
            , Phodetta
        }



        public void Tend(TendingActions tendingActions)
        {
            throw new NotImplementedException();
        }

        public enum PlantSize // ToDo Later
        {
            Wide,
            Tall,
            Single
        }

  

        // public string objectName;
        public PlantName plantname;

  

        [SerializeField, Range(-1, 1)] private int social = 0;
        [SerializeField, Range(-1, 1)] private int joyful = 0;
        [SerializeField, Range(-1, 1)] private int energetic = 0;
        [SerializeField, Range(-1, 1)] private int painful = 0;

        private int Social { get => social * PlantGrowth.GrowthLevelMoodMultiplier; set => social = value; }
        private int Joyful { get => joyful * PlantGrowth.GrowthLevelMoodMultiplier; set => joyful = value; }
        private int Energetic { get => energetic * PlantGrowth.GrowthLevelMoodMultiplier; set => energetic = value; }
        private int Painful { get => painful * PlantGrowth.GrowthLevelMoodMultiplier; set => painful = value; }


      

        public Player plantOwner;
        public bool inLocalGarden;

      

        // public bool isPlanted = false;
        public Player.PlayerEnum gardenID = Player.PlayerEnum.Unassigned;


        private void Awake() // hack, todo fix this
        {

            PlantGrowth = GetComponent<PlantGrowth>();


            // Get current player
            plantOwner = GameManager.Instance.ActivePlayer; // Load system will break here

            // Set if in local or other garden
            if (plantOwner.PlayerEnumValue == 0)
            { // = true if local (placed by player 1)
                inLocalGarden = true;
            }
            else
            { // = false if not
                inLocalGarden = false;
            }
        }

       

        

        public TraitValue PlantStats => new TraitValue(Social, Joyful, Energetic, Painful);

        public PlantGrowth PlantGrowth { get; private set; }
    }

}