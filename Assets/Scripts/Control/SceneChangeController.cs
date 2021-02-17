using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// jay 17/02

public class SceneChangeController : Singleton<SceneChangeController>
{

    public enum Scenes 
    {
        MainMenu
        , Game
    }

    [SerializeField] int MainMenuBuildIndex;
    [SerializeField] int GameBuildIndex;

    AsyncOperation loadingScene;

    public bool LoadingSceneInProgress { get => loadingScene != null; }

    /// <summary>
    /// A float representation of the loading progress of the scene currently loading
    /// </summary>
    public float LoadingSceneProgress { 
        get
        {
            if (!LoadingSceneInProgress) return 0;
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

    void LoadScene(Scenes scene) => StartCoroutine(LoadSceneAsync(sceneBuildIndexesDictionary[scene]));

    IEnumerator LoadSceneAsync(int buildInxed)
    {
        EventsManager.InvokeEvent(EventsManager.EventType.BeginSceneLoad);
        loadingScene = SceneManager.LoadSceneAsync(buildInxed);
        loadingScene.allowSceneActivation = false;
        while (!loadingScene.isDone)
        {

        }
    }
    
}
