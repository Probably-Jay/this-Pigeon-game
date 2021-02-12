using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// created jay 12/02

/// <summary>
/// <see cref="Singleton{}"/> class to allow for easy referancing of important objects 
/// </summary>
[RequireComponent(typeof(HotSeatManager))]
public class GameManager : Singleton<GameManager>
{
    public new static GameManager Instance { get => Singleton<GameManager>.Instance; }
    public override void Awake()
    {
        base.InitSingleton();
        hotSeatManager = GetComponent<HotSeatManager>();
    }

    public HotSeatManager hotSeatManager;
    
    


}
