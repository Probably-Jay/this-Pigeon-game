using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class GoalScriptAnimationController : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }


    private void OnEnable()
    {

        EventsManager.BindEvent(EventsManager.EventType.moodSlidersExplanation, SlightlyBigger);
        EventsManager.BindEvent(EventsManager.EventType.OnDialogueClose, SlightlySmaller);

        EventsManager.BindEvent(EventsManager.EventType.SeedBagOpen, SlightlyBigger);
        EventsManager.BindEvent(EventsManager.EventType.OnSeedBagClose, SlightlySmaller);
    }

    private void Bigger() => animator.SetBool("bigger", true);
    private void Smaller() => animator.SetBool("bigger", false);


    private void SlightlyBigger() => animator.SetBool("slightlyBigger", true);
    private void SlightlySmaller() => animator.SetBool("slightlyBigger", false);
}
