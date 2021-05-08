using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helper
{
    public static class Utility 
    {
        /// <summary>
        /// Get an enumerable list of all enum values of a particular type
        /// </summary>
        public static T[] GetEnumValues<T>() => (T[])System.Enum.GetValues(typeof(T));
    }
}