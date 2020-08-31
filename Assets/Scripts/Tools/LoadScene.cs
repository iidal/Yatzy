using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public string defaultScene = "Main";    //if a scene name has not been given in inspector, this will be loaded


    public void LoadNewScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
        
    }
}
