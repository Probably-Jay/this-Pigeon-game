using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// Script created by Alexander Purvis 04/05/2021
public class BugAnimationController : MonoBehaviour
{   
    public Animator animator;
    public GameObject SprayToolObject;
    ToolAnimationController sprayingToolAnimation;

    bool bugReady = false;
    Vector3 AnimationPosition;

    private void Start()
    {
        sprayingToolAnimation = SprayToolObject.GetComponent<ToolAnimationController>();
    }

    private void Update()
    {
        if (sprayingToolAnimation.playingAnimation == false && bugReady == true)
        {
            PlayAnimation();
        }
    }

    public void PlayReadyBug(Vector3 newPosition)
    {
        bugReady = true;
        AnimationPosition = newPosition;
        transform.position = AnimationPosition;
        // Turn animation on     
        animator.SetTrigger("ReadyBug");

        Debug.Log("ready bug");
    }


    void PlayAnimation()
    {
        bugReady = false;
        transform.position = AnimationPosition;
        // Turn animation on     
        animator.SetTrigger("PlayBugAnimation");
        Debug.Log("play bug");
    }
}
