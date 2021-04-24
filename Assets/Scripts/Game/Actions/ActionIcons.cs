using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Plants.PlantActions
{
    [CreateAssetMenu(menuName = "Plants/TendingIconList", order = 1)]
    public class ActionIcons : ScriptableObject
    {
        public GameObject watering;
     //   public GameObject staking;
        public GameObject spraying;
        public GameObject trimming;
        public GameObject removing;
    }
}