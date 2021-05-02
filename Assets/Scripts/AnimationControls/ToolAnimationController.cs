using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolAnimationController : MonoBehaviour
{   
    public Animator animator;
    public string animationName;
    int animationLoops = 0;
    bool playingAnimation = false;

    public GameObject linkedTool;

    Image uiImage;

    private void Start()
    {
        uiImage = linkedTool.GetComponent<Image>();     
    }


    // Update is called once per frame
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
        {          
            playingAnimation = true;
            var tempColor = uiImage.color;
            tempColor.a = 0f;
            uiImage.color = tempColor;
        }
        else if(playingAnimation == true)
        {          
            var tempColor = uiImage.color;
            tempColor.a = 1f;
            uiImage.color = tempColor;
            playingAnimation = false;
        }   
    }


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
