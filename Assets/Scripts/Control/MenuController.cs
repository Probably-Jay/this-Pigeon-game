using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToMyGarden()
    {
        // Change to metagarden after metagarden added
        SceneChangeController.Instance.ChangeScene(SceneChangeController.Scenes.MoodSelectScreen);
    }

    public void GoToLastGarden()
    {
        // Change to last garden once multiple are available
        SceneChangeController.Instance.ChangeScene(SceneChangeController.Scenes.MoodSelectScreen);
    }

    public void GoToOptions()
    {
        // Change to options screen once we have some to change (sound levels, accessibility options, etc)
        
    }


    public void GoToCredits()
    {
        // Change to credits scene once added
    }


    public void QuitGame()
    {
        EventsManager.InvokeEvent(EventsManager.EventType.QuitGame);
    }
}
