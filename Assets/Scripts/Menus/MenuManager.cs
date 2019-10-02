using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        SaveLoad.Load();    //loading saved data first thing when the game starts
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
