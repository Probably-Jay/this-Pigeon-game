using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SparkleAnimationController : MonoBehaviour
{   
    public Animator animator;
    public string animationName;
    bool playingAnimation = false;

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animationName) && playingAnimation == true)
        {
            Destroy(gameObject);
        }
    }

    public void PlayAnimation(Vector3 newPosition)
    {
        playingAnimation = true;

       transform.position = newPosition;
        // Turn animation on
        animator.SetTrigger("PlayAnimation");
    }
}
