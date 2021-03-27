using Plants.PlantActions;
using System.Collections.Generic;
using UnityEngine;

namespace Plants
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlantGrowth : MonoBehaviour
    {

        public enum PlantGrowthStage
        {
            Seed = 0,
            Sprout,
            Bloom

        }

        PlantGrowthStage growthState;


        [SerializeField] Sprite[] growthSprites;

        //Get the Renderer component for changing colours (temp, replace with actual different sprites later)
        Renderer matRenderer;

        SpriteRenderer spriteRenderer;

        private void Awake()
        {
            matRenderer = GetComponent<Renderer>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            growthState = PlantGrowthStage.Seed;
            UpdateGrowthImage();
        }

     
        public int GrowthLevelMoodMultiplier { get; set; } = 0;

      //  [SerializeField] public int growthGoal = 1;
     //   public int currGrowth = 0;

        [SerializeField] List<TendingActions> legalTendingActions;
        public List<TendingActions> LegalTendingActions { get => legalTendingActions; set => legalTendingActions = value; }

        public void UpdateSprite()
        {

            switch (growthState)
            {
                case PlantGrowthStage.Seed:
                    matRenderer.material.SetColor("_Color", Color.red);
                    break;

                case PlantGrowthStage.Sprout:
                    matRenderer.material.SetColor("_Color", Color.yellow);
                    break;

                case PlantGrowthStage.Bloom:
                    matRenderer.material.SetColor("_Color", Color.green);
                    break;
            }

        }
        private void UpdateGrowthImage()
        {
            spriteRenderer.sprite = growthSprites[(int)growthState];
        }

        public void WaterPlant()
        {
            //if (currGrowth <= growthGoal)
            //{
            //    plant.currGrowth += waterInc;
            //}
        }

        TendingState tendingState = new TendingState();

    }
}