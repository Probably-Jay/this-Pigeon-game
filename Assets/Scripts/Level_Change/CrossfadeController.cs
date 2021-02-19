using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

// jay 18/02

public class CrossfadeController : MonoBehaviour
{

    [SerializeField] Animator animator;
    //[SerializeField,Range(0,1)] float transitionTime = 1;


    private void OnEnable()
    {
        EventsManager.BindEvent(EventsManager.EventType.BeginSceneLoad, CrossfadeExitScene);
        EventsManager.BindEvent(EventsManager.EventType.SceneLoadComplete, CrossfadeEnterScene);
    }

    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.BeginSceneLoad, CrossfadeExitScene);
        EventsManager.UnbindEvent(EventsManager.EventType.SceneLoadComplete, CrossfadeEnterScene);
    }

    /// <summary>
    /// Crossfade for leaving a scene
    /// </summary>
    void CrossfadeExitScene()
    {
        animator.SetTrigger("CrossfadeExitScene");
        StartCoroutine(WaitForAnimatioeToReachState("Loading"));
    }

    /// <summary>
    /// crossfade for entering a scene
    /// </summary>
    void CrossfadeEnterScene()
    {
        animator.SetTrigger("CrossfadeEnterScene");
        StartCoroutine(WaitForAnimatioeToReachState("Passive"));
    }


    IEnumerator WaitForAnimatioeToReachState(string name)
    {
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(name))
        {
            yield return null;
        }
        EventsManager.InvokeEvent(EventsManager.EventType.CrossfadeAnimationEnd);
    }

}
