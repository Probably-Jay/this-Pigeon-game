using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonInitialiser : MonoBehaviour
{
    public GameObject eventsManager;
    public GameObject applicationManager;
    public GameObject sceneChangeController;
    public GameObject gameManager;


    private void Awake()
    {
        if (EventsManager.InstanceExists) return;

        CreateSingletons();
    }

    private void CreateSingletons()
    {
        var e = Instantiate(eventsManager);
        var a = Instantiate(applicationManager);
        var s = Instantiate(sceneChangeController);
        var g = Instantiate(gameManager);

        e.GetComponent<EventsManager>().Initialise();
        a.GetComponent<AplicationManager>().Initialise();
        s.GetComponent<SceneChangeController>().Initialise();
        g.GetComponent<GameManager>().Initialise();
    }
}
