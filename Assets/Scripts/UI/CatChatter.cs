using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mood;
using System;
using Tutorial;

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

        ArrowEnabler myArrows;

        private void OnEnable()
        {
            myArrows = GetComponent<ArrowEnabler>();
            BindEvent(EventsManager.EventType.StartGame, StartTurnOne);
            BindEvent(EventsManager.EventType.NewTurnBegin, StartedThirdTurn ,condition:()=> { return GameManager.Instance.HotSeatManager.TurnTracker.Turn > 4; });
            BindEvent(EventsManager.EventType.PlacedOwnObject, PlantedFirstPlant);

            BindEvent(EventsManager.EventType.PlantReadyToGrow, PlantGrows);

            BindEvent(EventsManager.EventType.PlacedOwnObjectMoodRelevant, SayNothing,
                      sideEffects: () => hasEverPlantedMoodRelaventPlant = true);

            BindEvent(EventsManager.EventType.NewTurnBegin, StartTurnTwoWithRelaventPlants,
                      condition: () =>
                      {
                          return GameManager.Instance.HotSeatManager.TurnTracker.Turn > 1 && hasEverPlantedMoodRelaventPlant;
                      }); 
            
            BindEvent(EventsManager.EventType.NewTurnBegin, StartTurnTwoWithNoRelaventPlants,
                      condition: () =>
                      {
                          return GameManager.Instance.HotSeatManager.TurnTracker.Turn > 1 && !hasEverPlantedMoodRelaventPlant;
                      });


            BindEvent(EventsManager.EventType.NewTurnBegin, MoodRelevantPlantReachesMaturity,
                condition: () => 
                {
                    TraitValue gardenCurrentTrait = GameManager.Instance.EmotionTracker.GardenCurrentTraits[GameManager.Instance.ActivePlayerID];
                    TraitValue gardenGoalTrait = GameManager.Instance.EmotionTracker.GardenGoalTraits[GameManager.Instance.ActivePlayerID];


                    float defaultDistance = TraitValue.Distance(TraitValue.Zero, gardenGoalTrait);
                    float currentDistance = TraitValue.Distance(gardenCurrentTrait, gardenGoalTrait);

                    return currentDistance < defaultDistance;
                }); 

            BindEvent(EventsManager.ParameterEventType.NotEnoughPointsForAction, NoMorePoints);


            BindEvent(EventsManager.ParameterEventType.AcheivedGoal, AcheivedGoal);
   

            //EventsManager.InvokeEvent(EventsManager.ParameterEventType.NotEnoughPointsForAction, new EventsManager.EventParams() { EnumData = TurnPoints.PointType.SelfObjectPlace });

           // BindEvent(EventsManager.EventType.PlantReadyToGrow,)
        }

        #region BindingAndUnbinding


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

            if (GameManager.Instance.ActivePlayerID != player1)
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
            if (GameManager.Instance.ActivePlayerID != player1)
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
        #endregion


        public void GetPoked()
        {
            EventsManager.InvokeEvent(EventsManager.EventType.PokePet);
        }

        void SayNothing() { } 

        void StartTurnOne()
        {

            myBox.Say("Hello there! Welcome to your garden!");
         

            string emotion = GoalEmotion().ToString();
            
            switch (emotion)
            {
                case "Loving":
                    myBox.Say($"You're feeling {GetEmotionOutput(emotion)}? That's great!");
                    myBox.Say($"Let's share that love! And as with all good things, we can say it with flowers!");
                    break;
                case "Lonely":
                    myBox.Say($"I'm sorry to hear that you're feeling {GetEmotionOutput(emotion)}");
                    myBox.Say($"Why not share that with your partner, too?");
                    break;
                case "Stressed":
                    myBox.Say($"You're {GetEmotionOutput(emotion)}? Then you've come to the right place!");
                    myBox.Say($"Let's unwind by planting growing some plants that express that feeling!");
                    break;
                case "Excited":
                    myBox.Say($"I'm {GetEmotionOutput(emotion)} too! Let's get going then!");
                    myBox.Say($"The goal of this garden is to communicate that feeling to your partner!");
                    myBox.Say($"Hopefully they're as jazzed as us!");
                    break;
                default:
                    break;
            }

  
            myBox.Say("When you're ready, you can choose which seed to plant this turn by tapping this seed basket!");
            myArrows.EnableArrow(ArrowScript.ArrowPurpose.SeedBox);
        }

        private string GetEmotionOutput(string emotion) => $"<b>{emotion}</b>";

        void StartedThirdTurn() {
            myBox.Say("Looks like both you and your partner are getting along nicely!");
            myBox.Say("Did you know you can gift them a plant?");
            myBox.Say("Simply go to their garden and plant a seed!");
            myArrows.EnableArrow(ArrowScript.ArrowPurpose.SwapGarden);
        }


        void PlantedFirstPlant()
        {
            myBox.Say("Wow, this place is looking beautiful already!");
            ExplainTending();
        }

        void StartTurnTwoWithRelaventPlants()
        {
            myBox.Say("Good morning!");
            ExplainTraits();
            myBox.Say($"That last plant was super in sync with you...");
            myBox.Say("Why not plant another with a different trait from your mood?");

        }
        void StartTurnTwoWithNoRelaventPlants()
        {
            myBox.Say("Good morning!");
            ExplainTraits();
            myBox.Say("Why not try planting a plant with a trait from your mood?");
 
        }

        void ExplainTending()
        {
            myBox.Say("Don't forget to water it by using the can in the toolbox!");
            EventsManager.BindEvent(EventsManager.EventType.ToolBoxOpen, ActivateCanArrow);
        }
        void ActivateCanArrow()
        {
            EventsManager.UnbindEvent(EventsManager.EventType.ToolBoxOpen, ActivateCanArrow);
            myArrows.EnableArrow(ArrowScript.ArrowPurpose.WateringCan);
        }

        void ExplainTraits()
        {
            EventsManager.InvokeEvent(EventsManager.EventType.moodSlidersExplanation);
            Emotion.Emotions emotion = GoalEmotion();
            var traits = Emotion.GetScalesInEmotion(emotion);
            myBox.Say($"The <b>emotion</b> you chose is {emotion}, right?");
            myBox.Say($"Did you know that <b>emotions</b> are comprised of <b>traits</b>?");
            myBox.Say($"For example, {emotion} is made up of <i>{traits.Item1}</i>{TraitValue.GetIconDisplay(traits.Item1)} and <i>{traits.Item2}</i>{TraitValue.GetIconDisplay(traits.Item2)}");
            myBox.Say($"A plant only contributes toward your garden's <b>emotion</b> once it fully matured, so make sure to tend to your plants!");
          
        }


        void PlantGrows()
        {
            myBox.Say("Looks like that plant really needed that!");
            myBox.Say("By the start of your next turn it will probably have grown!");
            myBox.Say("Plants will only grow if they have been well looked after, so make sure to keep an eye out for thier needs.");
            myBox.Say($"You can use the humane repellant to chase away any bugs and use the shears to trim extra leaves too!");
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

        private void AcheivedGoal(EventsManager.EventParams eventParams)
        {
            if ((Player.PlayerEnum)eventParams.EnumData != player1)
                return;

            myBox.Say("You did it! Your garden now reflects your mood as well as you communicated it.");


            var goal = GameManager.Instance.EmotionTracker.GardenGoalEmotions[player1];

            switch (goal)
            {
                case Emotion.Emotions.Loving:
                       myBox.Say($"Do you feel the love in the air? Because I do!");
                    break;
                case Emotion.Emotions.Excited:
                       myBox.Say($"Wow! I can barely stay in one place. Thank you for sharing all your positive vibes with me!");
                    break;
                case Emotion.Emotions.Stressed:
                       myBox.Say($"Finding some time for yourself is a very hard thing to do while stressed. You did well, playing game is a form of self-care!");
                    break;
                case Emotion.Emotions.Lonely:
                       myBox.Say($"Thank you for sharing your loneliness with me and your gardening partner. Opening up about your feelings is so hard and I'm proud of you.");
                    break;
            }

            myBox.Say("You can carry on playing the game in an endless mode if you like");

        }

    }
}