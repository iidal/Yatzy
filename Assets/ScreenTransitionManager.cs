using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScreenTransitionManager : MonoBehaviour
{
    Animator animator;
    public UnityEvent afterAnimation;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void TransitionScreen(string dir){
        StartCoroutine("Transition", dir);
    }
    
    public IEnumerator Transition(string dir)
    {
        string animName = dir;
        // if (dir == "in")
        //     animator.Play("in");
        // else if(dir == "out")
        // {
        //     animator.Play("out");
        // }
        // else{
        //     Debug.Log("screen transition direction in correct");
        //     yield break;
        // }

        animator.Play(animName);
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil (()=> animator.GetCurrentAnimatorStateInfo(0).IsName("default"));

        afterAnimation.Invoke();
    }
}
