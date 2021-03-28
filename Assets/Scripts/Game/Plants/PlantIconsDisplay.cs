using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plants
{
    
    /// <summary>
    /// Class that displays the icons that a plant needs
    /// </summary>
    [RequireComponent(typeof(Plant))]
    [RequireComponent(typeof(PlantGrowth))]
    public class PlantIconsDisplay : MonoBehaviour
    {



        PlantGrowth plantGrowth;
        Plant palnt;
        private void Awake()
        {
            plantGrowth = GetComponent<PlantGrowth>();
        }

        private void OnEnable()
        {
            plantGrowth.TendingState.OnPlantTended += TendingState_OnPlantTended;
        }



        private void OnDisable()
        {
            plantGrowth.TendingState.OnPlantTended -= TendingState_OnPlantTended;
        }

        
        private void TendingState_OnPlantTended()
        {
            UpdateIcons();
        }

        private void UpdateIcons()
        {
            throw new NotImplementedException();
        }
    }
        
}