using System;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    public static Conductor current;

    private Dictionary<int, AudioClip> _switchMusic;
    private Dictionary<AudioClip, AudioClip> _increaseTempo;
    private Dictionary<AudioClip, AudioClip> _decreaseTempo;
    
    public AudioSource audioSource;
    public AudioClip songIntroSlow;
    public AudioClip songLoopSlow;
    public AudioClip songIntroNormal;
    public AudioClip songLoopNormal;
    public AudioClip songIntroFast;
    public AudioClip songLoopFast;
    private float _currentSongTime; 
    public event Action SongLooped;
    
    private void Awake()
    {
        current = this;
        
        _switchMusic = new Dictionary<int, AudioClip>()
        {
            { -1, songLoopSlow },
            { 0, songLoopNormal },
            { 1, songLoopFast }
        };
        _increaseTempo = new Dictionary<AudioClip, AudioClip>()
        {
            {songIntroSlow, songIntroNormal},
            {songIntroNormal, songIntroFast},
            {songLoopSlow, songLoopNormal},
            {songLoopNormal, songLoopFast}
        };
        _decreaseTempo = new Dictionary<AudioClip, AudioClip>()
        {
            {songIntroFast, songIntroNormal},
            {songIntroNormal, songIntroSlow},
            {songLoopFast, songLoopNormal},
            {songLoopNormal, songLoopSlow}
        };
    }
    
    private void Start()
    {
        audioSource.clip = songIntroNormal;
        audioSource.loop = false;
        audioSource.Play();
        _currentSongTime = audioSource.time;
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            SwitchMusicFromIntroToLoop();
        }

        if (_currentSongTime > audioSource.time && SongLooped != null)
        {
            SongLooped();
        }
        _currentSongTime = audioSource.time;
    }

    private void SwitchMusicFromIntroToLoop()
    {
        audioSource.clip = _switchMusic[GameManager.current.GetAudioTempo()];
        audioSource.Play();
        audioSource.loop = true;
    }

    public void IncreaseTempo() {
        if (GameManager.current.GetAudioTempo() == 1) {
            return;
        }
        var audioSourceTimeBeforeSwitching = audioSource.time;
        audioSource.clip = _increaseTempo[audioSource.clip];
        audioSource.time = audioSourceTimeBeforeSwitching;
        audioSource.Play();
    }
    
    public void DecreaseTempo() {
        if (GameManager.current.GetAudioTempo() == -1) {
            return;
        }
        var audioSourceTimeBeforeSwitching = audioSource.time;
        audioSource.clip = _decreaseTempo[audioSource.clip];
        audioSource.time = audioSourceTimeBeforeSwitching;
        audioSource.Play();
    }

}
