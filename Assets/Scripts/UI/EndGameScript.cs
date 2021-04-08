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
       // EventsManager.BindEvent(EventsManager.EventType.StartGame, HideScreen);
    }

    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.GameOver, MakeVisible);
       // EventsManager.UnbindEvent(EventsManager.EventType.StartGame, HideScreen);
    }



    void MakeVisible()
    {
        if (!hideBuffer.activeSelf)
        {
            hideBuffer.SetActive(true);
        }
        if (hideBuffer.transform.rotation.x >= 0)
        {
            StartCoroutine(RotateIn());
        }

        squadGoals.text =
         $"Garden 1 Goal: {GameManager.Instance.Player1Goal.ToString()},\n" +
         $"Garden 2 Goal: {GameManager.Instance.Player2Goal.ToString()}";
    }

    private IEnumerator RotateIn()
    {
        while(hideBuffer.transform.rotation.eulerAngles.x > 0)
        {
            var angle = hideBuffer.transform.rotation.eulerAngles.x;
            hideBuffer.transform.eulerAngles = new Vector3(Mathf.LerpAngle(angle,0,Time.deltaTime), 0, 0);
            yield return null;
        }
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
