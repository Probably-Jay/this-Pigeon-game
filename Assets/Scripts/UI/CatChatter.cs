using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mood;
using System;
using Tutorial;
using Plants;

// created zap and jay 28/03
// edited zap 24/04 (added arrow system integration)
// edited Zap 28/04 (added nudging feature)

namespace Tutorial
{

    public class CatChatter : MonoBehaviour
    {
        private const Player.PlayerEnum player1 = Player.PlayerEnum.Player1;
        public TextBox myBox;

        public int nudgeTime = 20;

        private float secondsSinceAction=0f;

        private static Emotion.Emotions GoalEmotion() => GameManager.Instance.EmotionTracker.GardenGoalEmotions[player1];



        bool hasEverPlantedMoodRelaventPlant = false;

        delegate bool Condition();

        ArrowEnabler myArrows;

        enum PlayerAction
        {
               PlacePlant
            ,  TendPlant
            ,  GiftPlant
            ,  EndTurn
        }

        private PlayerAction nextAction;

        private void Update()
        {
            if (secondsSinceAction > nudgeTime)
            {
                secondsSinceAction = 0f;
                CheckAndNudgePlayer();
            }
            else
            {
                secondsSinceAction += Time.deltaTime;
            }
        }

        private void OnEnable()
        {
            EventsManager.BindEvent(EventsManager.ParameterEventType.SwappedGardenView, ParamaterResetActionTimer);
            EventsManager.BindEvent(EventsManager.ParameterEventType.OnPerformedTendingAction, ParamaterResetActionTimer);
            EventsManager.BindEvent(EventsManager.EventType.PlacedOwnObject, ResetActionTimer);
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

        void ResetActionTimer()
        {
            secondsSinceAction = 0f;
        }
        void ParamaterResetActionTimer(EventsManager.EventParams eventParams)
        {
            ResetActionTimer();
        }
        bool PlayerNeedsToTend(Player.PlayerEnum player)
        {
            bool tendingRequired = false;
            foreach (Plant plant in GameManager.Instance.SlotManagers[player].GetAllPlants())
            {
                if (plant.PlantGrowth.TendingState.ReadyToProgressStage == false)
                {
                    //Plants.PlantActions.TendingActions[] actions = Enum.GetValues(typeof(Plants.PlantActions.TendingActions));
                    //foreach (var action in (Plants.PlantActions.TendingActions)System.Enum.GetValues(typeof(Plants.PlantActions.TendingActions)))
                    foreach (var action in Helper.Utility.GetEnumValues<Plants.PlantActions.TendingActions>())
                    {
                        if (plant.PlantGrowth.TendingState.CanTend(action) == true)
                        {
                            tendingRequired = true;
                        }
                    }
                    
                }
            }
            return tendingRequired;
        }
        void CheckAndNudgePlayer()
        {
            bool tendingRequired = PlayerNeedsToTend(Player.PlayerEnum.Player1);
            if (GameManager.Instance.ActivePlayer.TurnPoints.GetPoints(TurnPoints.PointType.SelfObjectPlace) > 0)
            {
                nextAction = PlayerAction.PlacePlant;
            }
            else if(tendingRequired)
            {
                nextAction = PlayerAction.TendPlant;
            }
            else if (GameManager.Instance.ActivePlayer.TurnPoints.HasPointsLeft(TurnPoints.PointType.OtherObjectPlace)&&(GameManager.Instance.HotSeatManager.TurnTracker.Turn > 4))
            {
                nextAction = PlayerAction.GiftPlant;
            }
            else
            {
                nextAction = PlayerAction.EndTurn;
            }
            if (myBox.gameObject.activeSelf == false)
            {
                NudgePlayer(nextAction);
            }
        }
        void NudgePlayer(PlayerAction action)
        {
            myBox.gameObject.SetActive(true);
            switch (action)
            {
                case PlayerAction.PlacePlant:
                    myBox.Say("Remember you can still open the seed bag to choose a plant to plant!");
                    myArrows.EnableArrow(ArrowScript.ArrowPurpose.SeedBox);
                    break;
                case PlayerAction.TendPlant:
                    myBox.Say("Your plants still some attention!");
                    myBox.Say("You can drag items from the toolbox to attend to their needs!");
                    myArrows.EnableArrow(ArrowScript.ArrowPurpose.ToolBox);
                    break;
                case PlayerAction.GiftPlant:
                    myBox.Say("You can still give your partner a plant before you go!");
                    myArrows.EnableArrow(ArrowScript.ArrowPurpose.SwapGarden);
                    break;
                case PlayerAction.EndTurn:
                    myBox.Say("That's all we can do right now until your partner gets back!");
                    myBox.Say("Feel free to hang around though!");
                    break;
                default:
                    break;
            }
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
                    myBox.Say("We can have this garden communicate that feeling through the plants you choose!");
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

  
            myBox.Say("When you're ready, you can choose which seed to plant!");
            myArrows.EnableArrow(ArrowScript.ArrowPurpose.SeedBox);
        }

        private string GetEmotionOutput(string emotion) => $"<b>{emotion}</b>";

        void StartedThirdTurn() {
            myBox.Say("Hi there! did you know you and your partner can help each other out?");
            myBox.Say("If you go to their garden, you can plant a plant just for them!");
            myBox.Say("Remember that they're trying to communicate an emotion too!");
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
            myBox.Say($"That last plant was super in sync with you...");
            ExplainTraits();
            
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
 //           myBox.Say($"Did you know that <b>emotions</b> are comprised of <b>traits</b>?");
//            myBox.Say($"For example, when we started, you said you were {emotion}");
            myBox.Say($"\'{emotion}\' consists of two traits. <i>{traits.Item1}</i>{TraitValue.GetIconDisplay(traits.Item1)} and <i>{traits.Item2}</i>{TraitValue.GetIconDisplay(traits.Item2)}");
            myArrows.EnableArrow(ArrowScript.ArrowPurpose.MoodIndicator);
            myBox.Say($"A fully grown plant will help your garden communicate its trait!");
            //myBox.Say($"With a little help from your partner, your garden will be communicating {emotion} in no time!");
          
        }


        void PlantGrows()
        {
            myBox.Say("Looks like that plant really needed that!");
            myBox.Say("By the start of your next turn it will probably have grown!");
            //myBox.Say("Plants will only grow if they have been well looked after, so make sure to keep an eye out for thier needs.");
            //myBox.Say($"You can use the humane repellant to chase away any bugs and use the shears to trim extra leaves too!");
        }

        void MoodRelevantPlantReachesMaturity()
        {
            myBox.Say("Now this garden is really getting going!");
            myArrows.EnableArrow(ArrowScript.ArrowPurpose.MoodIndicator);
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