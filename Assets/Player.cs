using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// created jay 12/02

public class Player : MonoBehaviour
{

    public enum PlayerEnum { Player0 = 0, Player1 = 1 };
    public TurnPoints turnPoints;

    // Start is called before the first frame update
    void Start()
    {
        turnPoints = GetComponent<TurnPoints>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
