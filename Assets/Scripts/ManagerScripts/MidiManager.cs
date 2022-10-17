using System.Linq;
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

    private int _notesPlayed;

    private SevenBitNumber _platformSwitch;
    private SevenBitNumber _platformBlink;
    private SevenBitNumber _playerMeow;

    // public PlatformParent red_platforms;
    // public PlatformParent green_platforms;

    // public UnityMainThread UnityMainThread;


    private void Start()
    {
        var midiFile = MidiFile.Read("./Assets/Audio/Music/Level_One_Music/mus_Fine-Wine-with-Feline_110bpm_v2.mid");

        var trackList = midiFile.GetTrackChunks().ToList();
        
        // change track list number to match the new midi file!
        _platformSwitch = (trackList[1].GetNotes().ToList())[0].NoteNumber;
        _platformBlink = (trackList[2].GetNotes().ToList())[0].NoteNumber;
        _playerMeow = (trackList[3].GetNotes().ToList())[0].NoteNumber;
        
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

    private void OnNotesPlaybackStarted(object sender, NotesEventArgs e)
    {
        foreach (var note in e.Notes){
            if (_platformSwitch == note.NoteNumber){
                UnityMainThread.wkr.AddJob(() => {
                    PlatformManager.current.InvokeSwitch();
                });
            }
            if (_platformBlink == note.NoteNumber){
                UnityMainThread.wkr.AddJob(() => {
                    PlatformManager.current.InvokeBlink();
                });
            }
        }
        _notesPlayed += 1;
    }
}