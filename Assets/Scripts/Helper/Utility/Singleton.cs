using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// added by jay 12/02

/// <summary>
/// Singlton base class. 
/// <see cref="T"/> must be the type of the child inheriting from this class.
/// You must override <see cref="Singleton{}.Awake"/> and call <see cref="Singleton{}.InitSingleton"/> in this method.
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
    public static void AssertInstanceExists() {if(!InstanceExists && Application.isPlaying) throw new SingletonDoesNotExistException(); }
    

    /// <summary>
    /// Will output warning if instance doesnt exist
    /// </summary>d
    protected static void WarnInstanceDoesNotExist()  {if (!InstanceExists && Application.isPlaying) Debug.LogWarning(DoesNotExistMessage);}



    /// <summary>
    /// *The deriving class must impliment a <see cref="InitSingleton"/> call inside <see cref="T.Awake"/>*
    /// </summary>
    public abstract void Awake();


    /// <summary>
    /// All child classes must call this init function
    /// </summary>
    /// <param name="dontDestroyOnLoad">If this object should be set to <see cref="Object.DontDestroyOnLoad"/></param>
    protected void InitSingleton(bool dontDestroyOnLoad = true)
    {
        if (GetType() != typeof(T)) // this should never happen
        {
            Debug.LogError($"Singletons can only referance their own types, {typeof(Singleton<T>)} cannot be used to template typeof {GetType()}");
            throw new UnityException(); // this is really bad
        }

        if (instance == null || instance == this) // if instance does not exist set this to instance
        {
            Instance = this as T;
        }
        else // if instance already exists, desetroy self
        {
            Debug.LogWarning(typeof(Singleton<T>) + " is a singleton, but multiple copies exist in the scene " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + ", this instance will be destroyed", this.gameObject);
            Destroy(this.gameObject); // this will avoid meltdown but the actual other copy should be removed
        }

        if(dontDestroyOnLoad) DontDestroyOnLoad(gameObject);

        AssertInstanceExists();

    }

    private static string DoesNotExistMessage { get=> $"{typeof(Singleton<T>)} is required by a script, but does not exist in (or has not been initialised in) scene \"{ UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}\"."; } 

    [System.Serializable]
    protected class SingletonDoesNotExistException : System.Exception
    {
        public SingletonDoesNotExistException():base(DoesNotExistMessage) { }
    }

}
