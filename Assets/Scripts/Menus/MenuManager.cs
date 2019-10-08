using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
handles main things in menu scene
 */

public class MenuManager : MonoBehaviour
{
    
    void Awake()
    {
        SaveLoad.LoadSoloScores();    //loading saved data first thing when the game starts
    }



    public void ClearSavedData(string fileName){
        SaveLoad.DeleteFile(fileName);
    }


}
