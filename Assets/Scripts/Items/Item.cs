using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Created Jay 05/02
/// <summary>
/// Script all items will have
/// </summary>
/// 
/// Edited Scott 17/02
/// 
public class Item : MonoBehaviour
{
    [SerializeField] public string objectName;
    //[SerializeField,Range(0,10)] int reallyCoolData;

    [SerializeField, Range(-1, 1)] public int moodEnergy = 0;
    [SerializeField, Range(-1, 1)] public int moodPride  = 0;
    [SerializeField, Range(-1, 1)] public int moodSocial = 0;
}
