using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Created Jay 07/02

[RequireComponent(typeof(Item))]
public class RoomTypeItemIdentifier : MonoBehaviour
{
    [SerializeField] private RoomData.RoomType roomType;
    public RoomData.RoomType RoomType { get => roomType; private set => roomType = value; }
}
