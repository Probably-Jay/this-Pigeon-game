using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


// jay 17/02

/// <summary>
/// Class which controls the switching of scenes
/// </summary>
public class SceneChangeController : Singleton<SceneChangeController>
{
    /// <summary>
    /// Scenes in the game
    /// </summary>
    [System.Serializable]
    public enum Scenes 
    {
        MainMenu

        , ConnectingScene
                
        , MoodSelectScreen1
        , MoodSelectScreen2
    //    , PersonalGarden
        , Tutorial
        , Game
    }

    [SerializeField] int MainMenuIndex;
    [SerializeField] int ConectingSceneIndex;
    [SerializeField] int MoodSelect1BuildIndex;
    [SerializeField] int MoodSelect2BuildIndex;
   // [SerializeField] int PersonalGardenBuildIndex;  
    [SerializeField] int TutorialBuildIndex;
    [SerializeField] int GameBuildIndex;


    [SerializeReference] GameObject LoadingScreen;
    [SerializeField] Slider progressBar;

    AsyncOperation loadingScene;
    bool transitionAnimationDone;

    /// <summary>
    /// If the scene is changing
    /// </summary>
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
        sceneBuildIndexesDictionary.Add(Scenes.MainMenu, MainMenuIndex);
        sceneBuildIndexesDictionary.Add(Scenes.ConnectingScene, ConectingSceneIndex);
        sceneBuildIndexesDictionary.Add(Scenes.MoodSelectScreen1, MoodSelect1BuildIndex);
        sceneBuildIndexesDictionary.Add(Scenes.MoodSelectScreen2, MoodSelect2BuildIndex);
       // sceneBuildIndexesDictionary.Add(Scenes.PersonalGarden, PersonalGardenBuildIndex);
        sceneBuildIndexesDictionary.Add(Scenes.Tutorial, TutorialBuildIndex);
        sceneBuildIndexesDictionary.Add(Scenes.Game, GameBuildIndex);
    }

    new public static SceneChangeController Instance { get => Singleton<SceneChangeController>.Instance; }

    private void OnEnable()
    {
        EventsManager.BindEvent(EventsManager.EventType.CrossfadeAnimationEnd, TransitionAnimationDone);
    }

    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.CrossfadeAnimationEnd, TransitionAnimationDone);
    }

    private void TransitionAnimationDone() => transitionAnimationDone = true;


    //public override void Awake()
    //{
    //    InitSingleton();
    //    CreateBuildIndexDictionary();
    //}

    //private void Awake()
    //{

    //}

    public override void Initialise()
    {
        InitSingleton();
        CreateBuildIndexDictionary();
    }

    /// <summary>
    /// Will asynchornously load the scene at the enumvalue provided and enter that scene
    /// </summary>
    /// <param name="scene">Enum value of the scene to load</param>
    public void ChangeScene(Scenes scene) => StartCoroutine(LoadSceneAsyncCoroutine(sceneBuildIndexesDictionary[scene]));

    /// <summary>
    /// Prefer <see cref="ChangeScene"/> for saftey, but this overload allows passing build indexes directly
    /// </summary>
    /// <param name="scene">Build index</param>
    public void ChangeScene(int scene) {
        Debug.Assert(scene <= SceneManager.sceneCountInBuildSettings,$"Invalid scene index {scene} attampted to load");
        StartCoroutine(LoadSceneAsyncCoroutine(scene)); 
    }


    IEnumerator LoadSceneAsyncCoroutine(int buildInxed)
    {
        if (CurrentlyChangingScene) yield break;
        BeginLoad(buildInxed);
        while (LoadingSceneProgress < 1)
        {
            progressBar.value = LoadingSceneProgress;
            yield return null; // wait for load to end
        }
        progressBar.value = LoadingSceneProgress;
        while (!transitionAnimationDone)
        {
            yield return null; // wait for transition to end
        }
        EndLoad();
        while (!transitionAnimationDone) // load will always need to be done to reach here
        {
            yield return null; // wait for enter scene transition to end
        }
        EnterScene();
    }



    private void BeginLoad(int buildInxed)
    {
        LoadingScreen.SetActive(true);
        EventsManager.InvokeEvent(EventsManager.EventType.BeginSceneLoad);
        loadingScene = SceneManager.LoadSceneAsync(buildInxed);
        loadingScene.allowSceneActivation = false; // wait to finish scene load until we tell it to
        transitionAnimationDone = false;
    }



    private void EndLoad()
    {
        EventsManager.InvokeEvent(EventsManager.EventType.SceneLoadComplete);
        loadingScene.allowSceneActivation = true; // wait to finish scene load until we tell it to
        transitionAnimationDone = false; // transition into new scene
    }

    private void EnterScene()
    {
        LoadingScreen.SetActive(false);
        EventsManager.InvokeEvent(EventsManager.EventType.EnterNewScene);
        loadingScene = null;
    }
}
