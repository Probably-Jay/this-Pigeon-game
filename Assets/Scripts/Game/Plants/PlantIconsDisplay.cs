using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Plants.PlantActions;
using System.Collections.ObjectModel;

namespace Plants
{
    
    /// <summary>
    /// Class that displays the icons that a plant needs
    /// </summary>
    [RequireComponent(typeof(Plant))]
    [RequireComponent(typeof(PlantGrowth))]
    public class PlantIconsDisplay : MonoBehaviour
    {


        [SerializeField] ActionIcons iconActions;
        PlantGrowth plantGrowth;

        ReadOnlyCollection<TendingActions> requiredActions;

        Dictionary<TendingActions, GameObject> icons = new Dictionary<TendingActions, GameObject>();

        //[SerializeField] Vector3 iconOrigin;
        //[SerializeField] float iconSize;
        //[SerializeField] float iconPadding;


        private void Awake()
        {
            plantGrowth = GetComponent<PlantGrowth>();
            AddIconObject(TendingActions.Watering, iconActions.watering);
            //   AddIconObject(TendingActions.Staking, iconActions.staking);
            AddIconObject(TendingActions.Spraying, iconActions.spraying);
            AddIconObject(TendingActions.Trimming, iconActions.trimming);
            AddIconObject(TendingActions.Removing, iconActions.removing);
        }

        private void OnEnable()
        {
            plantGrowth.TendingState.OnPlantTended += UpdateIcons;
            plantGrowth.TendingState.OnPlantGrowth += UpdateIcons;
        }



        private void OnDisable()
        {
            plantGrowth.TendingState.OnPlantTended -= UpdateIcons;
            plantGrowth.TendingState.OnPlantGrowth -= UpdateIcons;
        }

        private void Start()
        {
      
        }

        private void AddIconObject(TendingActions action, GameObject icon)
        {
            GameObject newIcon = Instantiate(icon);
            var localPos = newIcon.transform.position;
            newIcon.transform.SetParent(transform);

            newIcon.transform.localPosition = localPos;

            newIcon.SetActive(false);
            icons.Add(action, newIcon);
        }

        private void UpdateIcons()
        {

            foreach (var action in Helper.Utility.GetEnumValues<TendingActions>())
            {
                icons[action].SetActive(false);
            }

            requiredActions = plantGrowth.TendingState.GetRequiredActions();

            for (int i = 0; i < requiredActions.Count; i++)
            {
                var icon = icons[requiredActions[i]];
              //  icon.transform.position = GetPosition(i);
                icon.SetActive(true);
            }

        }

       // private Vector3 GetPosition(int i) => iconOrigin + new Vector3(i * (iconSize + iconPadding), 0, 0);
    }
        
}