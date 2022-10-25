using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using UnityEngine.Networking;
using System;

public class Conductor : MonoBehaviour
{
    public static Conductor current;
    
    public AudioSource audioSource;
    public AudioClip songIntroNormal;
    public AudioClip songLoopNormal;

    public static MidiFile midiFile;
    public float noteTime;

    public Lane[] lanes;
    public double marginOfError;
    private void Awake()
    {
        current = this;
    }
    
    private void Start()
    {
        midiFile = null;
        if (Application.platform is RuntimePlatform.WindowsPlayer or RuntimePlatform.OSXEditor or RuntimePlatform.WindowsEditor)
            midiFile = MidiFile.Read(Application.dataPath + "/StreamingAssets/MIDI_test.mid");
        if (Application.platform == RuntimePlatform.OSXPlayer)
            midiFile = MidiFile.Read(Application.dataPath + "/Resources/Data/StreamingAssets/MIDI_test.mid");

        var notes = midiFile.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(array, 0);
        foreach (var lane in lanes) lane.SetTimeStamps(array);

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
        if (songLoopNormal != null)
        {
            audioSource.clip = songLoopNormal;
        }
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

    public static double GetAudioSourceTime()
    {
        return (double)current.audioSource.timeSamples / current.audioSource.clip.frequency;
    }
}
