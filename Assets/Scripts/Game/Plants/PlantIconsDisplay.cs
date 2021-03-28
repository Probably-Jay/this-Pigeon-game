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
        private void Awake()
        {
            plantGrowth = GetComponent<PlantGrowth>();
        }

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            
        }
    }
}