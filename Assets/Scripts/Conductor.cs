using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip songIntroNormal;
    public AudioClip songLoopNormal;
    public AudioClip songIntroFast;
    public AudioClip songLoopFast;

    void Start()
    {
        audioSource.clip = songIntroNormal;
        audioSource.loop = false;
        audioSource.Play();
    }

    private void Update() {
        if (!audioSource.isPlaying) {
            if (audioSource.clip == songIntroNormal) {
                audioSource.clip = songLoopNormal;
            } else if (audioSource.clip == songIntroFast) {
                audioSource.clip = songLoopFast;
            }
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void increaseTempo() {
        if (audioSource.clip == songIntroFast || audioSource.clip == songLoopFast) {
            return;
        }
        float audioSourceTimeBeforeSwitching = audioSource.time;
        if (audioSource.clip == songIntroNormal) {
            audioSource.clip = songIntroFast;
        }
        if (audioSource.clip == songLoopNormal) {
            audioSource.clip = songLoopFast;
        }
        audioSource.time = audioSourceTimeBeforeSwitching;
    }

    public void decreaseTempo() {
        if (audioSource.clip == songIntroNormal || audioSource.clip == songLoopNormal) {
            return;
        }
        float audioSourceTimeBeforeSwitching = audioSource.time;
        if (audioSource.clip == songIntroFast) {
            audioSource.clip = songIntroNormal;
        }
        if (audioSource.clip == songLoopFast) {
            audioSource.clip = songLoopNormal;
        }
        audioSource.time = audioSourceTimeBeforeSwitching;
    }
}
