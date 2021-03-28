using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Plants.PlantActions
{
    [CreateAssetMenu(menuName = "Plants/TendingIconList", order = 1)]
    public class ActionIcons : ScriptableObject
    {
        public Sprite watering;
        public Sprite staking;
        public Sprite spraying;
        public Sprite trimming;

    }
}