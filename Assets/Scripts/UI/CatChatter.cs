using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mood;
using System;

// created zap and jay 28/03

namespace Tutorial
{

    public class CatChatter : MonoBehaviour
    {
        private const Player.PlayerEnum player1 = Player.PlayerEnum.Player1;
        public TextBox myBox;


        private static Emotion.Emotions GoalEmotion() => GameManager.Instance.EmotionTracker.GardenGoalEmotions[player1];


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


            BindEvent(EventsManager.ParameterEventType.AcheivedGoal, AcheivedGoal);
   

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
        void BindEvent(EventsManager.EventType eventType, System.Action tutorialToCall, System.Action sideEffects = null, System.Func<bool> condition = null, bool unbindIfFailCondition = false, bool waitForYourTurn = false)
        {
            void func() => LaunchTutorial(func, eventType, tutorialToCall, sideEffects, condition, unbindIfFailCondition, waitForYourTurn);
            EventsManager.BindEvent(eventType, func);
        }   
        /// <summary>
        /// Overload for paramatised function
        /// </summary>
        void BindEvent(EventsManager.ParameterEventType eventType, System.Action<EventsManager.EventParams> tutorialToCall, System.Action sideEffects = null, System.Func<bool> condition = null, bool unbindIfFailCondition = false, bool waitForYourTurn = false)
        {
            void paramFunc(EventsManager.EventParams eventParams) => LaunchTutorialParamatised(paramFunc, eventType, tutorialToCall, eventParams, sideEffects, condition, unbindIfFailCondition, waitForYourTurn);
            EventsManager.BindEvent(eventType, paramFunc);
        }

        void LaunchTutorial(System.Action func, EventsManager.EventType eventType, System.Action tutorial, System.Action sideEffects, System.Func<bool> condition, bool unbindIfFailCondition, bool waitForYourTurn)
        {

            if (GameManager.Instance.ActivePlayer.PlayerEnumValue != player1)
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
        
        void LaunchTutorialParamatised(System.Action<EventsManager.EventParams> func, EventsManager.ParameterEventType eventType, System.Action<EventsManager.EventParams> tutorial, EventsManager.EventParams eventParams, System.Action sideEffects, System.Func<bool> condition, bool unbindIfFailCondition, bool waitForYourTurn)
        {
            if (GameManager.Instance.ActivePlayer.PlayerEnumValue != player1)
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
            List<string> emotionComments = new List<string> { };

            myBox.Say("Hello there! Welcome to your garden!");
            //myBox.Say("This place could do with some flora, don't you think?");
            string emotion = GoalEmotion().ToString();
            emotionComments.Clear();
            switch (emotion)
            {
                case "Loving":
                    emotionComments.Add("You're feeling " + emotion + "? That's great!");
                    emotionComments.Add("Let's share that love! And as with all good things, we can say it with flowers!");
                    break;
                case "Lonely":
                    emotionComments.Add("I'm sorry to hear that you're feeling " + emotion);
                    emotionComments.Add("Why not share that with your partner, too?");
                    break;
                case "Stressed":
                    emotionComments.Add("You're " + emotion + "? Then you've come to the right place!");
                    emotionComments.Add("Let's unwind by planting growing some plants that express that feeling!");
                    break;
                case "Excited":
                    emotionComments.Add("I'm " + emotion + " too! Let's get going then!");
                    emotionComments.Add("The goal of this garden is to communicate that feeling to your partner!");
                    emotionComments.Add("Hopefully they're as jazzed as us!");
                    break;
                default:
                    break;
            }

            foreach (string comment in emotionComments)
            {
                myBox.Say(comment);
            }
            emotionComments.Clear();
            myBox.Say("When you're ready, you can choose a seed to plant by tapping this seed basket!");
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
            ExplainTraits();
            myBox.Say("Why not try planting a plant with a trait from your mood?");
 
        }


        void ExplainTraits()
        {
            Emotion.Emotions emotion = GoalEmotion();
            var traits = Emotion.GetScalesInEmotion(emotion);
            myBox.Say($"The <b>emotion</b> you chose is {emotion}, right?");
            myBox.Say($"Did you know that <b>emotions</b> are made up of <b>traits</b>?");
            myBox.Say($"For example, {emotion} is made up of <i>{traits.Item1}</i>{TraitValue.GetIconDisplay(traits.Item1)} and <i>{traits.Item2}</i>{TraitValue.GetIconDisplay(traits.Item2)}");
        }


        void PlantGrows()
        {
            myBox.Say("It looks like this plant has been well looked after...");
            myBox.Say("by the start of your next turn it will probably have grown!");
        }

        void MoodRelevantPlantReachesMaturity()
        {
            myBox.Say("Now this garden is really getting going!");
            ExplainTraits();
           

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

        private void AcheivedGoal(EventsManager.EventParams eventParams)
        {
            if ((Player.PlayerEnum)eventParams.EnumData != player1)
                return;

            myBox.Say("Congratulations, your garden now totally reflects your mood!");


            var goal = GameManager.Instance.EmotionTracker.GardenGoalEmotions[player1];

            switch (goal)
            {
                case Emotion.Emotions.Loving:
                    // myBox.Say($"I");
                    break;
                case Emotion.Emotions.Excited:
                    // myBox.Say($"I");
                    break;
                case Emotion.Emotions.Stressed:
                  // myBox.Say($"I");
                    break;
                case Emotion.Emotions.Lonely:
                    //myBox.Say($"This garden really communicates a loneliness to me");
                    break;
            }

           // myBox.Say(" kee playing endlless etc. ");

        }

    }
}