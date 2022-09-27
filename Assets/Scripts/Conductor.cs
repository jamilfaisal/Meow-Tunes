using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    public AudioSource songIntro;
    public AudioSource songLoop;



    void Start()
    {
        songIntro.Play();
        // Play the Loop version after the intro version is done
        songLoop.PlayDelayed(songIntro.clip.length);
    }
}
