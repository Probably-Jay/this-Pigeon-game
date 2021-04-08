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
            , Lonely
        }

        public static (TraitValue.Scales,TraitValue.Scales) GetScalesInEmotion(Emotions emotion)
        {
            switch (emotion)
            {
                case Emotions.Loving:
                    return (TraitValue.Scales.Social, TraitValue.Scales.Joyful);
                case Emotions.Excited:
                    return (TraitValue.Scales.Joyful, TraitValue.Scales.Energetic);
                case Emotions.Stressed:
                    return (TraitValue.Scales.Energetic, TraitValue.Scales.Painful);
                case Emotions.Lonely:
                    return (TraitValue.Scales.Social, TraitValue.Scales.Painful);
                default: throw new System.ArgumentException();

            }
        }

        /// <summary>
        /// Read only collection of <see cref="TraitValue"/>s that represent each emotion
        /// </summary>
        public static readonly ReadOnlyDictionary<Emotions, TraitValue> EmotionValues = new ReadOnlyDictionary<Emotions, TraitValue>
        (
            new Dictionary<Emotions, TraitValue> 
            { 
                  { Emotions.Loving,    new TraitValue(social: 3, joyful: 3, energetic: 0, painful: 0) } 
                , { Emotions.Excited,   new TraitValue(social: 0, joyful: 3, energetic: 3, painful: 0) }
                , { Emotions.Stressed,  new TraitValue(social: 0, joyful: 0, energetic: 3, painful: 3) }
                , { Emotions.Lonely,    new TraitValue(social: 3, joyful: 0, energetic: 0, painful: 3) }
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

        /// <summary>
        /// If this emotion contains this <paramref name="scale"/> at a value above <c>0</c>
        /// </summary>
        public bool Contains(TraitValue.Scales scale) => traits.Contains(scale);

        /// <summary>
        /// If this emotion containts any of the same trais as <paramref name="traitValue"/>
        /// </summary>
        public bool Overlaps(TraitValue traitValue)
        {
            foreach (var trait in Helper.Utility.GetEnumValues<TraitValue.Scales>())
            {
                if(Contains(trait) && traitValue.Contains(trait))
                {
                    return true;
                }
            }
            return false;
        }

    }
}