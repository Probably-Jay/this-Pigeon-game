using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Created Jay 05/02
/// <summary>
/// Script all items will have
/// </summary>
public class Item : MonoBehaviour
{
    [SerializeField] string objectName;
    [SerializeField,Range(0,10)] int reallyCoolData;
}
