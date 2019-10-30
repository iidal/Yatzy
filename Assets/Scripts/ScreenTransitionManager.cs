using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScreenTransitionManager : MonoBehaviour
{
    public Animator animator;
    public UnityEvent afterAnimation;
    public void TransitionScreen(string dir){
        StartCoroutine("Transition", dir);
    }
    IEnumerator Transition(string dir)
    {
        //maybe should add some failsafe if animation names are wrong
        string animName = dir;
        animator.Play(animName);
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil (()=> animator.GetCurrentAnimatorStateInfo(0).IsName("default"));

        afterAnimation.Invoke();
    }
}
