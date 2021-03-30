using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mood;

// created zap and jay 28/03

namespace Tutorial
{

    public class CatChatter : MonoBehaviour
    {
        public TextBox myBox;


        bool hasEverPlantedMoodRelaventPlant = false;

        delegate bool Condition();

        private void OnEnable()
        {
            BindEvent(EventsManager.EventType.StartGame, StartTurnOne);
            BindEvent(EventsManager.EventType.PlacedOwnObject, PlantedFirstPlant);

            BindEvent(EventsManager.EventType.PlantReadyToGrow, PlantGrows);

            BindEvent(EventsManager.EventType.PlacedOwnObjectMoodRelavent, PlantedFirstMoodRelevantPlant,
                      sideEffects: () => hasEverPlantedMoodRelaventPlant = true);

            BindEvent(EventsManager.EventType.NewTurnBegin, StartTurnTwoWithNoRelaventPlants,
                      condition: () =>
                      {
                          return GameManager.Instance.HotSeatManager.TurnTracker.Turn > 1 && !hasEverPlantedMoodRelaventPlant;
                      });


            BindEvent(EventsManager.EventType.NewTurnBegin, MoodRelevantPlantReachesMaturity,
                condition: () => 
                {
                    TraitValue gardenCurrentTrait = GameManager.Instance.EmotionTracker.GardenCurrentTraits[GameManager.Instance.ActivePlayer.PlayerEnumValue];
                    TraitValue gardenGoalTrait = GameManager.Instance.EmotionTracker.GardenGoalTraits[GameManager.Instance.ActivePlayer.PlayerEnumValue];


                    float defaultDistance = TraitValue.Distance(TraitValue.Zero, gardenGoalTrait);
                    float currentDistance = TraitValue.Distance(gardenCurrentTrait, gardenGoalTrait);

                    return currentDistance < defaultDistance;
                }); 

            BindEvent(EventsManager.ParameterEventType.NotEnoughPointsForAction, NoMorePoints);
   

            //EventsManager.InvokeEvent(EventsManager.ParameterEventType.NotEnoughPointsForAction, new EventsManager.EventParams() { EnumData = TurnPoints.PointType.SelfObjectPlace });

           // BindEvent(EventsManager.EventType.PlantReadyToGrow,)
        }

        /// <summary>
        /// Binds a function event to then be unbound after it is invoked
        /// </summary>
        /// <param name="eventType">The event that will trigger the passed function</param>
        /// <param name="tutorialToCall">The funcion to be called</param>
        /// <param name="sideEffects">Any side effects</param>
        /// <param name="condition">Any other conditions to the funciton being called</param>
        /// <param name="unbindIfFailCondition">If the function should be unbound if the condition fails</param>
        void BindEvent(EventsManager.EventType eventType, System.Action tutorialToCall, System.Action sideEffects = null, System.Func<bool> condition = null, bool unbindIfFailCondition = false)
        {
            void func() => LaunchTutorial(func, eventType, tutorialToCall, sideEffects, condition, unbindIfFailCondition);
            EventsManager.BindEvent(eventType, func);
        }   
        /// <summary>
        /// Overload for paramatised function
        /// </summary>
        void BindEvent(EventsManager.ParameterEventType eventType, System.Action<EventsManager.EventParams> tutorialToCall, System.Action sideEffects = null, System.Func<bool> condition = null, bool unbindIfFailCondition = false)
        {
            void paramFunc(EventsManager.EventParams eventParams) => LaunchTutorialParamatised(paramFunc, eventType, tutorialToCall, eventParams, sideEffects, condition, unbindIfFailCondition);
            EventsManager.BindEvent(eventType, paramFunc);
        }

        void LaunchTutorial(System.Action func, EventsManager.EventType eventType, System.Action tutorial, System.Action sideEffects, System.Func<bool> condition, bool unbindIfFailCondition)
        {
            if (GameManager.Instance.ActivePlayer.PlayerEnumValue != Player.PlayerEnum.Player1)
                return;


            if (condition == null || condition())
            {
                myBox.gameObject.SetActive(true);

                tutorial();

                sideEffects?.Invoke();

                EventsManager.UnbindEvent(eventType, func);
            }
            else if (condition != null && unbindIfFailCondition)
            {
                EventsManager.UnbindEvent(eventType, func);
            }
        } 
        
        void LaunchTutorialParamatised(System.Action<EventsManager.EventParams> func, EventsManager.ParameterEventType eventType, System.Action<EventsManager.EventParams> tutorial, EventsManager.EventParams eventParams, System.Action sideEffects, System.Func<bool> condition, bool unbindIfFailCondition)
        {
            if (GameManager.Instance.ActivePlayer.PlayerEnumValue != Player.PlayerEnum.Player1)
                return;


            if (condition == null || condition())
            {
                myBox.gameObject.SetActive(true);

                tutorial(eventParams);

                sideEffects?.Invoke();

                EventsManager.UnbindEvent(eventType, func);
            }
            else if (condition != null && unbindIfFailCondition)
            {
                EventsManager.UnbindEvent(eventType, func);
            }
        }


        //these functions could be modified to include if statements or just bound directly to the 
        void StartTurnOne()
        {
            myBox.Say("Hello there! Welcome to your garden!");
            myBox.Say("This place could do with some flora, don't you think?");
            myBox.Say("When you're ready, you can choose a plant to plant by tapping this seed basket!");
        }
        void PlantedFirstPlant()
        {
            myBox.Say("Wow, this place is looking beautiful already!");
            myBox.Say("Don't forget to water it by using the can in the toolbox!");
        }
        void PlantedFirstMoodRelevantPlant()
        {
            myBox.Say("That plant is so in sync with you!");
            myBox.Say("Well done! A few more of those are just what this place needs!");
        }
        void StartTurnTwoWithNoRelaventPlants()//this would be called when turn two starts but the player hasn't planted a mood relevant plant
        {
            myBox.Say("Good morning!");
            myBox.Say("Today, why not try planting a plant with a trait from your mood?");
        }

        void PlantGrows()
        {
            myBox.Say("It looks like this plant has been well looked after...");
            myBox.Say("by the start of your next turn it will probably have grown!");
        }

        void MoodRelevantPlantReachesMaturity()
        {
            myBox.Say("Now this garden is really getting going!");
        }

        void NoMorePoints(EventsManager.EventParams eventParams)
        {
            switch ((TurnPoints.PointType)eventParams.EnumData)
            {
                case TurnPoints.PointType.SelfObjectPlace:
                    myBox.Say("Whoa there! You can only plant one plant in your garden each turn!");  
                    break;
                case TurnPoints.PointType.OtherObjectPlace:
                    myBox.Say("You can't plant in your companion's garden again until your next turn, sorry.");
                    break;
                default: throw new System.ArgumentException();
            }
        }


    }
}