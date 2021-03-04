using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// SJ, 4/3/21
public class EndGameScript : MonoBehaviour
{
    public GameObject hideBuffer;
    public Text squadGoals;

    // Start is called before the first frame update
    void Start()
    {
        hideBuffer.transform.rotation.Set(90, 0, 0, 0);
        hideBuffer.SetActive(false);
    }

    void OnEnable()
    {
        EventsManager.BindEvent(EventsManager.EventType.GameOver, MakeVisible);
        EventsManager.BindEvent(EventsManager.EventType.EnterNewScene, HideScreen);
    }

    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.GameOver, MakeVisible);
        EventsManager.UnbindEvent(EventsManager.EventType.EnterNewScene, HideScreen);
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    if (GameManager.Instance.PlantManager.gameWon)
    //    {
    //        makeVisible();

          

    //    }
    //    else
    //    {
    //        hideScreen();
    //    }
    //}

    void MakeVisible()
    {
        if (!hideBuffer.activeSelf)
        {
            hideBuffer.SetActive(true);
        }
        if (hideBuffer.transform.rotation.x >= 0)
        {
            hideBuffer.transform.Rotate(-1, 0, 0);
        }

        squadGoals.text =
         $"Garden 1 Goal: {GameManager.Instance.CurrentGoal.ToString()},\n" +
         $"Garden 2 Goal: {GameManager.Instance.AlternateGoal.ToString()}";
    }

    void HideScreen()
    {
        if (hideBuffer.activeSelf)
        {
            hideBuffer.SetActive(false);
        }
        hideBuffer.transform.rotation.Set(90, 0, 0, 0);
    }
}
