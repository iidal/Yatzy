using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnImpact : MonoBehaviour
{
    public DiceAudioManager diceAudio;
    Rigidbody rb;
    public AudioClip clip;
    AudioSource source;
    void Start()
    {
        source = GetComponent<AudioSource>();
        diceAudio = GetComponentInParent<DiceAudioManager>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Floor")
        {
            Vector3 diceVelocity = rb.velocity;
            float vol = 0;
            if(diceVelocity.x >10 || diceVelocity.y >10 || diceVelocity.z >10){
                vol = Random.Range(0.4f,0.6f);
            }
            else if(diceVelocity.x >5 || diceVelocity.y >5 || diceVelocity.z >5){
                vol = Random.Range(0.2f,0.4f);
            }
            else{
                vol = Random.Range(0.04f,0.2f);
            }

            diceAudio.PlaySound(vol);
            // source.PlayOneShot(clip);
        }
    }
}
