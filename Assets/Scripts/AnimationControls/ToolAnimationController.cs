using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Script created by Alexander Purvis 04/05/2021

public class ToolAnimationController : MonoBehaviour
{
    public Animator animator;
    public string animationName;
    public bool playingAnimation = false;

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
            var tempColor = uiImage.color;
            tempColor.a = 0f;
            uiImage.color = tempColor;
            playingAnimation = true;
        }
        else if (playingAnimation == true)
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
        Debug.Log("Turn animation on");
    }
}