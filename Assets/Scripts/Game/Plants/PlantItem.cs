using System.Collections;
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
    public class PlantItem : MonoBehaviour
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

        internal void Tend(Tending.TendingActions tendingActions)
        {
            throw new NotImplementedException();
        }

        public enum PlantSize // ToDo Later
        {
            Wide,
            Tall,
            Single
        }

        public enum PlantGrowthStage {
            Seed,
            Sprout,
            Bloom
        }


        [SerializeField] List<Tending.TendingActions> legalTendingActions;
        public List<Tending.TendingActions> LegalTendingActions { get => legalTendingActions; set => legalTendingActions = value; }




        // public string objectName;
        public PlantName plantname;

        public PlantGrowthStage plantGrowthState = PlantGrowthStage.Seed;
        public int moodMult = 0;

        [SerializeField, Range(-1, 1)] private int social = 0;
        [SerializeField, Range(-1, 1)] private int joyful = 0;
        [SerializeField, Range(-1, 1)] private int energetic = 0;
        [SerializeField, Range(-1, 1)] private int painful = 0;

        private int Social { get => social * moodMult; set => social = value; }
        private int Joyful { get => joyful * moodMult; set => joyful = value; }
        private int Energetic { get => energetic * moodMult; set => energetic = value; }
        private int Painful { get => painful * moodMult; set => painful = value; }


        [SerializeField] public int growthGoal = 1;
        public int currGrowth = 0;

        public Player plantOwner;
        public bool inLocalGarden;

        //Get the Renderer component for changing colours (temp, replace with actual different sprites later)
        Renderer matRenderer;

        // public bool isPlanted = false;
        public Player.PlayerEnum gardenID = Player.PlayerEnum.Unassigned;


        private void OnEnable() // hack, todo fix this
        {
            matRenderer = GetComponent<Renderer>();

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

        public void UpdateSprite() {

            switch (plantGrowthState) {
                case PlantGrowthStage.Seed:
                    matRenderer.material.SetColor("_Color", Color.red);
                    break;

                case PlantGrowthStage.Sprout:
                    matRenderer.material.SetColor("_Color", Color.yellow);
                    break;

                case PlantGrowthStage.Bloom:
                    matRenderer.material.SetColor("_Color", Color.green);
                    break;
            }

        }

        public TraitValue PlantStats => new TraitValue(Social, Joyful, Energetic, Painful);

    }

}