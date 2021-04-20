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

        VisibleGrowthStage visibleGrowthState;


        [SerializeField] Sprite[] growthSprites;
        [SerializeField] Collider2D[] growthColliders;

        [SerializeField] TendingState tendingState;
        public TendingState TendingState { get => tendingState; private set => tendingState = value; }
        public int GrowthLevelMoodMultiplier { get; private set; } = 0;



        //Get the Renderer component for changing colours (temp, replace with actual different sprites later)
        //  Renderer matRenderer;

        SpriteRenderer spriteRenderer;

        private void Awake()
        {
           // matRenderer = GetComponent<Renderer>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            tendingState = Instantiate(tendingState);
        }

        private void Start()
        {
            visibleGrowthState = VisibleGrowthStage.Seed;
            UpdateGrowthImage();
            UpdateGrowthCollider();
            tendingState.Init();
        }

     

        //private void Update()
        //{
        //    if (Input.GetMouseButtonDown(1))
        //    {
        //        foreach (var item in tendingState.GetRequiredActions())
        //        {
        //            Debug.Log(item);
        //        }
        //    }
        //}

        /// <summary>
        /// Grow the plant or progress to the next growth stage
        /// </summary>
        private void GrowIfShould()
        {
            Debug.Log("Enter try to grow");
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

        public event Action ReachedMaturity;

        void VisiblyGrow()
        {
            visibleGrowthState++;
            if(visibleGrowthState == VisibleGrowthStage.Bloom)
            {
                GrowthLevelMoodMultiplier = 1;
                ReachedMaturity?.Invoke();

            }
            UpdateGrowthImage();
            UpdateGrowthCollider();
        }

        void UpdateGrowthImage()
        {
            spriteRenderer.sprite = growthSprites[(int)visibleGrowthState];
        }

        void UpdateGrowthCollider()
        {
            foreach (Collider2D col in growthColliders)
            {
                if (col == growthColliders[(int)visibleGrowthState])
                {
                    col.enabled = true;
                }
                else
                {
                    col.enabled = false;
                }
            }
        }

        //void UpdateSprite()
        //{

        //    switch (growthState)
        //    {
        //        case VisibleGrowthStage.Seed:
        //            matRenderer.material.SetColor("_Color", Color.red);
        //            break;

        //        case VisibleGrowthStage.Sprout:
        //            matRenderer.material.SetColor("_Color", Color.yellow);
        //            break;

        //        case VisibleGrowthStage.Bloom:
        //            matRenderer.material.SetColor("_Color", Color.green);
        //            break;
        //        default: throw new System.ArgumentException();
        //    }

        //}

        void ProgressGrowthStage() => TendingState.ProgressGrowthStage();


        /// <summary>
        /// Tend the plant. No effect if this plant does not need this action taken
        /// </summary>
        public void Tend(TendingActions action)
        {
            if (!TendingState.CanTend(action))
            {
                Debug.Log($"This plant does not need the {action} action performed");
                return;
            }

            TendingState.Tend(action);

           

        }
        public Collider2D GetActiveCollider()
        {
            return growthColliders[(int)visibleGrowthState];
        }

 

    }
}