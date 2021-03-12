using UnityEngine;

// created Jay 04/03

 /// <summary>
 /// Class responsible for ensuring there is only ever exactly one of each singleton in each scene
 /// </summary>
public class SingletonInitialiser : MonoBehaviour
{
    public GameObject eventsManager;
    public GameObject applicationManager;
    public GameObject sceneChangeController;
    public GameObject saveManager;
    public GameObject gameManager;


    private void Awake()
    {
        if (EventsManager.InstanceExists) return;

        CreateSingletons();
    }

    private void CreateSingletons()
    {
        var eventsManagerObject = Instantiate(eventsManager);
        eventsManagerObject.GetComponent<EventsManager>().Initialise();

        var applicationManagerObject = Instantiate(applicationManager);
        applicationManagerObject.GetComponent<ApplicationManager>().Initialise();
       
        var sceneChangeControllerObject = Instantiate(sceneChangeController);
        sceneChangeControllerObject.GetComponent<SceneChangeController>().Initialise();
        
        var saveManagerObject = Instantiate(saveManager);
        saveManagerObject.GetComponent<SaveManager>().Initialise();

        var gameManagerObject = Instantiate(gameManager);
        gameManagerObject.GetComponent<GameManager>().Initialise();
        
    }
}
