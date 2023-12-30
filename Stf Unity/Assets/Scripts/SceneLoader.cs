using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    //int currentSceneIndex;
    public void LoadQuestScene()
    {
        //currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene("Daily Quest");
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("Main");
    }
}
