using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// created jay 12/02
/// <summary>
/// Class holding all of the data about a player
/// </summary>
[RequireComponent(typeof(TurnPoints))]
public class Player : MonoBehaviour
{

    public enum PlayerEnum { Player0 = 0, Player1 = 1 };

    public TurnPoints TurnPoints { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        TurnPoints = GetComponent<TurnPoints>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
