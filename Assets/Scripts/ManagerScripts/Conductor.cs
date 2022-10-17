using System;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    public static Conductor current;
    
    public AudioSource audioSource;
    public AudioClip songIntroNormal;
    public AudioClip songLoopNormal;
    private void Awake()
    {
        current = this;
    }
    
    private void Start()
    {
        audioSource.clip = songIntroNormal;
        audioSource.loop = false;
        audioSource.Play();
    }

    private void Update()
    {
        if (GameManager.current.IsGamePaused() || GameManager.current.HasGameEnded() 
                                               || GameManager.current.playerIsDying) return;
        if (!audioSource.isPlaying)
        {
            SwitchMusicFromIntroToLoop();
            enabled = false;
        }
    }

    private void SwitchMusicFromIntroToLoop()
    {
        audioSource.clip = songLoopNormal;
        audioSource.Play();
        audioSource.loop = true;
    }

    public void Pause()
    {
        audioSource.Pause();
    }

    public void Resume()
    {
        audioSource.Play();
    }
}
