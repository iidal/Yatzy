using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// if something needs to happen with a delay
/// </summary>

public class DelayTimer : MonoBehaviour
{
    
    public float timerTime; //time set by user
    private float time; //temp timer 
    public bool startTimerOnEnable; //should the timer start when gameobject is enabled (false = start when triggered)
    private bool timerCanRun;   //timer on/off

    public UnityEvent onTimerEnd;   //actions to be triggered
    

    private void OnEnable()
    {   //start on enable
        if (startTimerOnEnable)
        {
            timerCanRun = true;
            time = timerTime;
        }
        else {
            timerCanRun = false;
        }
    }
    //start when triggered
    public void StartTimerOnCommand() {
        timerCanRun = true;
        time = timerTime;
    }
    void Update()
    {   //timer running
        if (timerCanRun) {
            time -= Time.deltaTime;
            if (time <= 0) {
                timerCanRun = false;
                onTimerEnd.Invoke();
            }
        }
    }
}
