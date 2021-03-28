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
        PlantGrowth plant;
        private void Awake()
        {
            plant = GetComponent<PlantGrowth>();
        }
    }
}