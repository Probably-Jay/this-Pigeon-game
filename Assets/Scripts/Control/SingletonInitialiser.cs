using UnityEngine;
using NetSystem;
using SaveSystem;
using GameCore;

// created Jay 04/03

 /// <summary>
 /// Class responsible for ensuring there is only ever exactly one of each singleton in each scene
 /// </summary>
public class SingletonInitialiser : MonoBehaviour
{
    [SerializeField] GameObject eventsManager;
    [SerializeField] GameObject applicationManager;
    [SerializeField] GameObject sceneChangeController;
    [SerializeField] GameObject saveManager;
    [SerializeField] GameObject gameManager;
    [SerializeField] GameObject networkHandler;


    private void Awake()
    {
        CreateSingletons();
    }

    private void CreateSingletons()
    {
        CreateSingleon<EventsManager>(eventsManager);
        CreateSingleon<ApplicationManager>(applicationManager);
        CreateSingleon<SceneChangeController>(sceneChangeController);
        CreateSingleon<SaveManager>(saveManager);
        CreateSingleon<NetworkHandler>(networkHandler);
        CreateSingleon<GameManager>(gameManager);
    }

    private void CreateSingleon<T>(GameObject singletonPrefab) where T : Singleton<T>
    {
        if (Singleton<T>.InstanceExists)
        {
            return;
        }

        if(singletonPrefab == null)
        {
            Singleton<T>.WarnInstanceDoesNotExist();
            return;
        }

        var singletonObject = Instantiate(singletonPrefab);
        singletonObject.GetComponent<T>().Initialise();
    }
}
