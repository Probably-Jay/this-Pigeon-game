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
    private void Awake()
    {
        animator = GetComponent<Animator>();
        text = GetComponent<Text>();
    }
    private void Start()
    {
        FadeInText();
    }

    private void OnEnable()
    {
        EventsManager.BindEvent(EventsManager.EventType.EndTurn, FadeInText);
    }

    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.EndTurn, FadeInText);
    }


    void FadeInText()
    {
        Player.PlayerEnum player = GameManager.Instance.ActivePlayer.PlayerEnumValue;
        int turnCount = GameManager.Instance.TurnCount;
        text.text = $"Turn {turnCount.ToString()}\nPlayer {(player == Player.PlayerEnum.Player0 ? "One" : "Two")}'s turn";
        animator.SetTrigger("FadeInOut");
    }

}
