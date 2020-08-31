using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColliderButton : MonoBehaviour
{
    public UnityEvent OnClick;
    public UnityEvent OnClickRelease;

    // Start is called before the first frame update
    void Start()
    {

    }

    void Update(){

    }

    void OnMouseDown()
    { //does this work on touch screen?
     
        if (isActiveAndEnabled)
        {
            OnClick.Invoke();
        }
    }
    

}
