using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAction : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;
    // public KeyCode input;
    public List<double> timeStamps = new List<double>();
    protected int _inputIndex;
    public int prespawnWarningSeconds;
    protected double _timeStamp;
    protected double _marginOfError;
    protected double _audioTime;

    public virtual void SetTimeStamps(IEnumerable<Note> array)
    {
        foreach (var note in array)
        {
            if (note.Octave == 1 && note.NoteName == noteRestriction)
            {
                var metricTimeSpan =
                    TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, MusicPlayer.MidiFileTest.GetTempoMap());
                var spawnTime = ((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds +
                                 (double)metricTimeSpan.Milliseconds / 1000f);

                timeStamps.Add(spawnTime - prespawnWarningSeconds);
            }
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (Time.time > 5 && !GameManager.current.IsGamePaused() && _inputIndex < timeStamps.Count)
        {
            _timeStamp = timeStamps[_inputIndex];
            _marginOfError = MusicPlayer.current.marginOfError;
            _audioTime = MusicPlayer.GetAudioSourceTime() - (MusicPlayer.current.inputDelayInMilliseconds / 1000.0);

            // if (Input.GetKeyDown(input))
            // {
            //     GetAccuracy();
            // }

            if (_timeStamp + _marginOfError <= _audioTime)
            {
                Miss();
                print($"Missed {_inputIndex} note - time: {_timeStamp} audio time {_audioTime}");
                _inputIndex++;
            }
        }
    }


    protected void GetAccuracy()
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
            print(
                $"Hit inaccurate on {_inputIndex} note with {Math.Abs(_audioTime - _timeStamp)} delay - time: {_timeStamp} audio time {_audioTime}");
        }
    }

    protected void Hit()
    {
        ScoreManager.current.Hit();
    }

    protected void Miss()
    {
        ScoreManager.current.Miss();
    }

    protected void Inaccurate()
    {
        ScoreManager.current.Inaccurate();
    }

    public virtual void TriggerScoreCalculation(InputAction.CallbackContext context)
    {
        if (context.performed && Time.time > 5 && !GameManager.current.IsGamePaused() && _inputIndex < timeStamps.Count)
        {
            GetAccuracy();
        }
    }
}