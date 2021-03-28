using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// careted zap and jay 28/03

namespace Tutorial
{

    public class CatChatter : MonoBehaviour
    {
        public TextBox myBox;
        // Start is called before the first frame update
        void Start()
        {
            //myBox.gameObject.SetActive(true);
            //myBox.Say("hi");
            //myBox.Say("hello");
            //myBox.Say("nya");
            //myBox.Say("is this annoying yet");

        }

 

        delegate bool Condition();

        private void OnEnable()
        {
            BindEvent(EventsManager.EventType.StartGame, StartTurnOne);
        }


        private void BindEvent(EventsManager.EventType eventType, System.Action tutorialToCall, System.Func<bool> condition = null)
        {
            void func() => LaunchTutorial(func, eventType, tutorialToCall, condition);
            EventsManager.BindEvent(eventType, func);
        }

        void LaunchTutorial(System.Action func, EventsManager.EventType eventType, System.Action tutorial, System.Func<bool> condition)
        {
            if (condition == null || condition())
            {
                tutorial();
                EventsManager.UnbindEvent(eventType, func);
            }
        }

        //these functions could be modified to include if statements or just bound directly to the 
        void StartTurnOne()
        {
            myBox.gameObject.SetActive(true);
            myBox.Say("Hello there! Welcome to your garden!");
            myBox.Say("This place could do with some flora, don't you think?");
            myBox.Say("When you're ready, you can choose a plant to plant by tapping this seed basket!");
        }
        void PlantedFirstPlant()
        {
            myBox.gameObject.SetActive(true);
            myBox.Say("Wow, this place is looking beautiful already!");
            myBox.Say("Don't forget to water it by using the can in the toolbox!");
        }
        void PlantedFirstMoodRelevantPlant()
        {
            myBox.Say("That plant is so in sync with you!");
            myBox.Say("Well done! A few more of those are just what this place needs!");
        }
        void StartTurnTwoOutOfSync()//this would be called when turn two starts but the player hasn't planted a mood relevant plant
        {
            myBox.gameObject.SetActive(true);
            myBox.Say("Good morning!");
            myBox.Say("Today, why not try planting a plant with a trait from your mood?");
        }
        void MoodRelevantPlantReachesMaturity()
        {
            myBox.gameObject.SetActive(true);
            myBox.Say("Now this garden is really getting going!");
        }
    }
}