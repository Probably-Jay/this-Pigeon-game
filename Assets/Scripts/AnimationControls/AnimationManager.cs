using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public GameObject [] animationObjects;

    public GameObject sparklePrefab;

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
        AnimationStartPos = new Vector3(AnimationStartPos.x, AnimationStartPos.y + 0.7f, 0.9f); ;

        animationObjects[0].GetComponent<ToolAnimationController>().PlayAnimation(AnimationStartPos);
    }

    void PlayTrimming(Vector3 AnimationStartPos)
    {
        AnimationStartPos = new Vector3(AnimationStartPos.x, AnimationStartPos.y + 0.2f, -2f); ;
        animationObjects[1].GetComponent<ToolAnimationController>().PlayAnimation(AnimationStartPos);
    }


    void PlaySpraying(Vector3 AnimationStartPos)
    {
        AnimationStartPos = new Vector3(AnimationStartPos.x - 0.1f, AnimationStartPos.y, -2f); ;

        animationObjects[2].GetComponent<ToolAnimationController>().PlayAnimation(AnimationStartPos);
        PlayAngryBug(AnimationStartPos);
    }

    void PlayDigging(Vector3 AnimationStartPos)
    {
        AnimationStartPos = new Vector3(AnimationStartPos.x, AnimationStartPos.y, 0.9f); ;
        animationObjects[3].GetComponent<ToolAnimationController>().PlayAnimation(AnimationStartPos);
    }

   void PlayAngryBug(Vector3 AnimationStartPos)
   {       
        animationObjects[4].GetComponent<BugAnimationController>().PlayReadyBug(AnimationStartPos);
   }


    public void PlaySparkles(List<Vector3> SparkelStartPostions)
    {
        for (int i = 0; i < SparkelStartPostions.Count; i++)
        {
            SparkelStartPostions[i] = new Vector3(SparkelStartPostions[i].x, SparkelStartPostions[i].y, -2f);

            GameObject newSparkle = Instantiate(sparklePrefab);           
            newSparkle.GetComponent<SparkleAnimationController>().PlayAnimation(SparkelStartPostions[i]);
        }     
    }  
}
