using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSideAction : PlayerAction
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction2;
    public List<double> timeStamps2 = new List<double>();
    private int _inputIndex2;
    private double _timeStamp2;

    private double _curTimeStamp;
    private int _curInputIndex;
    public override void SetTimeStamps(IEnumerable<Note> array)
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
            else if (note.Octave == 1 && note.NoteName == noteRestriction2)
            {
                var metricTimeSpan =
                    TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, MusicPlayer.MidiFileTest.GetTempoMap());
                var spawnTime = ((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds +
                                 (double)metricTimeSpan.Milliseconds / 1000f);

                timeStamps2.Add(spawnTime - prespawnWarningSeconds);
            }
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (Time.time > 5 && !GameManager.current.IsGamePaused() && _inputIndex < timeStamps.Count)
        {
            _timeStamp = timeStamps[_inputIndex];
            _timeStamp2 = timeStamps2[_inputIndex2];
            _marginOfError = MusicPlayer.current.marginOfError;
            _audioTime = MusicPlayer.GetAudioSourceTime() - (MusicPlayer.current.inputDelayInMilliseconds / 1000.0);

            if (_timeStamp + _marginOfError <= _audioTime)
            {
                Miss();
                print($"Missed {_inputIndex} note - time: {_timeStamp} audio time {_audioTime}");
                _inputIndex++;
            }

            if (_timeStamp2 + _marginOfError <= _audioTime)
            {
                Miss();
                print($"Missed {_inputIndex2} note - time: {_timeStamp2} audio time {_audioTime}");
                _inputIndex2++;
            }
        }
    }
    protected void GetAccuracySide(bool left)
    {
        if (left){
            _curTimeStamp = _timeStamp;
            _curInputIndex = _inputIndex;
        }else{
            _curTimeStamp = _timeStamp2;
            _curInputIndex = _inputIndex2;
        }

        if (Math.Abs(_audioTime - (_curTimeStamp)) < _marginOfError)
        {
            Hit();
            print($"Hit on {_curInputIndex} note - time: {_curTimeStamp} audio time {_audioTime}");
            if (left){
                _inputIndex++;
            }else{
                _inputIndex2++;
            }
        }
        else
        {
            Inaccurate();
            print(
                $"Hit inaccurate on {_curInputIndex} note with {Math.Abs(_audioTime - _curTimeStamp)} delay - time: {_curTimeStamp} audio time {_audioTime}");
        }
    }
    public override void TriggerScoreCalculation(InputAction.CallbackContext context)
    {
        if (Time.time > 5 && !GameManager.current.IsGamePaused() && _inputIndex < timeStamps.Count)
        {
            if (context.ReadValue<Vector2>().x < 0){
                GetAccuracySide(true);
            }
            else if (context.ReadValue<Vector2>().x > 0){
                GetAccuracySide(false);
            }
        }
    }

}