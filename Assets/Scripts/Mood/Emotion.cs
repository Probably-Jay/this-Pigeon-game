using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;


namespace Mood
{

    /// <summary>
    /// Struct representing an emotion. Effectively a constrained wrapper for a <see cref="Mood.TraitValue"/>
    /// </summary>
    [System.Serializable]
    public struct Emotion
    {
        public enum Emotions
        {
            Loving
            , Excited
            , Stressed
            , Lonley
        }

        public static readonly ReadOnlyDictionary<Emotions, TraitValue> EmotionValues = new ReadOnlyDictionary<Emotions, TraitValue>
        (
            new Dictionary<Emotions, TraitValue> 
            { 
                  { Emotions.Loving,    new TraitValue(social: 1, joyful: 1, energetic: 0, painful: 0) } 
                , { Emotions.Excited,   new TraitValue(social: 0, joyful: 1, energetic: 1, painful: 0) }
                , { Emotions.Stressed,  new TraitValue(social: 0, joyful: 0, energetic: 1, painful: 1) }
                , { Emotions.Excited,   new TraitValue(social: 1, joyful: 0, energetic: 0, painful: 1) }
            }
        );

        public readonly TraitValue traits;

        public Emotion(Emotions emotion) => traits = EmotionValues[emotion];

    }
}