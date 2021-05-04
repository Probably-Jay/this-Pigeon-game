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
// Added plantIcon related things, Zap, 04/05/2021

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

        public PlantIcon[] icons = new PlantIcon[2];



        private void OnEnable()
        {
            PlantGrowth.ReachedMaturity += UpdateStats;
            PlantGrowth.TendingState.OnPlantGrowth += SavePlantState;
            EventsManager.BindEvent(EventsManager.EventType.TurnClaimed, GrowIfShould);
        }


        private void OnDisable()
        {
            PlantGrowth.ReachedMaturity -= UpdateStats;
            PlantGrowth.TendingState.OnPlantGrowth -= SavePlantState;

            EventsManager.UnbindEvent(EventsManager.EventType.TurnClaimed, GrowIfShould);
        }

        private void GrowIfShould()
        {
            if(StoredInGarden != GameCore.GameManager.Instance.LocalPlayerEnumID)
            {
                return; // only grow on your turn
            }

            PlantGrowth.GrowIfShould();
        }

        private void UpdateStats() => EventsManager.InvokeEvent(EventsManager.EventType.PlantChangedStats);

        // private PlantSize plantSize1;

        // public string objectName;
        public PlantName plantname;

        public int InternalGrowthStage => PlantGrowth.TendingState.CurrentGrowthStage;


        public void SetState(NetSystem.GardenDataPacket.Plant plantData)
        {
            PlantGrowth.SetState(plantData);
        }
        private void SavePlantState()
        {
            PlantGrowth.SaveState(StoredInGarden, StoredInSlot);
        }


        public Player.PlayerEnum StoredInGarden { get; private set; }
        public int StoredInSlot { get; private set; }

        public void Init(Player.PlayerEnum garden, int slot)
        {
            StoredInGarden = garden;
            StoredInSlot = slot;
        }

        public bool RequiresAction(TendingActions tendingActions) => PlantGrowth.TendingState.GetRequiredActions().Contains(tendingActions);

        [SerializeField] PlantSize thisPlantsSize;

        [SerializeField] public int requiredSlot = 1;
  

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

        private void Start()
        {
            Debug.Log(thisPlantsSize);
            switch (thisPlantsSize)
            {
                case PlantSize.Single:
                    icons[0].Promote();
                    icons[1].gameObject.SetActive(false);
                    icons[0].SetTrait(TraitsUnscaled.GetTraits()[0]);
                    break;
                case PlantSize.Tall:
                    icons[0].SetTrait(TraitsUnscaled.GetTraits()[0]);
                    icons[1].SetTrait(TraitsUnscaled.GetTraits()[1]);
                    break;
            }
        }
        private void Awake() // hack, todo fix this
        {

            PlantGrowth = GetComponent<PlantGrowth>();
            
            /*
            if (thisPlantsSize == PlantSize.Single)
            {
                icons[0].Promote();
                icons[1].gameObject.SetActive(false);
                icons[0].SetTrait(Traits.GetTraits()[0]);
            }
            */

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


        private void OnMouseDown()
        {
            switch (thisPlantsSize)
            {
                case PlantSize.Single:
                    icons[0].PopUp();
                    
                    break;
                case PlantSize.Tall:
                    icons[0].PopUp();
                    icons[1].PopUp();
                    break;
            }
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


        public void RemoveSelfFromSever()
        {
            switch (StoredInGarden)
            {
                case Player.PlayerEnum.Player1:
                    GameCore.GameManager.Instance.DataManager.RemovePlantFromGarden1(StoredInSlot);
                    break;
                case Player.PlayerEnum.Player2:
                    GameCore.GameManager.Instance.DataManager.RemovePlantFromGarden2(StoredInSlot);
                    break;
            }
        }
        /*private void OnMouseDown()
        {
            
        }
        */
    }

}