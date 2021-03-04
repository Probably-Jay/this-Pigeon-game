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
        e.GetComponent<EventsManager>().Initialise();

        var app = Instantiate(applicationManager);
        ApplicationManager acomp = app.GetComponent<ApplicationManager>();
        acomp.Initialise();

        var s = Instantiate(sceneChangeController);
        s.GetComponent<SceneChangeController>().Initialise();

        var g = Instantiate(gameManager);
        GameManager gcomp = g.GetComponent<GameManager>();
        gcomp.Initialise();

    }
}
