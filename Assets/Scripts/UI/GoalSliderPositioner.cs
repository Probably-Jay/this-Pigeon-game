using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalSliderPositioner : MonoBehaviour
{
    public Vector3[] gsPositions;
    public Vector3 playerOnePos;
    public Vector3 playerTwoPos;
    private int activePlayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(activePlayer!= (int)GameManager.Instance.ActivePlayer.PlayerEnumValue)
        {
            activePlayer = (int)GameManager.Instance.ActivePlayer.PlayerEnumValue;
            UpdateSliderPosition(activePlayer);
        }    
    }
    void UpdateSliderPosition(int player)
    {
        this.transform.localPosition = gsPositions[player];
    }
}
