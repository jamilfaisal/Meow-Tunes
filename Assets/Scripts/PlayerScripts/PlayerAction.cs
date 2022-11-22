using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAction : MonoBehaviour
{
    public static PlayerAction Current;

    private void Awake()
    {
        Current = this;
    }

    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;
    public KeyCode input;
    public List<double> timeStamps = new List<double>();
    private int _inputIndex;
    public int prespawnWarningSeconds;
    private double _timeStamp;
    private double _marginOfError;
    private double _audioTime;

    private void Start()
    {
        _marginOfError = MusicPlayer.current.marginOfError;
    }

    public void SetTimeStamps(IEnumerable<Note> array)
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
    private void Update()
    {
        if (Time.time > 5 && !GameManager.current.IsGamePaused() && _inputIndex < timeStamps.Count)
        {
            _timeStamp = timeStamps[_inputIndex];
            _audioTime = MusicPlayer.current.GetAudioSourceTime() - (MusicPlayer.current.inputDelayInMilliseconds / 1000.0);

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
            print(
                $"Hit inaccurate on {_inputIndex} note with {Math.Abs(_audioTime - _timeStamp)} delay - time: {_timeStamp} audio time {_audioTime}");
        }
    }
    
    public double GetNextTimestamp(int index)
    {
        if (index < timeStamps.Count)
        {
            return timeStamps[index];
        }
        return 999;
    }

    private void Hit()
    {
        ScoreManager.current.Hit();
    }

    private void Miss()
    {
        ScoreManager.current.Miss();
    }

    private void Inaccurate()
    {
        ScoreManager.current.Inaccurate();
    }

    public void TriggerScoreCalculation(InputAction.CallbackContext context)
    {
        if (context.performed && Time.time > 5 && !GameManager.current.IsGamePaused() && _inputIndex < timeStamps.Count)
        {
            GetAccuracy();
        }
    }
}