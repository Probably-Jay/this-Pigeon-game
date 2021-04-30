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
// Edited Alexander Purvis 30/04/2021

namespace Plants {
    /// <summary>
    /// Script all plants will have
    /// </summary>
    [RequireComponent(typeof(PlantGrowth))]
    [RequireComponent(typeof(PlantIconsDisplay))]
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


        public enum PlantSize // ToDo Later
        {
            Wide,
            Tall,
            Single
        }


        private void OnEnable()
        {
            PlantGrowth.ReachedMaturity += UpdateStats;
        }
        private void OnDisable()
        {
            PlantGrowth.ReachedMaturity -= UpdateStats;
        }
        private void UpdateStats() => EventsManager.InvokeEvent(EventsManager.EventType.PlantChangedStats);

        // private PlantSize plantSize1;

        // public string objectName;
        public PlantName plantname;

        public int InternalGrowthStage => PlantGrowth.TendingState.CurrentGrowthStage;


        public void SetState(GardenDataPacket.Plant plantData)
        {
            PlantGrowth.SetState(plantData);
        }


        public int StoredInGarden { get; private set; }
        public int StoredInSlot { get; private set; }

        public void Init(int garden, int slot)
        {
            StoredInGarden = garden;
            StoredInSlot = slot;
        }

        public bool RequiresAction(TendingActions tendingActions) => PlantGrowth.TendingState.GetRequiredActions().Contains(tendingActions);

        [SerializeField] PlantSize thisPlantsSize;

        [SerializeField] public int requiredSlot = 1;
        [SerializeField] public int currentSlotPlantedIn;

        [Header("Plant Stats")]
        [SerializeField, Range(0, 1)] private int social = 0;
        [SerializeField, Range(0, 1)] private int joyful = 0;
        [SerializeField, Range(0, 1)] private int energetic = 0;
        [SerializeField, Range(0, 1)] private int painful = 0;

        private int Social { get => social * PlantGrowth.GrowthLevelMoodMultiplier; set => social = value; }
        private int Joyful { get => joyful * PlantGrowth.GrowthLevelMoodMultiplier; set => joyful = value; }
        private int Energetic { get => energetic * PlantGrowth.GrowthLevelMoodMultiplier; set => energetic = value; }
        private int Painful { get => painful * PlantGrowth.GrowthLevelMoodMultiplier; set => painful = value; }


        public TraitValue Traits => new TraitValue(Social, Joyful, Energetic, Painful);
        public TraitValue TraitsUnscaled => new TraitValue(social, joyful, energetic, painful);

       // public Player PlantOwner { get; set; }




        // public bool isPlanted = false;
       /// private Player.PlayerEnum? gardenID = null;

      //  public Player.PlayerEnum? GardenPlayerID { get => gardenID; set => gardenID = value; }


        private void Awake() // hack, todo fix this
        {

            PlantGrowth = GetComponent<PlantGrowth>();


            // Get current player

            //PlantOwner = GameCore.GameManager.Instance.GetPlayer(GameCore.GameManager.Instance.PlayerWhosGardenIsCurrentlyVisible); // Load system will break here
           // throw new NotImplementedException("Above line was removed in hotseat removal and has not been re-implimented");

            //// Set if in local or other garden
            //if (plantOwner.PlayerEnumValue == 0)
            //{ // = true if local (placed by player 1)
            //    InLocalGarden = true;
            //}
            //else
            //{ // = false if not
            //    InLocalGarden = false;
            //}
        }






        public PlantGrowth PlantGrowth { get; private set; }
        public PlantSize ThisPlantsSize => thisPlantsSize;



        //public Player PlantOwner { get => PlantOwner1; set => PlantOwner1 = value; }



        //  public bool InLocalGarden { get; set; }

        /// <summary>
        /// Tend the plant. No effect if this plant does not need this action taken
        /// </summary>
        public void Tend(TendingActions action)
        {
            bool tended = PlantGrowth.Tend(action);

            if (!tended)
            {
                return;
            }

            GameCore.GameManager.Instance.DataManager.UpdatePlantTendingActions(this);
        }
    }

}