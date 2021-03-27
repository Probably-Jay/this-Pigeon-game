using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.ObjectModel;

namespace Plants
{
    namespace PlantActions
    {

        public enum TendingActions
        {
            Watering
                    
        }


        [CreateAssetMenu(menuName = "Plants/TendingRequiremnts", order = 1)]
        public class TendingState : ScriptableObject
        {
            [SerializeField] Plant.PlantSize size;

            #region UI Lists
            // Some plants take multiple days of passing thier growth requiremetns before they grow
            [SerializeField ]List<TendingActions> growthRequiremetsStage1;
            [SerializeField ]List<TendingActions> growthRequiremetsStage2;
            [SerializeField ]List<TendingActions> growthRequiremetsStage3;
            [SerializeField ]List<TendingActions> growthRequiremetsStage4;
            [SerializeField ]List<TendingActions> growthRequiremetsStage5;
            [SerializeField ]List<TendingActions> growthRequiremetsStage6;
            [SerializeField ]List<TendingActions> growthRequiremetsStage7;
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

            public static readonly ReadOnlyDictionary<Plant.PlantSize, int[]> EmotionValues = new ReadOnlyDictionary<Plant.PlantSize, int[]>
            (
                new Dictionary<Plant.PlantSize, int[]>
                {
                    {Plant.PlantSize.Single, new int[3]{1,2,3} }
                    ,{Plant.PlantSize.Tall, new int[3]{1,3,5} }
                    ,{Plant.PlantSize.Wide, new int[3]{1,4,7} }
                }    
            );



            List<TendingActions>[] growthRequirements;

            public int GrowthStage { get; private set; }




           // public List<TendingActions> CurrentRequiredActions => new(List<TendingActions>)



            //public int WateredLevel { get => tends[TendingActions.Watering]; set => tends[TendingActions.Watering] = value; }

            //readonly Dictionary<TendingActions, int> tends = new Dictionary<TendingActions, int>();






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

