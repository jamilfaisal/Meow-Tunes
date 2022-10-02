using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    
    public GameManager gameManager;
    public AudioSource audioSourceIntro;
    public AudioSource audioSourceLoop;
    private int audioSourcePlaying = 0;
    private AudioSource[] audioSources;

    void Start()
    {
        audioSourceIntro.Play();
        audioSources = new AudioSource[] {audioSourceIntro, audioSourceLoop};
    }

    private void FixedUpdate() {
        if (gameManager.isGamePaused()) return;
        if (!audioSourceIntro.isPlaying && audioSourcePlaying == 0) {
            audioSourceLoop.Play();
            audioSourcePlaying = 1;
        }
    }

    public void Pause()
    {
        audioSources[audioSourcePlaying].Pause();
    }
    
    public void Resume()
    {
        audioSources[audioSourcePlaying].Play();
    }
}
