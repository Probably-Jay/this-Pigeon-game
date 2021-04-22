using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SFXHandler : MonoBehaviour
{

    public AudioClip[] clipArray;
    public AudioSource clipSource;
    float origPitch;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Play sound effect at specified index
    public void PlaySound(int idx) {
        clipSource.PlayOneShot(clipArray[idx]);
    }


    // Make banjo-kazooie style pet noises
    public void SpeakLoop(int len) {
        origPitch = clipSource.pitch;
        int curLoop = 0;

        if (!clipSource.isPlaying)
        {
            if (curLoop < len)
            {
                clipSource.pitch += Random.Range(-10, 10);
                clipSource.PlayOneShot(clipArray[0]);
                curLoop++;
            }
        }
        clipSource.pitch = origPitch;
    }
}
