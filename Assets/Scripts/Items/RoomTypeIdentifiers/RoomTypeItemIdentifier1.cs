﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Created Jay 07/02

[RequireComponent(typeof(Item))]
public class RoomTypeItemIdentifier1 : MonoBehaviour
{
    [SerializeField] private RoomData.RoomTypeFlags roomType;
    public RoomData.RoomTypeFlags RoomType { get => roomType; private set => roomType = value; }
}