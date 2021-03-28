using System.Collections;
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
        }


        [CreateAssetMenu(menuName = "Plants/TendingRequiremnts", order = 1)]
        public class TendingState : ScriptableObject
        {
            [SerializeField] Plant.PlantSize plantSize;

            #region UI Lists
            // Some plants take multiple days of passing thier growth requiremetns before they grow
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

            static readonly ReadOnlyDictionary<Plant.PlantSize, List<int>> ArtChagesAt = new ReadOnlyDictionary<Plant.PlantSize, List<int>>
            (
                new Dictionary<Plant.PlantSize, List<int>>
                {
                    {Plant.PlantSize.Single, new List<int>(){1,2,3} }
                    ,{Plant.PlantSize.Tall, new List<int>(){1,3,5} }
                    ,{Plant.PlantSize.Wide, new List<int>(){1,4,7} }
                }
            );


            List<TendingActions>[] growthRequirements;

            int currentGrowthStage = 0;
            int MaxGrowthStage => (ArtChagesAt[plantSize][ArtChagesAt[plantSize].Count - 1]) +1 ;
            bool AtFullStageOfGrowth => currentGrowthStage == MaxGrowthStage;



            List<TendingActions> RequiredActions => !AtFullStageOfGrowth ? growthRequirements[currentGrowthStage] : new List<TendingActions>();
            public ReadOnlyCollection<TendingActions> GetRequiredActions() => new ReadOnlyCollection<TendingActions>(RequiredActions);

            /// <summary>
            /// If the plant is ready to progress to the next growth stage at the end of this turn
            /// </summary>
            public bool ReadyToProgressStage => !AtFullStageOfGrowth && RequiredActions.Count == 0;

            /// <summary>
            /// If the plant is ready to change it's art at the end of this turn
            /// </summary>
            public bool ReadyToVisiblyGrow => ReadyToProgressStage && NextStageIsArtChange;
            bool NextStageIsArtChange => ArtChagesAt[plantSize].Contains(currentGrowthStage + 1);

            /// <summary>
            /// If this plant needs this action done to it
            /// </summary>
            public bool CanTend(TendingActions action) => RequiredActions.Contains(action);

            /// <summary>
            /// Removes the tending action from the <see cref="RequiredActions"/>
            /// </summary>
            public void Tend(TendingActions action)
            {
                Debug.Log($"Tended {action.ToString()} successfully");
                RequiredActions.Remove(action);
                OnPlantTended?.Invoke();
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
                if (ReadyToProgressStage)
                {
                    currentGrowthStage++;
                    OnPlantGrowth?.Invoke();
                }
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

