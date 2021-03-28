using Plants.PlantActions;
using System;
using System.Collections.Generic;
using UnityEngine;

// jay 26/03

namespace Plants
{
    /// <summary>
    /// Part of the plant responsible for growth and managing tending tending. Controlled by <see cref="Plant"/>
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Plant))]
    public class PlantGrowth : MonoBehaviour
    {

        private void OnEnable()
        {
            EventsManager.BindEvent(EventsManager.EventType.EndTurn, GrowIfShould);
        }
        private void OnDisable()
        {
            EventsManager.UnbindEvent(EventsManager.EventType.EndTurn, GrowIfShould);
        }


        public enum VisibleGrowthStage
        {
            Seed = 0,
            Sprout,
            Bloom

        }

        VisibleGrowthStage growthState;


        [SerializeField] Sprite[] growthSprites;

        [SerializeField] TendingState tendingState;
        public TendingState TendingState { get => tendingState; private set => tendingState = value; }



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
            growthState = VisibleGrowthStage.Seed;
           // UpdateGrowthImage();
        }

     
        public int GrowthLevelMoodMultiplier { get; private set; } = 0;


        //[SerializeField] List<TendingActions> legalTendingActions;
        //public List<TendingActions> LegalTendingActions { get => legalTendingActions; set => legalTendingActions = value; }




        /// <summary>
        /// Grow the plant or progress to the next growth stage
        /// </summary>
        private void GrowIfShould()
        {
            if (TendingState.ReadyToVisiblyGrow)
            {
                ProgressGrowthStage();
                VisiblyGrow();
            }
            else if (TendingState.ReadyToProgressStage)
            {
                ProgressGrowthStage();
            }
        }

        void VisiblyGrow()
        {
            growthState++;
            UpdateSprite();
            //UpdateGrowthImage();
        }

        void UpdateGrowthImage()
        {
            spriteRenderer.sprite = growthSprites[(int)growthState];
        }

        void UpdateSprite()
        {

            switch (growthState)
            {
                case VisibleGrowthStage.Seed:
                    matRenderer.material.SetColor("_Color", Color.red);
                    break;

                case VisibleGrowthStage.Sprout:
                    matRenderer.material.SetColor("_Color", Color.yellow);
                    break;

                case VisibleGrowthStage.Bloom:
                    matRenderer.material.SetColor("_Color", Color.green);
                    break;
                default: throw new System.ArgumentException();
            }

        }

        void ProgressGrowthStage() => TendingState.ProgressGrowthStage();


        /// <summary>
        /// Tend the plant. No effect if this plant does not need this action taken
        /// </summary>
        public void Tend(TendingActions action)
        {
            if (!TendingState.CanTend(action))
            {
                
                return;
            }

            TendingState.Tend(action);

            UpdateTendingIcons();

        }

        private void UpdateTendingIcons()
        {
            throw new NotImplementedException();
        }


    }
}