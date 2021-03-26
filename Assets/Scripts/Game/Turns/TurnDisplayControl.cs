using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

// created Jay 14/02

public class TurnDisplayControl : MonoBehaviour
{
    Animator animator;
    Text text;
    [SerializeField, Range(0, 1)] float turnUpdateDelay;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        text = GetComponent<Text>();
    }
    private void Start()
    {
      //  FadeInText();
    }

    private void OnEnable()
    {
        EventsManager.BindEvent(EventsManager.EventType.NewTurnBegin, FadeInText);
    }

    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.NewTurnBegin, FadeInText);
    }


    void FadeInText()
    {
        Player.PlayerEnum player = GameManager.Instance.ActivePlayer.PlayerEnumValue;
        int turnCount = GameManager.Instance.TurnCount;
        animator.SetTrigger("FadeInOut");
        StartCoroutine(UpdateTurnDisplay(player, turnCount));
    }

    private IEnumerator UpdateTurnDisplay(Player.PlayerEnum player, int turnCount)
    {
        yield return new WaitForSeconds(turnUpdateDelay);
        text.text = $"Turn {turnCount.ToString()}\nPlayer {(player == Player.PlayerEnum.Player1 ? "One" : "Two")}";
    }
}
