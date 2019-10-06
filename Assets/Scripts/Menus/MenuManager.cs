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
        SaveLoad.Load();    //loading saved data first thing when the game starts
    }




}
