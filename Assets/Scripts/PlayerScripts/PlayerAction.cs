using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;
    public KeyCode input;
    public List<double> timeStamps = new List<double>();
    private int _inputIndex;
    public int prespawnWarningSeconds;
    public ScoreManager scoreManager;

    public void SetTimeStamps(IEnumerable<Note> array)
    {
        foreach (var note in array)
        {
            if (note.Octave == 1 && note.NoteName == noteRestriction){
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, MusicPlayer.MidiFileTest.GetTempoMap());
                var spawnTime = ((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
                    
                timeStamps.Add(spawnTime - prespawnWarningSeconds);
            }
        }
    }
    
    // Update is called once per frame
    private void Update()
    {
        if (Time.time > 5 && !GameManager.current.IsGamePaused() && _inputIndex < timeStamps.Count)
        {
            double timeStamp = timeStamps[_inputIndex];
            double marginOfError = MusicPlayer.current.marginOfError;
            double audioTime = MusicPlayer.GetAudioSourceTime() - (MusicPlayer.current.inputDelayInMilliseconds / 1000.0);

            // Only when PreSpawnWarningSeconds > 0
            // if (Math.Abs(audioTime - timeStamp) < marginOfError){
            //     StartCoroutine(ActionWarning());
            // }

            if (Input.GetKeyDown(input))
            {
                if (Math.Abs(audioTime - (timeStamp)) < marginOfError)
                {
                    Hit();
                    print($"Hit on {_inputIndex} note - time: {timeStamp} audio time {audioTime}");
                    _inputIndex++;
                }
                else
                {
                    Inaccurate();
                    print($"Hit inaccurate on {_inputIndex} note with {Math.Abs(audioTime - timeStamp)} delay - time: {timeStamp} audio time {audioTime}");
                }
            }
            if (timeStamp + marginOfError <= audioTime)
            {
                Miss();
                print($"Missed {_inputIndex} note - time: {timeStamp} audio time {audioTime}");
                _inputIndex++;
            }
        }   
    }

    private void Hit()
    {
        scoreManager.Hit();
    }
    private void Miss()
    {
        scoreManager.Miss();
    }
    
    private void Inaccurate()
    {
        scoreManager.Inaccurate();
    }
}