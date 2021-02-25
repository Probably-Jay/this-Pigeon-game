using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalStore : Singleton<GoalStore>
{

 

    public override void Awake()
    {
        InitSingleton();
    }

    public void StoreGoal(GameManager.Goal goal)
    {
        PlayerPrefs.SetInt("Goal", (int)goal);
        PlayerPrefs.Save();
    }

    public GameManager.Goal GetGoal()
    {
        return (GameManager.Goal)PlayerPrefs.GetInt("Goal");
    }


}
