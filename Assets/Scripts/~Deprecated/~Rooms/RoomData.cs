using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created Jay 07/02

[System.Obsolete("Obsolete",true)]
public class RoomData : MonoBehaviour
{
    public enum RoomType
    {
        None = 0
        , A 
        , B
        , C
        , D
        , E
    }
    
    [System.Flags]
    public enum RoomTypeFlags 
    {
        None = 0
        , A  = 1 << 0
        , B  = 1 << 1
        , C  = 1 << 2
        , D  = 1 << 3
        , E  = 1 << 4
    }
}
