using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectAnimationController : MonoBehaviour
{   
    public Animator animator;
    public string animationName;
    int animationLoops = 0;
    bool playingAnimation = false;

   
  
    public void PlayAnimation(Vector3 newPosition)
    {
       transform.position = newPosition;

        // Turn animation on
        playingAnimation = true;
        animator.SetTrigger("PlayAnimation");
        animationLoops++;
        Debug.Log("Turn animation on");
    }
}
