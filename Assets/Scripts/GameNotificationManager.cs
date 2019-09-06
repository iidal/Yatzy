using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameNotificationManager : MonoBehaviour
{

    /// <summary>
    /// notifications popping up during game are called through here
    /// </summary>

    public static GameNotificationManager instance;

    string notificationName;


    public GameObject allDicesLockedCantThrow;

    

    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        else
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        allDicesLockedCantThrow.SetActive(false);
    }

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
