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
    public double blinkOffset;
    public double blinkCooldown;
    private double _previousBlink;
    private bool _ableToBlink;
    private int _inputIndex;
    public int prespawnWarningSeconds;
    private double _timeStamp;
    private double _marginOfError;
    private double _audioTime;

    private void Start() {
        _ableToBlink = true;
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
            _marginOfError = MusicPlayer.current.marginOfError;
            _audioTime = MusicPlayer.GetAudioSourceTime() - (MusicPlayer.current.inputDelayInMilliseconds / 1000.0);

            // if(!_ableToBlink && _audioTime > _previousBlink + blinkCooldown){
            //     print("Can Blink!");
            //     _ableToBlink = true;
            // }

            if (_timeStamp - blinkOffset <= _audioTime && _timeStamp > _audioTime){
                // Blink();
                // _ableToBlink = false;
                // _previousBlink = _audioTime;
                print("blink");
            }

            if (Input.GetKeyDown(input))
            {
                GetAccuracy();
            }

            if (_timeStamp + _marginOfError <= _audioTime)
            {
                Miss();
                // print($"Missed {_inputIndex} note - time: {_timeStamp} audio time {_audioTime}");
                _inputIndex++;
            }
        }
    }


    private void GetAccuracy()
    {
        if (Math.Abs(_audioTime - (_timeStamp)) < _marginOfError)
        {
            Hit();
            // print($"Hit on {_inputIndex} note - time: {_timeStamp} audio time {_audioTime}");
            _inputIndex++;
        }
        else
        {
            Inaccurate();
            // print(
            //     $"Hit inaccurate on {_inputIndex} note with {Math.Abs(_audioTime - _timeStamp)} delay - time: {_timeStamp} audio time {_audioTime}");
        }
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

    private void Blink()
    {
        PlatformManager.current.InvokeBlink();
    }

    public void TriggerScoreCalculation(InputAction.CallbackContext context)
    {
        if (context.performed && Time.time > 5 && !GameManager.current.IsGamePaused() && _inputIndex < timeStamps.Count)
        {
            GetAccuracy();
        }
    }
}