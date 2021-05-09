using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mood;
using System;
using GameCore;
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
        private TextBox MyBox => myBox;

        public int nudgeTime = 20;

        private static Emotion.Emotions GoalEmotion() => GameManager.Instance.EmotionTracker.EmotionGoal.enumValue;


        string HasEverPlantedMoodRelaventPlantKey => GameKey + "_variable_"+ "HasEverPlantedMoodRelaventPlantKey";

        List<(EventsManager.EventType, Action)> functionBinds = new List<(EventsManager.EventType, Action)>();
        List<(EventsManager.ParameterEventType, Action<EventsManager.EventParams>)> paramFunctionBinds = new List<(EventsManager.ParameterEventType, Action<EventsManager.EventParams>)>();

        string GameKey => NetSystem.NetworkHandler.Instance.NetGame.CurrentNetworkGame.GroupEntityKey.Id + NetSystem.NetworkHandler.Instance.ClientEntity.Id;

        delegate bool Condition();

        ArrowEnabler myArrows;

       // TextAsset asset;


        [SerializeField] SeedBag seedBag;
        [SerializeField] ToolBox toolBox;

        bool SeedBagOpen => seedBag.gameObject.activeSelf;
        bool ToolBoxOpen => toolBox.Open;


        enum PlayerAction
        {
               PlacePlant
            ,  TendPlant
            ,  GiftPlant
            ,  EndTurn
            ,  WaitForYourTurn
        }

        private PlayerAction nextAction;

        float secondsSinceAction = 0f;

        private void Update()
        {
            
            if (secondsSinceAction > nudgeTime)
            {
                secondsSinceAction = 0f;
                CheckAndNudgePlayer();
            }
            else
            {
                if (GameManager.Instance.Playing)
                {
                    secondsSinceAction += Time.deltaTime;

                }
                else
                {
                    secondsSinceAction += Time.deltaTime*0.3f;

                }
            }
        }

        private void Awake()
        {
            myArrows = GetComponent<ArrowEnabler>();
            MyBox.gameObject.SetActive(false);
        }




        private void OnEnable()
        {
            EventsManager.CleanEvents(gameObject);

            int turn = GameManager.Instance.OnlineTurnManager.TurnTracker.Turn;
            int localTurn = (int)Math.Floor(turn / 2f) + 1;

            EventsManager.BindEvent(EventsManager.ParameterEventType.SwappedGardenView, ParamaterResetActionTimer);
            EventsManager.BindEvent(EventsManager.ParameterEventType.OnPerformedTendingAction, ParamaterResetActionTimer);
            EventsManager.BindEvent(EventsManager.EventType.PlacedOwnObject, ResetActionTimer);
            EventsManager.BindEvent(EventsManager.EventType.PlacedCompanionObject, ResetActionTimer);

           

            BindEvent(EventsManager.EventType.EnterPlayingState, StartTurnOne, WhenToDisplay.YouArePlaying,
                condition: ()=> { return localTurn == 1; });


            BindEvent(EventsManager.EventType.EnterPlayingState, StartedThirdTurn, WhenToDisplay.YouArePlaying,
                condition:()=> { return localTurn == 3; });        
            

            BindEvent(EventsManager.EventType.JustResumedGame, EnterGameSpectating, WhenToDisplay.YouAreSpectating);


            BindEvent(EventsManager.EventType.PlacedOwnObject, PlantedFirstPlant, WhenToDisplay.YouArePlayingAndLookingAtOwnGarden);

            BindEvent(EventsManager.EventType.PlantReadyToGrow, PlantFullyTended, WhenToDisplay.YouArePlayingAndLookingAtOwnGarden);

            BindEvent(EventsManager.EventType.PlacedOwnObjectMoodRelevant, SayNothing, WhenToDisplay.YouArePlayingAndLookingAtOwnGarden,
                      sideEffects: () => PlayerPrefs.SetInt(HasEverPlantedMoodRelaventPlantKey, 1)) ;

            BindEvent(EventsManager.EventType.EnterPlayingState, StartTurnTwoWithRelaventPlants, WhenToDisplay.YouArePlaying,
                      condition: () =>
                      {
                          return localTurn == 2 && PlayerPrefs.GetInt(HasEverPlantedMoodRelaventPlantKey) == 1;
                      }); 
            
            BindEvent(EventsManager.EventType.EnterPlayingState, StartTurnTwoWithNoRelaventPlants, WhenToDisplay.YouArePlaying,
                      condition: () =>
                      {
                          return localTurn == 2 && PlayerPrefs.GetInt(HasEverPlantedMoodRelaventPlantKey) == default;
                      });


            BindEvent(EventsManager.EventType.EnterPlayingState, MoodRelevantPlantReachesMaturity, WhenToDisplay.YouArePlaying,
                condition: () => 
                {
                    TraitValue gardenCurrentTrait = GameManager.Instance.EmotionTracker.CurrentGardenTraits;
                    TraitValue gardenGoalTrait = GameManager.Instance.EmotionTracker.EmotionGoal.traits;

                    return gardenCurrentTrait.Overlaps(gardenGoalTrait);

                    //float defaultDistance = TraitValue.Distance(TraitValue.Zero, gardenGoalTrait);
                    //float currentDistance = TraitValue.Distance(gardenCurrentTrait, gardenGoalTrait);


                }); 

            BindEvent(EventsManager.ParameterEventType.NotEnoughPointsForAction, NoMorePoints, WhenToDisplay.Always, dontUnbind: true);


            BindEvent(EventsManager.ParameterEventType.AcheivedGoal, AcheivedGoal, WhenToDisplay.Always);


   

            //EventsManager.InvokeEvent(EventsManager.ParameterEventType.NotEnoughPointsForAction, new EventsManager.EventParams() { EnumData = TurnPoints.PointType.SelfObjectPlace });

           // BindEvent(EventsManager.EventType.PlantReadyToGrow,)
        }

        private void OnDisable()
        {
            if (EventsManager.InstanceExists)
            {

                EventsManager.UnbindEvent(EventsManager.ParameterEventType.SwappedGardenView, ParamaterResetActionTimer);
                EventsManager.UnbindEvent(EventsManager.ParameterEventType.OnPerformedTendingAction, ParamaterResetActionTimer);
                EventsManager.UnbindEvent(EventsManager.EventType.PlacedOwnObject, ResetActionTimer);
                EventsManager.UnbindEvent(EventsManager.EventType.PlacedCompanionObject, ResetActionTimer);

                UnbindEvents();

                EventsManager.CleanEvents(this);
            }

        }

        private void UnbindEvents()
        {
            foreach (var evnt in functionBinds)
            {
                EventsManager.UnbindEvent(evnt.Item1, evnt.Item2);
            }
            foreach (var evnt in paramFunctionBinds)
            {
                EventsManager.UnbindEvent(evnt.Item1, evnt.Item2);
            } 
        }

        private void EnterGameSpectating()
        {
            MyBox.Say("Your partner is taking thier turn, I'm sure they'll be done soon.");
            MyBox.Say("Stick around if you like, or go check another garden and come back later!");
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
            foreach (Plant plant in GameManager.Instance.LocalPlayerSlotManager.GetAllPlants())
            {
                if (plant.PlantGrowth.TendingState.ReadyToProgressStage == false)
                {
                    //Plants.PlantActions.TendingActions[] actions = Enum.GetValues(typeof(Plants.PlantActions.TendingActions));
                    //foreach (var action in (Plants.PlantActions.TendingActions)System.Enum.GetValues(typeof(Plants.PlantActions.TendingActions)))
                    foreach (var action in Helper.Utility.GetEnumValues<Plants.PlantActions.TendingActions>())
                    {
                        if (plant.PlantGrowth.TendingState.RequiresAction(action) == true)
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

            if (GameManager.Instance.LocalPlayer.TurnPoints.GetPoints(TurnPoints.PointType.SelfObjectPlace) > 0)
            {
                nextAction = PlayerAction.PlacePlant;
            }
            else if (tendingRequired)
            {
                nextAction = PlayerAction.TendPlant;
            }
            else if (GameManager.Instance.LocalPlayer.TurnPoints.HasPointsLeft(TurnPoints.PointType.OtherObjectPlace) && (GameManager.Instance.OnlineTurnManager.TurnTracker.Turn > 4))
            {
                nextAction = PlayerAction.GiftPlant;
            }
            else
            {
                nextAction = PlayerAction.EndTurn;
            }

            if (GameManager.Instance.Spectating)
            {
                nextAction = PlayerAction.EndTurn;
            }

            if (MyBox.gameObject.activeSelf != false)
            {
                return;
            }

            if(ToolBoxOpen || SeedBagOpen)
            {
                return;
            }

            NudgePlayer(nextAction);


        }
        void NudgePlayer(PlayerAction action)
        {


            MyBox.gameObject.SetActive(true);
            switch (action)
            {
                case PlayerAction.PlacePlant:
                    MyBox.Say("Remember you can still open the seed bag to choose a plant to plant!");
                    myArrows.EnableArrow(ArrowScript.ArrowPurpose.SeedBox);
                    break;
                case PlayerAction.TendPlant:
                    MyBox.Say("Your plants still some attention!");
                    MyBox.Say("You can drag items from the toolbox to attend to their needs!");
                    myArrows.EnableArrow(ArrowScript.ArrowPurpose.ToolBox);
                    break;
                case PlayerAction.GiftPlant:
                    MyBox.Say("You can still give your partner a plant before you go!");
                    myArrows.EnableArrow(ArrowScript.ArrowPurpose.SwapGarden);
                    break;
                case PlayerAction.EndTurn:
                    MyBox.Say("That's all we can do right now until your partner gets back...");
                    MyBox.Say("Be sure to hit that end turn button so they can play!");
                    break;
                case PlayerAction.WaitForYourTurn:
                    MyBox.Say("Your partner is taking thier turn, I'm sure they'll be done soon.");
                    MyBox.Say("Stick around if you like, or go check another garden and come back later!");
                    break;
                default:
                    break;
            }
        }

        #region BindingAndUnbinding

        enum WhenToDisplay
        {
            Always,
            YouArePlaying,
            YouArePlayingAndLookingAtOwnGarden,
            YouArePlayingAndLookingAtCompanionsGarden,
            YouAreSpectating,
            

        }

        /// <summary>
        /// Binds a function event to then be unbound after it is invoked
        /// </summary>
        /// <param name="eventType">The event that will trigger the passed function</param>
        /// <param name="tutorialToCall">The funcion to be called</param>
        /// <param name="sideEffects">Any side effects</param>
        /// <param name="condition">Any other conditions to the funciton being called</param>
        /// <param name="unbindIfFailCondition">If the function should be unbound if the condition fails</param>
        void BindEvent(EventsManager.EventType eventType, System.Action tutorialToCall, WhenToDisplay whenToDisplay, System.Action sideEffects = null, System.Func<bool> condition = null, bool dontUnbind = false, bool unbindIfFailCondition = false)
        {
            void func() => LaunchTutorial(func, eventType, tutorialToCall, whenToDisplay, sideEffects, condition, dontUnbind, unbindIfFailCondition);
            functionBinds.Add((eventType,func));
            EventsManager.BindEvent(eventType, func);
        }   
        /// <summary>
        /// Overload for paramatised function
        /// </summary>
        void BindEvent(EventsManager.ParameterEventType eventType, System.Action<EventsManager.EventParams> tutorialToCall, WhenToDisplay whenToDisplay, System.Action sideEffects = null, System.Func<bool> condition = null, bool dontUnbind = false, bool unbindIfFailCondition = false)
        {
            void paramFunc(EventsManager.EventParams eventParams) => LaunchTutorialParamatised(paramFunc, eventType, tutorialToCall, eventParams, whenToDisplay, sideEffects, condition, dontUnbind, unbindIfFailCondition);
            paramFunctionBinds.Add((eventType, paramFunc));
            EventsManager.BindEvent(eventType, paramFunc);
        }

        void LaunchTutorial(System.Action func, EventsManager.EventType eventType, System.Action tutorial, WhenToDisplay whenToDisplay, System.Action sideEffects, System.Func<bool> condition, bool dontUnbind, bool unbindIfFailCondition)
        {
            if(PlayerPrefs.GetInt(GetKey(tutorial)) == 1)
            {
                return;
            }

            if (!ValidateShouldDisplay(whenToDisplay))
            {
                return;
            }


            if (condition == null || condition())
            {
                if(MyBox == null || MyBox.gameObject == null || MyBox.Equals(null) || MyBox.gameObject.Equals(null))
                {
                    Debug.LogWarning($"Cleaning up old event {tutorial.Method.Name}");
                    ExhastTutorial(func, eventType, tutorial);
                    return;
                }

                MyBox.gameObject.SetActive(true);

                tutorial();

                sideEffects?.Invoke();
                if(!dontUnbind)
                    ExhastTutorial(func, eventType, tutorial);

            }
            else if (!dontUnbind &&(condition != null && unbindIfFailCondition))
            {
                ExhastTutorial(func, eventType, tutorial);
            }
        }

        private void ExhastTutorial(Action func, EventsManager.EventType eventType, Action tutorial)
        {
            EventsManager.UnbindEvent(eventType, func);
            functionBinds.Remove((eventType, func));
            string key = GetKey(tutorial);

            PlayerPrefs.SetInt(key, 1);
        }
        private void ExhastTutorial(Action<EventsManager.EventParams> func, EventsManager.ParameterEventType eventType, Action<EventsManager.EventParams> tutorial)
        {
            EventsManager.UnbindEvent(eventType, func);
            paramFunctionBinds.Remove((eventType, func));

            string key = GetKey(tutorial);

            PlayerPrefs.SetInt(key, 1);
        }

        private string GetKey(Action<EventsManager.EventParams> tutorial)
        {
            return GameKey + tutorial.Method.Name;
        }        
        private string GetKey(Action tutorial)
        {
            return GameKey + tutorial.Method.Name;
        }

        void LaunchTutorialParamatised(System.Action<EventsManager.EventParams> func, EventsManager.ParameterEventType eventType, System.Action<EventsManager.EventParams> tutorial, EventsManager.EventParams eventParams, WhenToDisplay whenToDisplay, System.Action sideEffects, System.Func<bool> condition, bool dontUnbind, bool unbindIfFailCondition)
        {
            if (PlayerPrefs.GetInt(GetKey(tutorial)) == 1)
            {
                return;
            }

            if (!ValidateShouldDisplay(whenToDisplay))
            {
                return;
            }


            if (condition == null || condition())
            {
                if (MyBox == null || MyBox.gameObject == null || MyBox.Equals(null) || MyBox.gameObject.Equals(null))
                {
                    Debug.LogWarning($"Cleaning up old event {tutorial}");
                    ExhastTutorial(func, eventType, tutorial);
                    return;
                }

                MyBox.gameObject.SetActive(true);

                tutorial(eventParams);

                sideEffects?.Invoke();

                ExhastTutorial(func, eventType, tutorial);

            }
            else if (condition != null && unbindIfFailCondition)
            {
                ExhastTutorial(func, eventType, tutorial);

            }
        }

        private bool ValidateShouldDisplay(WhenToDisplay whenToDisplay)
        {
            switch (whenToDisplay)
            {
                case WhenToDisplay.Always:
                    return true;
                case WhenToDisplay.YouArePlaying:
                    return GameManager.Instance.Playing;
                case WhenToDisplay.YouArePlayingAndLookingAtOwnGarden:
                    return GameManager.Instance.Playing && (GameManager.Instance.PlayerWhosGardenIsCurrentlyVisible == GameManager.Instance.LocalPlayerEnumID);
                case WhenToDisplay.YouArePlayingAndLookingAtCompanionsGarden:
                    return GameManager.Instance.Playing && (GameManager.Instance.PlayerWhosGardenIsCurrentlyVisible != GameManager.Instance.LocalPlayerEnumID);
                case WhenToDisplay.YouAreSpectating:
                    return GameManager.Instance.Spectating;
                default: throw new Exception();
            }
        }


        #endregion



        void SayNothing() { }


        void StartTurnOne()
        {

            MyBox.Say("Hello there! Welcome to your garden!");
         

            string emotion = GoalEmotion().ToString();
            
            switch (emotion)
            {
                case "Loving":
                    MyBox.Say($"You're feeling {GetEmotionOutput(emotion)}? That's great!");
                    MyBox.Say($"Let's share that love! And as with all good things, we can say it with flowers!");
                    break;
                case "Lonely":
                    MyBox.Say($"I'm sorry to hear that you're feeling {GetEmotionOutput(emotion)}");
                    MyBox.Say($"Why not share that with your partner, too?");
                    MyBox.Say("We can have this garden communicate that feeling through the plants you choose!");
                    break;
                case "Stressed":
                    MyBox.Say($"You're {GetEmotionOutput(emotion)}? Then you've come to the right place!");
                    MyBox.Say($"Let's unwind by planting growing some plants that express that feeling!");
                    break;
                case "Excited":
                    MyBox.Say($"I'm {GetEmotionOutput(emotion.ToString())} too! Let's get going then!");
                    MyBox.Say($"The goal of this garden is to communicate that feeling to your partner!");
                    MyBox.Say($"Hopefully they're as jazzed as us!");
                    break;
                default:
                    break;
            }

  
            MyBox.Say("When you're ready, you can choose which seed to plant!");
            myArrows.EnableArrow(ArrowScript.ArrowPurpose.SeedBox);
        }

        private string GetEmotionOutput(string emotion) => $"<b>{emotion}</b>";

        void StartedThirdTurn() {
            MyBox.Say("Hi there! did you know you and your partner can help each other out?");
            MyBox.Say("If you go to their garden, you can plant a plant just for them!");
            MyBox.Say("Remember that they're trying to communicate an emotion too!");
            myArrows.EnableArrow(ArrowScript.ArrowPurpose.SwapGarden);
        }


        void PlantedFirstPlant()
        {
            MyBox.Say("Wow, this place is looking beautiful already!");
            ExplainTending();
        }

        void StartTurnTwoWithRelaventPlants()
        {
            MyBox.Say("Good morning!");
            MyBox.Say($"That last plant was super in sync with you...");
            ExplainTraits();
            
            MyBox.Say("Why not plant another with a different trait from your mood?");

        }
        void StartTurnTwoWithNoRelaventPlants()
        {
            MyBox.Say("Good morning!");
            ExplainTraits();
            MyBox.Say("Why not try planting a plant with a trait from your mood?");
 
        }

        void ExplainTending()
        {
            MyBox.Say("Don't forget to water it by using the can in the toolbox!");
            EventsManager.BindEvent(EventsManager.EventType.ToolBoxOpen, ActivateCanArrow);
        }
        void ActivateCanArrow()
        {
            EventsManager.UnbindEvent(EventsManager.EventType.ToolBoxOpen, ActivateCanArrow);
            myArrows.EnableArrow(ArrowScript.ArrowPurpose.WateringCan);
        }

        void ExplainTraits()
        {
            EventsManager.InvokeEvent(EventsManager.EventType.MoodSlidersExplination);
            Emotion.Emotions emotion = GoalEmotion();
            var traits = Emotion.GetScalesInEmotion(emotion);
 //           MyBox.Say($"Did you know that <b>emotions</b> are comprised of <b>traits</b>?");
//            MyBox.Say($"For example, when we started, you said you were {emotion}");
            MyBox.Say($"\'{emotion}\' consists of two traits. <i>{traits.Item1}</i>{TraitValue.GetIconDisplay(traits.Item1)} and <i>{traits.Item2}</i>{TraitValue.GetIconDisplay(traits.Item2)}");
            myArrows.EnableArrow(ArrowScript.ArrowPurpose.MoodIndicator);
            MyBox.Say($"A fully grown plant will help your garden communicate its trait!");
            //MyBox.Say($"With a little help from your partner, your garden will be communicating {emotion} in no time!");
          
        }


        void PlantFullyTended()
        {
            MyBox.Say("Looks like that plant really needed that!");
            MyBox.Say("By the start of your next turn it will probably have grown!");
            //MyBox.Say("Plants will only grow if they have been well looked after, so make sure to keep an eye out for thier needs.");
            //MyBox.Say($"You can use the humane repellant to chase away any bugs and use the shears to trim extra leaves too!");
        }

        void MoodRelevantPlantReachesMaturity()
        {
            MyBox.Say("Now this garden is really getting going!");
            myArrows.EnableArrow(ArrowScript.ArrowPurpose.MoodIndicator);
        }

        void NoMorePoints(EventsManager.EventParams eventParams)
        {
            switch ((TurnPoints.PointType)eventParams.EnumData1)
            {
                case TurnPoints.PointType.SelfObjectPlace:
                    MyBox.Say("Whoa there! You can only plant one plant in your garden each turn!");  
                    break;
                case TurnPoints.PointType.OtherObjectPlace:
                    MyBox.Say("You can't plant in your companion's garden again until your next turn, sorry.");
                    break;
                default: throw new System.ArgumentException();
            }
        }

        private void AcheivedGoal(EventsManager.EventParams eventParams)
        {
            if ((Player.PlayerEnum)eventParams.EnumData1 != player1)
                return;

            MyBox.Say("You did it! Your garden now reflects your mood as well as you communicated it.");


            var goal = GameManager.Instance.EmotionTracker.EmotionGoal.enumValue;

            switch (goal)
            {
                case Emotion.Emotions.Loving:
                       MyBox.Say($"Do you feel the love in the air? Because I do!");
                    break;
                case Emotion.Emotions.Excited:
                       MyBox.Say($"Wow! I can barely stay in one place. Thank you for sharing all your positive vibes with me!");
                    break;
                case Emotion.Emotions.Stressed:
                       MyBox.Say($"Finding some time for yourself is a very hard thing to do while stressed. You did well, playing game is a form of self-care!");
                    break;
                case Emotion.Emotions.Lonely:
                       MyBox.Say($"Thank you for sharing your loneliness with me and your gardening partner. Opening up about your feelings is so hard and I'm proud of you.");
                    break;
            }

            MyBox.Say("You can carry on playing the game in an endless mode if you like");

        }

    }
}