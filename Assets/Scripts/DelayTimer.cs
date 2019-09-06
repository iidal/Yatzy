using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class DelayTimer : MonoBehaviour
{
    
    public float timerTime;
    private float time;
    public bool startTimerOnEnable;
    private bool timerCanRun;

    public UnityEvent onTimerEnd;
    
    void Start()
    {
        
    }

    private void OnEnable()
    {
        if (startTimerOnEnable)
        {
            timerCanRun = true;
            time = timerTime;
        }
        else {
            timerCanRun = false;
        }
    }

    public void StartTimerOnCommand() {
        timerCanRun = true;
        time = timerTime;
    }
    void Update()
    {
        if (timerCanRun) {
            time -= Time.deltaTime;
            if (time <= 0) {
                timerCanRun = false;
                onTimerEnd.Invoke();
            }
        }
    }
    public void Test() {
        Debug.Log("testi");
    }
}
