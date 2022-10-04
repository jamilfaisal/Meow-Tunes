using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    public static Conductor current;

    // 0 is normal, 1 is fast
    private int _audioPlaying;
    private AudioClip[] _switchMusic;
    private Dictionary<AudioClip, AudioClip> _increaseTempo;
    private Dictionary<AudioClip, AudioClip> _decreaseTempo;
    public AudioSource audioSource;
    public AudioClip songIntroNormal;
    public AudioClip songLoopNormal;
    public AudioClip songIntroFast;
    public AudioClip songLoopFast;
    
    private void Awake()
    {
        current = this;
        _switchMusic = new[]
        {
            songLoopNormal,
            songLoopFast
        };
        _increaseTempo = new Dictionary<AudioClip, AudioClip>()
        {
            {songIntroNormal, songIntroFast},
            {songLoopNormal, songLoopFast}
        };
        _decreaseTempo = new Dictionary<AudioClip, AudioClip>()
        {
            {songIntroFast, songIntroNormal},
            {songLoopFast, songLoopNormal}
        };
    }
    
    private void Start()
    {
        audioSource.clip = songIntroNormal;
        audioSource.loop = false;
        audioSource.Play();
    }

    private void Update()
    {
        if (audioSource.isPlaying) return;
        SwitchMusicFromIntroToLoop();
        enabled = false;
    }

    private void SwitchMusicFromIntroToLoop()
    {
        audioSource.clip = _switchMusic[_audioPlaying];
        audioSource.Play();
        audioSource.loop = true;
    }

    public void IncreaseTempo() {
        if (_audioPlaying == 1) {
            return;
        }
        var audioSourceTimeBeforeSwitching = audioSource.time;
        audioSource.clip = _increaseTempo[audioSource.clip];
        audioSource.time = audioSourceTimeBeforeSwitching;
        audioSource.Play();
        _audioPlaying = 1;
    }
    
    public void DecreaseTempo() {
        Debug.Log("here1");
        if (_audioPlaying == 0) {
            return;
        }
        var audioSourceTimeBeforeSwitching = audioSource.time;
        audioSource.clip = _decreaseTempo[audioSource.clip];
        audioSource.time = audioSourceTimeBeforeSwitching;
        audioSource.Play();
        _audioPlaying = 0;
    }

}
