using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceAudioManager : MonoBehaviour
{
    public AudioClip clip;
    AudioSource source;
    bool playingSound = false;
    float playWait= 0;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlaySound(float vol){
        StartCoroutine("Play", vol);

    }
    IEnumerator Play(float vol){
        if(playingSound == false)
        {
            playingSound = true;
            source.pitch = Random.Range(0.7f, 1.1f);
            source.volume = vol;
            source.PlayOneShot(clip);
            playWait = clip.length * 0.7f;
            yield return new WaitForSeconds(clip.length);
            playingSound = false;
        }

    }
}
