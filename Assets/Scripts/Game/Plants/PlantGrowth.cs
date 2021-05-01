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

    

        public enum VisibleGrowthStage
        {
            Seed = 0,
            Sprout,
            Bloom

        }

        VisibleGrowthStage VisibleGrowthState => TendingState.VisibleGrowthStage;


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
            //  visibleGrowthState = VisibleGrowthStage.Seed;
            UpdateVisibleGrowthStage();
            tendingState.Init();
        }

        public void SetState(NetSystem.GardenDataPacket.Plant plantData)
        {
            tendingState.SetState(plantData);
            UpdateVisibleGrowthStage();
        }
        public void SaveState(Player.PlayerEnum storedInGarden, int storedInSlot)
        {
            tendingState.SaveState(storedInGarden, storedInSlot);
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
        public void GrowIfShould()
        {
            if (TendingState.ReadyToProgressStage)
            {
                ProgressGrowthStage();
            }
            UpdateVisibleGrowthStage();
        }

        public event Action ReachedMaturity;

        private void UpdateVisibleGrowthStage()
        {
            if (VisibleGrowthState == VisibleGrowthStage.Bloom)
            {
                GrowthLevelMoodMultiplier = 1;
                ReachedMaturity?.Invoke();

            }
            UpdateGrowthImage();
            UpdateGrowthCollider();
        }


        //void VisiblyGrow()
        //{
        //    visibleGrowthState++;
        //    if(VisibleGrowthState == VisibleGrowthStage.Bloom)
        //    {
        //        GrowthLevelMoodMultiplier = 1;
        //        ReachedMaturity?.Invoke();

        //    }
        //    UpdateGrowthImage();
        //    UpdateGrowthCollider();
        //}

        void UpdateGrowthImage()
        {
            spriteRenderer.sprite = growthSprites[(int)VisibleGrowthState];
        }

        void UpdateGrowthCollider()
        {
            foreach (Collider2D col in growthColliders)
            {
                if (col == growthColliders[(int)VisibleGrowthState])
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
        public bool Tend(TendingActions action)
        {
            if (!TendingState.RequiresAction(action))
            {
                Debug.Log($"This plant does not need the {action} action performed");
                return false;
            }

            TendingState.Tend(action);
            EventsManager.InvokeEvent(EventsManager.ParameterEventType.OnPerformedTendingAction, new EventsManager.EventParams() { EnumData1 = action });

            return true;

        }
        public Collider2D GetActiveCollider()
        {
            return growthColliders[(int)VisibleGrowthState];
        }

 

    }
}