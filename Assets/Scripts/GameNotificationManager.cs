using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameNotificationManager : MonoBehaviour
{

    /// <summary>
    /// notifications popping up during game are called through here
    /// </summary>

    public static GameNotificationManager instance;

    string notificationName;    //what popup needs to be shown

    //all the popups that are accessed through here
    public GameObject allDicesLockedCantThrow; //if player has locked all the dices and tries to throw when there's still throws left to use

    

    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        else
            instance = this;
    }

    void Start()
    {
        //making sure all the popups are inactive at the start of the game (if they need to be). 
        allDicesLockedCantThrow.SetActive(false);   
    }

    //this is called with the needed name of the popup that is needed
    public void ShowNotification(string name) {

        notificationName = name;
        switch (notificationName) {
            case "allDicesLockedCantThrow":
                allDicesLockedCantThrow.SetActive(true);
                break;
            default:
                break;
        }
    }


}
