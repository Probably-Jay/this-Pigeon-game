using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// careted zap and jay 28/03

namespace Tutorial
{

    public class CatChatter : MonoBehaviour
    {
        public TextBox myBox;


        bool hasEverPlantedMoodRelaventPlant;

        delegate bool Condition();

        private void OnEnable()
        {
            BindEvent(EventsManager.EventType.StartGame, StartTurnOne);
            BindEvent(EventsManager.EventType.PlacedOwnObject, PlantedFirstPlant);

            BindEvent(EventsManager.EventType.PlacedOwnObjectMoodRelavent, PlantedFirstMoodRelevantPlant,
                      sideEffects: () => hasEverPlantedMoodRelaventPlant = true);

            BindEvent(EventsManager.EventType.NewTurnBegin, StartTurnTwoWithNoRelaventPlants,
                      condition: () =>  !hasEverPlantedMoodRelaventPlant, 
                      unbindIfFailCondition: true);

            BindEvent(EventsManager.EventType.AddedToEmotionGoal, MoodRelevantPlantReachesMaturity);
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
        void MoodRelevantPlantReachesMaturity()
        {
            myBox.Say("Now this garden is really getting going!");
        }
    }
}