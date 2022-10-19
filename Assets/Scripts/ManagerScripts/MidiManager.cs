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

    public static MidiManager current;
    
    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        var midiFile = MidiFile.Read(Application.dataPath + "/StreamingAssets/mus_Feline-Tipsy_120bpm_arr.mid");
        var trackList = midiFile.GetTrackChunks().ToList();
        
        // change track list number to match the midi file!
        _platformBlink = (trackList[1].GetNotes().ToList())[0].NoteNumber;
        _platformSwitch = (trackList[2].GetNotes().ToList())[0].NoteNumber;
        _playerMeow = (trackList[3].GetNotes().ToList())[0].NoteNumber;
        
        InitializeFilePlayback(midiFile);
        StartPlayback();
    }

    private void OnApplicationQuit()
    {
        if (_playback != null)
        {
            _playback.NotesPlaybackStarted -= OnNotesPlaybackStarted;
            _playback.Dispose();
        }
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

    public ITimeSpan GetPlaybackTime()
    {
        return _playback.GetCurrentTime(TimeSpanType.Metric);
    }

    public void ResumePlayback()
    {
        _playback.Start();
    }
    
    public void PausePlayback()
    {
        _playback.Stop();
    }

    public void AdjustMidiTime(ITimeSpan midiTime)
    {
        _playback.MoveToTime(midiTime);
    }

    public void RestartLevel()
    {
        OnApplicationQuit();
    }
}