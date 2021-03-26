using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mood
{
    /// <summary>
    /// Struct representing a collection of triat values. Immutable.
    /// </summary>
    [System.Serializable]
    public struct TraitValue
    {
        /// <summary>
        /// The number of trait scales. Important to be const for serialisation
        /// </summary>
        public const int NumberOfAtributeScales = 4;
        /// <summary>
        /// Enum representing each trait scale
        /// </summary>
        public enum Scales
        {
            Social
            , Joyful
            , Energetic
            , Painful
        }


        /// <summary>
        /// An unambiguous way to store the traits: <see cref="Scales.Social"/>, <see cref="Scales.Joyful"/>, <see cref="Scales.Energetic"/>, and <see cref="Scales.Painful"/>
        /// </summary>
        /// <param name="social">The value of <see cref="Scales.Social"/></param>
        /// <param name="joyful">The value of <see cref="Scales.Joyful"/></param>
        /// <param name="energetic">The value of <see cref="Scales.Energetic"/></param>
        /// <param name="painful">The value of <see cref="Scales.Painful"/></param>
        public TraitValue(int social, int joyful, int energetic, int painful)
        {
            this.social     = social;
            this.joyful     = joyful;
            this.energetic  = energetic;
            this.painful    = painful;
        }

        // private int[] values;

        // not an array to allow this to be a struct (no refs)
        private readonly int social;
        private readonly int joyful;
        private readonly int energetic;
        private readonly int painful;


        /// <summary>
        /// The value on the scale of <see cref="Scales.Social"/>
        /// </summary>
        public int Social { get => this[Scales.Social];}
        /// <summary>
        /// The value on the scale of <see cref="Scales.Joyful"/>
        /// </summary>
        public int Joyful { get => this[Scales.Joyful];}
        /// <summary>
        /// The value on the scale of <see cref="Scales.Energetic"/>
        /// </summary>
        public int Energetic { get => this[Scales.Energetic];}
        /// <summary>
        /// The value on the scale of <see cref="Scales.Painful"/>
        /// </summary>
        public int Painful { get => this[Scales.Painful];}

        /// <summary>
        /// The correct way to index this struct
        /// </summary>
        /// <param name="index">The <see cref="Scales"/> name of the property you wish to access</param>
        public int this[Scales index]
        {
            get
            {
                switch (index)
                {
                    case Scales.Social: return social;
           
                    case Scales.Joyful: return joyful;
           
                    case Scales.Energetic: return energetic;
  
                    case Scales.Painful:  return painful;

                    default: throw new System.ArgumentException();
                }
            }
        }

        /// <summary>
        /// Provided for internal ease of life, however prefer the explicit properties, or indexing with <see cref="this[Scales]"/>
        /// </summary>
        /// <param name="index">The index of the value you wish to access</param>
        private int this[int index]
        {
            get { return this[(Scales)index]; }
        }

        #region Operators
        public static TraitValue operator -(TraitValue a) => new TraitValue(-a[0], -a[1], -a[2], -a[3]);

        public static TraitValue operator +(TraitValue a, TraitValue b) => new TraitValue(a[0] + b[0], a[1] + b[1], a[2] + b[2], a[3] + b[3]);

        public static TraitValue operator -(TraitValue a, TraitValue b) => a + (-b);


        public static bool operator ==(TraitValue a, TraitValue b) => a.Equals(b);
        public static bool operator !=(TraitValue a, TraitValue b) => !(a == b);


        /// <summary>
        /// Requirement of implimenting <see cref="=="/> equality
        /// </summary>
        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                TraitValue o = (TraitValue)obj;
                return this[0] == o[0] && this[1] == o[1] && this[2] == o[2] && this[3] == o[3];
            }
        }

        /// <summary>
        /// Requirement of implimenting <see cref="=="/> equality
        /// </summary>
        public override int GetHashCode() => (new int[] {this[0],this[1],this[2], this[3]}).GetHashCode();

        #endregion
    }
}