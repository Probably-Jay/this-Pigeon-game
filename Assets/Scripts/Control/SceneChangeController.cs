using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


// jay 17/02

public class SceneChangeController : Singleton<SceneChangeController>
{
    [System.Serializable]
    public enum Scenes 
    {
        MainMenu
        , Game
    }

    [SerializeField] int MainMenuBuildIndex;
    [SerializeField] int GameBuildIndex;

    [SerializeReference] GameObject LoadingScreen;
    [SerializeField] Slider progressBar;

    AsyncOperation loadingScene;

    public bool CurrentlyChangingScene { get => loadingScene != null; }


    /// <summary>
    /// A float representation of the loading progress of the scene currently loading
    /// </summary>
    public float LoadingSceneProgress { 
        get
        {
            if (!CurrentlyChangingScene) return 0;
            return Mathf.Clamp01(loadingScene.progress / 0.9f); // 0.9 represents a fully loaded scene
        } 
    }

    Dictionary<Scenes, int> sceneBuildIndexesDictionary = new Dictionary<Scenes, int>();

    private void CreateBuildIndexDictionary()
    {
        sceneBuildIndexesDictionary.Add(Scenes.MainMenu, MainMenuBuildIndex);
        sceneBuildIndexesDictionary.Add(Scenes.Game, GameBuildIndex);
    }

    new public static SceneChangeController Instance { get => Singleton<SceneChangeController>.Instance; }

    public override void Awake()
    {
        InitSingleton();
        CreateBuildIndexDictionary();
    }

    /// <summary>
    /// Will asynchornously load the scene at the enumvalue provided
    /// </summary>
    /// <param name="scene">Enum value of the scene to load</param>
    public void LoadSceneAsync(Scenes scene) => StartCoroutine(LoadSceneAsyncCoroutine(sceneBuildIndexesDictionary[scene]));

    /// <summary>
    /// Prefer <see cref="LoadSceneAsync"/> for saftey, but this overload allows passing build indexes directly
    /// </summary>
    /// <param name="scene">Build index</param>
    public void LoadSceneAsync(int scene) {
        Debug.Assert(scene <= SceneManager.sceneCountInBuildSettings,$"Invalid scene index {scene} attampted to load");
        StartCoroutine(LoadSceneAsyncCoroutine(scene)); 
    }


    IEnumerator LoadSceneAsyncCoroutine(int buildInxed)
    {
        BeginLoad(buildInxed);
        while (LoadingSceneProgress < 1)
        {
            progressBar.value = LoadingSceneProgress;
            yield return null;
        }
        progressBar.value = LoadingSceneProgress;
        EndLoad();
    }


    private void BeginLoad(int buildInxed)
    {
        EventsManager.InvokeEvent(EventsManager.EventType.BeginSceneLoad);
        LoadingScreen.SetActive(true);
        loadingScene = SceneManager.LoadSceneAsync(buildInxed);
        loadingScene.allowSceneActivation = false; // wait to finish scene load until we tell it to
    }
    private void EndLoad()
    {
        loadingScene.allowSceneActivation = true; // wait to finish scene load until we tell it to
        LoadingScreen.SetActive(false);
    }
}
