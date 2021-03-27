using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

// added Jay 26/03


namespace Mood
{

    /// <summary>
    /// Struct representing an emotion. Effectively a constrained wrapper for a <see cref="Mood.TraitValue"/>
    /// </summary>
    [System.Serializable]
    public struct Emotion
    {
        /// <summary>
        /// Enum of emotions
        /// </summary>
        public enum Emotions
        {
            Loving
            , Excited
            , Stressed
            , Lonley
        }

        /// <summary>
        /// Read only collection of <see cref="TraitValue"/>s that represent each emotion
        /// </summary>
        public static readonly ReadOnlyDictionary<Emotions, TraitValue> EmotionValues = new ReadOnlyDictionary<Emotions, TraitValue>
        (
            new Dictionary<Emotions, TraitValue> 
            { 
                  { Emotions.Loving,    new TraitValue(social: 1, joyful: 1, energetic: 0, painful: 0) } 
                , { Emotions.Excited,   new TraitValue(social: 0, joyful: 1, energetic: 1, painful: 0) }
                , { Emotions.Stressed,  new TraitValue(social: 0, joyful: 0, energetic: 1, painful: 1) }
                , { Emotions.Lonley,    new TraitValue(social: 1, joyful: 0, energetic: 0, painful: 1) }
            }
        );

        /// <summary>
        /// The traits of this emotion
        /// </summary>
        public readonly TraitValue traits;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="emotion">The emotion being created</param>
        public Emotion(Emotions emotion) => traits = EmotionValues[emotion];

    }
}