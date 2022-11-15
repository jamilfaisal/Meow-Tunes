using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAction : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;
    public KeyCode input;
    public List<double> timeStamps = new List<double>();
    int _inputIndex = 0;
    public int PrespawnWarningSeconds = 0;
    public ScoreManager scoreManager;
    private double _timeStamp;
    private double _marginOfError;
    private double _audioTime;

    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (var note in array)
        {
            if (note.Octave == 1)
            {
                if (note.NoteName == noteRestriction)
                {
                    var metricTimeSpan =
                        TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, MusicPlayer.MidiFileTest.GetTempoMap());
                    double spawn_time = ((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds +
                                         (double)metricTimeSpan.Milliseconds / 1000f);

                    timeStamps.Add(spawn_time - PrespawnWarningSeconds);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > 5)
        {
            if (_inputIndex < timeStamps.Count)
            { 
                _timeStamp = timeStamps[_inputIndex];
                _marginOfError = MusicPlayer.current.marginOfError;
                _audioTime = MusicPlayer.GetAudioSourceTime() - (MusicPlayer.current.inputDelayInMilliseconds / 1000.0);

                // Only when PreSpawnWarningSeconds > 0
                // if (Math.Abs(audioTime - timeStamp) < marginOfError){
                //     StartCoroutine(ActionWarning());
                // }

                if (Input.GetKeyDown(input))
                {
                    GetAccuracy();
                }
                if (_timeStamp + _marginOfError <= _audioTime)
                {
                    Miss();
                    print($"Missed {_inputIndex} note - time: {_timeStamp} audio time {_audioTime}");
                    _inputIndex++;
                }
            }    
        }   
    }
    

    private void GetAccuracy()
    {
        if (Math.Abs(_audioTime - (_timeStamp)) < _marginOfError)
        {
            Hit();
            print($"Hit on {_inputIndex} note - time: {_timeStamp} audio time {_audioTime}");
            _inputIndex++;
        }
        else
        {
            Inaccurate();
            print($"Hit inaccurate on {_inputIndex} note with {Math.Abs(_audioTime - _timeStamp)} delay - time: {_timeStamp} audio time {_audioTime}");
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

    public void TriggerScoreCalculation(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Time.time > 5)
            {
                if (_inputIndex < timeStamps.Count)
                {
                    GetAccuracy();
                }
            }
        }
    }


}