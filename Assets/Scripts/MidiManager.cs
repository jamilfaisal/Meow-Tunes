using System;
using System.Linq;
using System.Text;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Composing;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Multimedia;
using Melanchall.DryWetMidi.Standards;
using UnityEngine;

public class MidiManager : MonoBehaviour
{
    private Playback _playback;

    private int notes_played;

    private SevenBitNumber platform_appear;
    private SevenBitNumber platform_blink;
    private SevenBitNumber platform_move;

    public double speed_up_percentage;
    public double slow_down_percentage;
    public PlatformParent platform;

    public UnityMainThread UnityMainThread;


    private void Start()
    {
        var midiFile = MidiFile.Read("./Assets/Audio/Music/Level_One_Music/mus_Fine-Wine-with-Feline_110bpm_v2.mid");

        var trackList = midiFile.GetTrackChunks().ToList();
        platform_appear = (trackList[1].GetNotes().ToList())[0].NoteNumber;
        platform_blink = (trackList[2].GetNotes().ToList())[0].NoteNumber;
        platform_move = (trackList[5].GetNotes().ToList())[0].NoteNumber;
        
        
        InitializeFilePlayback(midiFile);
        StartPlayback();
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Releasing playback...");

        if (_playback != null)
        {
            _playback.NotesPlaybackStarted -= OnNotesPlaybackStarted;
            _playback.Dispose();
        }

        Debug.Log("Playback released.");
    }

    private void InitializeFilePlayback(MidiFile midiFile)
    {
        Debug.Log("Initializing playback...");

        _playback = midiFile.GetPlayback();
        _playback.Loop = true;
        _playback.NotesPlaybackStarted += OnNotesPlaybackStarted;
       
        Debug.Log("Playback initialized.");
    }

    private void StartPlayback()
    {
        Debug.Log("Starting playback...");
        _playback.Start();
    }

    public void SpeedUp()
    {
        _playback.Speed = speed_up_percentage;
    }

    public void SlowDown()
    {
        _playback.Speed = slow_down_percentage;
    }

    private void OnNotesPlaybackStarted(object sender, NotesEventArgs e)
    {
        foreach (Note note in e.Notes){
            if (platform_appear == note.NoteNumber){
                // Debug.Log("~~Appear~~");
                // Debug.Log(note);
                UnityMainThread.wkr.AddJob(() => {
                    platform.Appear_Disappear();
                });
            }
            if (platform_blink == note.NoteNumber){
                // Debug.Log("~~Blink~~");
                // Debug.Log(note);
                UnityMainThread.wkr.AddJob(() => {
                    platform.Blink();
                });
            }
            if (platform_move == note.NoteNumber){
                // Debug.Log("~~Move~~");
                // Debug.Log(note);
            }
        }
        notes_played += 1;
    }

    // private void Update() {
    //     if(notes_played > 20 && _playback.Speed < 8){
    //         SpeedUp();
    //     }

    //     if(notes_played > 50 && _playback.Speed >= 8){
    //         SlowDown();
    //     }
    // }
}