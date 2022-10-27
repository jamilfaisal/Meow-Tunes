using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Composing;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Multimedia;
using Melanchall.DryWetMidi.Standards;
using System.IO;
using UnityEngine.Networking;
using System;

public class Conductor : MonoBehaviour
{
    public static Conductor current;
    
    public AudioSource audioSource;
    public AudioClip songIntroNormal;
    public AudioClip songLoopNormal;

    public static MidiFile midiFile_test;
    public float noteTime;

    public Lane[] lanes;
    public double marginOfError;
    private void Awake()
    {
        current = this;
    }
    
    private void Start()
    {
        midiFile_test = null;
        if (Application.platform is RuntimePlatform.WindowsPlayer or RuntimePlatform.OSXEditor or RuntimePlatform.WindowsEditor)
            midiFile_test = MidiFile.Read(Application.dataPath + "/StreamingAssets/MIDI_test.mid");
        if (Application.platform == RuntimePlatform.OSXPlayer)
            midiFile_test = MidiFile.Read(Application.dataPath + "/Resources/Data/StreamingAssets/MIDI_test.mid");
        
        var notes = midiFile_test.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        // Debug.Log(notes.Count);
        notes.CopyTo(array, 0);
        foreach (var lane in lanes){
            lane.SetTimeStamps(array);
            // Debug.Log(lane.timeStamps.Count);
        }

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
