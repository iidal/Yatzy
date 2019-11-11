using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColliderButton : MonoBehaviour
{
    public UnityEvent OnClick;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnMouseDown(){ //does this work on touch screen?
        Debug.Log("clicked");
        OnClick.Invoke();
    }
   
}
