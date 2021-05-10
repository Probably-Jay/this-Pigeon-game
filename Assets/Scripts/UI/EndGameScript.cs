using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameCore;

// SJ, 4/3/21
// obsoleted jay 26/04
// Hastily un-obsoleted SJ 04/05

//[System.Obsolete("Should be replaced with new system")]
public class EndGameScript : MonoBehaviour
{
    public GameObject EndGameDisplay;
    public Text squadGoals;

    // Start is called before the first frame update
    void Start()
    {
        EndGameDisplay.transform.rotation.Set(90, 0, 0, 0);
        EndGameDisplay.SetActive(false);
    }

    void OnEnable()
    {
        EventsManager.BindEvent(EventsManager.EventType.GameOver, MakeVisible);
       // EventsManager.BindEvent(EventsManager.EventType.StartGame, HideScreen);
    }

    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.GameOver, MakeVisible);
       // EventsManager.UnbindEvent(EventsManager.EventType.StartGame, HideScreen);
    }

    enum goals { 
       Loving,
       Excited,
       Stressed,
       Lonely
    }

    void MakeVisible()
    {
        
         EndGameDisplay.SetActive(true);
        
        //if (EndGameDisplay.transform.rotation.x >= 0)
        //{
        //    StartCoroutine(RotateIn());
        //}

        squadGoals.text =
         $"Garden 1 Goal: {(goals)GameManager.Instance.DataManager.GetPlayer1GoalMood()},\n" +
         $"Garden 2 Goal: {(goals)GameManager.Instance.DataManager.GetPlayer2GoalMood()}";
    }

    private IEnumerator RotateIn()
    {
        while(EndGameDisplay.transform.rotation.eulerAngles.x > 0)
        {
            var angle = EndGameDisplay.transform.rotation.eulerAngles.x;
            EndGameDisplay.transform.eulerAngles = new Vector3(Mathf.LerpAngle(angle,0,Time.deltaTime), 0, 0);
            yield return null;
        }
    }

    void HideScreen()
    {
        if (EndGameDisplay.activeSelf)
        {
            EndGameDisplay.SetActive(false);
        }
        EndGameDisplay.transform.rotation.Set(90, 0, 0, 0);
    }
}
