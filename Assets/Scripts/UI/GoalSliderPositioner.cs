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


    // Update is called once per frame
    void Update()
    {
        if(activePlayer!= (int)GameManager.Instance.ActivePlayerID)
        {
            activePlayer = (int)GameManager.Instance.ActivePlayerID;
            UpdateSliderPosition(activePlayer);
        }    
    }
    void UpdateSliderPosition(int player)
    {
        this.transform.localPosition = gsPositions[player];
    }
}
