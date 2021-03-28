using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helper
{
    public static class Utility 
    {
        public static T[] GetEnumValues<T>() => (T[])System.Enum.GetValues(typeof(T));
    }
}