﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.ObjectModel;
using System;

namespace Plants
{
    namespace PlantActions
    {

        public enum TendingActions
        {
            Watering
        //    , Staking        
            , Spraying        
            , Trimming  
            , Removing
        }


        [CreateAssetMenu(menuName = "Plants/TendingRequirements", order = 1)]
        public class TendingState : ScriptableObject
        {
            [SerializeField] Plant.PlantSize plantSize;

        
            #region UI Lists
            // Some plants take multiple days of passing thier growth requiremetns before they grow
            [Header("Growth Stages")]
            [SerializeField] List<TendingActions> growthRequiremetsStage1;
            [SerializeField] List<TendingActions> growthRequiremetsStage2;
            [SerializeField] List<TendingActions> growthRequiremetsStage3;
            [SerializeField] List<TendingActions> growthRequiremetsStage4;
            [SerializeField] List<TendingActions> growthRequiremetsStage5;
            [SerializeField] List<TendingActions> growthRequiremetsStage6;
            [SerializeField] List<TendingActions> growthRequiremetsStage7;
            // [SerializeField ]List<TendingActions> growthRequiremetsStage8;
            // [SerializeField ]List<TendingActions> growthRequiremetsStage9;
            // [SerializeField ]List<TendingActions> growthRequiremetsStage10;
            [Header("Max Growth Behaviour")]
            [SerializeField] List<TendingActions> maxGrowthRequirements;

            void OnEnable()
            {
                growthRequirements = new List<TendingActions>[7]
                {
                    growthRequiremetsStage1
                    ,growthRequiremetsStage2
                    ,growthRequiremetsStage3
                    ,growthRequiremetsStage4
                    ,growthRequiremetsStage5
                    ,growthRequiremetsStage6
                    ,growthRequiremetsStage7
            //        ,growthRequiremetsStage8
            //        ,growthRequiremetsStage9
            //        ,growthRequiremetsStage10
                };
            }

            #endregion
          

            public void SetState(NetSystem.GardenDataPacket.Plant plantData)
            {
                CurrentGrowthStage = plantData.stage;

                if (!plantData.watering && RequiresAction(TendingActions.Watering))
                {
                    RequiredActions.Remove(TendingActions.Watering);
                }                
                
                if (!plantData.spraying && RequiresAction(TendingActions.Spraying))
                {
                    RequiredActions.Remove(TendingActions.Spraying);
                }                
                
                if (!plantData.trimming && RequiresAction(TendingActions.Trimming))
                {
                    RequiredActions.Remove(TendingActions.Trimming);
                }

            }

            public void SaveState(Player.PlayerEnum storedInGarden, int storedInSlot)
            {
                GameCore.GameManager.Instance.DataManager.UpdatePlantState(
                    gardenNumber: storedInGarden,
                    slotNumber: storedInSlot,
                    stage: CurrentGrowthStage,
                    watering: RequiresAction(TendingActions.Watering),
                    spraying: RequiresAction(TendingActions.Spraying),
                    trimming: RequiresAction(TendingActions.Trimming)
                    );
            }

            static readonly ReadOnlyDictionary<Plant.PlantSize, List<int>> ArtChagesAt = new ReadOnlyDictionary<Plant.PlantSize, List<int>>
            (
                new Dictionary<Plant.PlantSize, List<int>>
                {
                    {Plant.PlantSize.Single, new List<int>(){0,1,2} }
                    ,{Plant.PlantSize.Tall, new List<int>(){0,2,4} }
                    ,{Plant.PlantSize.Wide, new List<int>(){0,3,6} }
                }
            );

            public Plants.PlantGrowth.VisibleGrowthStage VisibleGrowthStage
            {
                get
                {
                    List<int> artChangesStates = ArtChagesAt[plantSize];
                    int prevVal = artChangesStates[0];
                    PlantGrowth.VisibleGrowthStage stage;
                    for (int i = 0; i < artChangesStates.Count; i++)
                    {
                        int val = artChangesStates[i];
                        if (CurrentGrowthStage == val)
                        {
                            return (PlantGrowth.VisibleGrowthStage)i;
                        }
                        if (CurrentGrowthStage > val)
                        {
                            stage = (PlantGrowth.VisibleGrowthStage)prevVal;
                        }
                        prevVal = val;
                    }
                    return (PlantGrowth.VisibleGrowthStage)(artChangesStates.Count - 1);
                }
            }

            List<TendingActions>[] growthRequirements;

            int MaxGrowthStage => (ArtChagesAt[plantSize][ArtChagesAt[plantSize].Count - 1]);
            bool AtFullStageOfGrowth => CurrentGrowthStage >= MaxGrowthStage;




            List<TendingActions> RequiredActions => !AtFullStageOfGrowth ? growthRequirements[CurrentGrowthStage] : new List<TendingActions>(maxGrowthRequirements);
            public ReadOnlyCollection<TendingActions> GetRequiredActions() => new ReadOnlyCollection<TendingActions>(RequiredActions);

            /// <summary>
            /// If the plant is ready to progress to the next growth stage at the end of this turn
            /// </summary>
            public bool ReadyToProgressStage => !AtFullStageOfGrowth && RequiredActions.Count == 0;

            /// <summary>
            /// If the plant is ready to change it's art at the end of this turn
            /// </summary>
          //  public bool ReadyToVisiblyGrow => ReadyToProgressStage && NextStageIsArtChange;
         //   bool NextStageIsArtChange => ArtChagesAt[plantSize].Contains(CurrentGrowthStage + 1);

            public int CurrentGrowthStage { get; private set; } = 0;

            /// <summary>
            /// If this plant needs this action done to it
            /// </summary>
            public bool RequiresAction(TendingActions action) => RequiredActions.Contains(action);

            /// <summary>
            /// Removes the tending action from the <see cref="RequiredActions"/>
            /// </summary>
            public void Tend(TendingActions action)
            {
                RequiredActions.Remove(action);
                OnPlantTended?.Invoke();

               // GameCore.GameManager.Instance.DataManager.RemoveTendingActionFromPlant()

                if(RequiredActions.Count == 0) // for the tutorial
                {
                    EventsManager.InvokeEvent(EventsManager.EventType.PlantReadyToGrow);
                }

            }

            /// <summary>
            /// The event that this plant has grown
            /// </summary>
            public event Action OnPlantGrowth;           
            
            /// <summary>
            /// The event that this plant has been tended
            /// </summary>
            public event Action OnPlantTended;

            /// <summary>
            /// Grows the plant if ready
            /// </summary>
            public void ProgressGrowthStage()
            {
                Debug.Log("tryingToGrow");
                if (ReadyToProgressStage)
                {
                    Debug.Log("Growing!");
                    CurrentGrowthStage++;
                   
                    OnPlantGrowth?.Invoke();
                }
                
                //if (AtFullStageOfGrowth)
                //    FullGrowthBehaviour();
            }

            //private void FullGrowthBehaviour()
            //{
            //    throw new NotImplementedException();
            //}

            public void Init()
            {
                OnPlantGrowth?.Invoke();

            }


        }
    }
}


//public static bool AttemptToTendPlant(PlantItem plant, TendingActions tendingActions)
//{
//    if (!CanTendPlant(plant, tendingActions)) return false;

//    if (plant.)

//        plant.Tend(tendingActions);

//}


//static bool CanTendPlant(PlantItem plant, TendingActions tendingActions) => plant.LegalTendingActions.Contains(tendingActions);

