using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public GameObject [] animationObjects;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PlayToolAnimation(Vector3 plantPos, int animationNumber)
    {
        plantPos.z = 0.9184281f;

        switch (animationNumber)
        {
            case 1:
                PlayWatering(plantPos);
                break;

            case 2:
                PlayTrimming(plantPos);
                break;

            case 3:
                PlaySpraying(plantPos);
                break;

            case 4:
                PlayDigging(plantPos);
                break;

            default:

                break;
        }
    }


    void PlayWatering(Vector3 AnimationStartPos)
    {
        animationObjects[0].GetComponent<ToolAnimationController>().PlayAnimation(AnimationStartPos);
    }

    void PlayTrimming(Vector3 AnimationStartPos)
    {
        animationObjects[1].GetComponent<ToolAnimationController>().PlayAnimation(AnimationStartPos);
    }

    void PlaySpraying(Vector3 AnimationStartPos)
    {
        animationObjects[2].GetComponent<ToolAnimationController>().PlayAnimation(AnimationStartPos);
    }

    void PlayDigging(Vector3 AnimationStartPos)
    {

    }

   void PlayAngryBug(Vector3 AnimationStartPos)
   {

   }
}
