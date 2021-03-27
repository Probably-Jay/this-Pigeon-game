using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plants
{
    namespace PlantActions
    {

        public enum TendingActions
        {
            Watering
            
        }



        public class TendingState
        {
           // int wateredLevel;
            public int WateredLevel { get => tends[TendingActions.Watering]; set => tends[TendingActions.Watering] = value; }

            readonly Dictionary<TendingActions, int> tends = new Dictionary<TendingActions, int>();




            public TendingState(Plant.PlantSize size)
            {

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

