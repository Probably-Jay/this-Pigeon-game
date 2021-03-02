using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnButtonScript : MonoBehaviour
{
    public void CallEndTurn() => GameManager.Instance.EndTurn();
}
