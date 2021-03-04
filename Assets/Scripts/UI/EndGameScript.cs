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
        EventsManager.BindEvent(EventsManager.EventType.GameOver, makeVisible);
        EventsManager.BindEvent(EventsManager.EventType.EnterNewScene, hideScreen);
    }

    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.GameOver, makeVisible);
        EventsManager.UnbindEvent(EventsManager.EventType.EnterNewScene, hideScreen);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.PlantManager.gameWon)
        {
            makeVisible();

            squadGoals.text =
           $"Garden 1 Goal: {GameManager.Instance.CurrentGoal.ToString()},\n" +
           $"Garden 2 Goal: {GameManager.Instance.AlternateGoal.ToString()}";

        }
        else
        {
            hideScreen();
        }
    }

    void makeVisible()
    {
        if (!hideBuffer.activeSelf)
        {
            hideBuffer.SetActive(true);
        }
        if (hideBuffer.transform.rotation.x >= 0)
        {
            hideBuffer.transform.Rotate(-1, 0, 0);
        }
    }

    void hideScreen()
    {
        if (hideBuffer.activeSelf)
        {
            hideBuffer.SetActive(false);
        }
        hideBuffer.transform.rotation.Set(90, 0, 0, 0);
    }
}
