using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer current;
    
    public AudioSource audioSource;
    public AudioClip songIntroNormal;

    // public float songBpm = 120;
    // public float secPerBeat;
    // public float songPosition;
    // public float songPositionInBeats;
    // public float dspSongTime;

    public static MidiFile MidiFileTest;
    public float noteTime;

    public Lane[] lanes;
    public PlayerAction[] playerActions;
    public double marginOfError = 0.3;
    public int inputDelayInMilliseconds; //Delay Time for when the music starts

    private bool _audioPlayed;
    private void Awake()
    {
        current = this;
    }
    
    private void Start()
    {
        MidiFileTest = null;
        if (Application.platform is RuntimePlatform.WindowsPlayer or RuntimePlatform.OSXEditor or RuntimePlatform.WindowsEditor)
            MidiFileTest = MidiFile.Read(Application.dataPath + "/StreamingAssets/full_arrangement_v14.mid");
        if (Application.platform == RuntimePlatform.OSXPlayer)
            MidiFileTest = MidiFile.Read(Application.dataPath + "/Resources/Data/StreamingAssets/full_arrangement_v14.mid");
        
        var notes = MidiFileTest.GetNotes();
        var array = new Note[notes.Count];
        // Debug.Log(notes.Count);
        notes.CopyTo(array, 0);
        foreach (var lane in lanes){
            lane.SpawnPlatformsAndFishTreats(array);
            // Debug.Log(lane.timeStamps.Count);
        }
        foreach (var playerAction in playerActions){
            playerAction.SetTimeStamps(array);
        }

        audioSource.clip = songIntroNormal;
        audioSource.loop = false;
        
        // secPerBeat = 60f / songBpm;
        // dspSongTime = (float)AudioSettings.dspTime;
    }

    private void Update()
    {
        // songPosition = (float)(AudioSettings.dspTime - dspSongTime);
        // songPositionInBeats = songPosition / secPerBeat;
        if (Time.time > 5 && !_audioPlayed)
        {
            Debug.Log("played!");
            audioSource.Play();
            _audioPlayed = true;
        }
    }

    // private void SwitchMusicFromIntroToLoop()
    // {
    //     if (songLoopNormal != null)
    //     {
    //         audioSource.clip = songLoopNormal;
    //     }
    //     audioSource.Play();
    //     audioSource.loop = true;
    // }

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
