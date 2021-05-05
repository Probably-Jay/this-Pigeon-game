using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public List<GameObject> MoodButtons;

    public void ChangeMoodSelection(int buttonID)
    {
        for (int i = 0; i < MoodButtons.Count; i++)
        {
            if (i != buttonID)
            {
                MoodButtons[i].GetComponent<MoodSelectionButtons>().resetColour();
            }
        }
    }
}
