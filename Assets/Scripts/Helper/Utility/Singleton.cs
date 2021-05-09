using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;


// added by jay 12/02

/// <summary>
/// Singlton base class. 
/// <see cref="T"/> must be the type of the child inheriting from this class.
/// You must override <see cref="Singleton{}.Initialise"/> and call <see cref="Singleton{}.InitSingleton"/> in this method.
/// </summary>
/// <typeparam name="T">The class *inheriting* from this class, the one that will be a singleton</typeparam>
public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;
    /// <summary>
    /// The singleton instance of this class
    /// </summary>
    public static T Instance // singleton Instance
    {
        get
        {
            AssertInstanceExists();
            return instance;
        }

        private set => instance = value;

    }

    /// <summary>
    /// If an instance of this singleton currently exists
    /// </summary>
    public static bool InstanceExists => instance != null;


    /// <summary>
    /// Will throw <see cref="SingletonDoesNotExistException"/> if instance does not exist
    /// </summary>
    public static void AssertInstanceExists() { if (!InstanceExists && Application.isPlaying) throw new SingletonDoesNotExistException(); }


    /// <summary>
    /// Will output warning if instance doesnt exist
    /// </summary>d
    public static void WarnInstanceDoesNotExist() { if (!InstanceExists && Application.isPlaying && !quitting) Debug.LogWarning(DoesNotExistMessage); }



    // <summary>
    // *The deriving class must impliment a <see cref="InitSingleton"/> call inside <see cref="T.Awake"/>*
    // </summary>
    // public abstract void Awake(); // this was causing execution order issues
 

    /// <summary>
    /// *The deriving class must impliment a <see cref="InitSingleton"/> call inside <see cref="T.Initialise"/>*
    /// </summary>
    public abstract void Initialise();


    /// <summary>
    /// All child classes must call this init function
    /// </summary>
    /// <param name="dontDestroyOnLoad">If this object should be set to <see cref="Object.DontDestroyOnLoad"/></param>
    protected void InitSingleton(bool dontDestroyOnLoad = true)
    {
        if (GetType() != typeof(T)) // this should never happen
        {
            throw new System.Exception($"Singletons can only referance their own types, {typeof(Singleton<T>)} cannot be used to template typeof {GetType()}"); // this is really bad
        }

        if (instance == null || instance == this) // if instance does not exist set this to instance
        {
            Instance = this as T;
        }
        else // if instance already exists, desetroy self
        {
            Debug.LogWarning($"{typeof(Singleton<T>)} is a singleton, but multiple copies exist in the scene {SceneManager.GetActiveScene().name}, this instance will be destroyed");
            Destroy(this.gameObject); // this will avoid meltdown but the actual other copy should be removed
        }

        if(dontDestroyOnLoad) DontDestroyOnLoad(gameObject);

        AssertInstanceExists();

    }

    private void OnDestroy()
    {
        if(instance == this)
        {
            if(!quitting) Debug.Log($"Destroying singlton instance");
            instance = null;
        }
    }

    private void OnApplicationQuit()
    {
        quitting = true;
    }

    // unused
    /// <summary>
    /// Bind this to the scene change event to disable duplicates in the scene
    /// </summary>
    /// <param name="s"></param>
    /// <param name="m"></param>
    public void DisableDuplicateObjects(Scene s, LoadSceneMode m)
    {
        Singleton<T>[] duplicates = (Singleton<T>[])Resources.FindObjectsOfTypeAll(typeof(Singleton<T>));
        foreach (var singleton in duplicates)
        {
            if (singleton.GetInstanceID() != Instance.GetInstanceID())
            {
                singleton.gameObject.SetActive(false);
            }
        }
    }
    /// <summary>
    /// The application has begin to quit
    /// </summary>
    static bool quitting = false;



    private static string DoesNotExistMessage => $"{typeof(Singleton<T>)} is required by a script, but does not exist in (or has not been initialised in) scene \"{SceneManager.GetActiveScene().name}\".";

    [System.Serializable]
    protected class SingletonDoesNotExistException : System.Exception
    {
        public SingletonDoesNotExistException():base(DoesNotExistMessage) { }
    }

}
