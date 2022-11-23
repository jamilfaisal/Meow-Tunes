using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerAction : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;
    public List<double> timeStamps = new List<double>();
    public double blinkOffset;
    public double blinkCooldown;
    protected double _previousBlink;
    protected bool _ableToBlink;
    protected int _inputIndex;
    public int prespawnWarningSeconds;
    protected double _timeStamp;
    protected double _marginOfError;
    protected double _audioTime;

    private void Start() {
        _ableToBlink = true;
    }

    public abstract void SetTimeStamps(IEnumerable<Note> array);

    // Update is called once per frame
    public abstract void Update(){
        if(!_ableToBlink && _audioTime > _previousBlink + blinkCooldown){
                _ableToBlink = true;
            }

        if (_timeStamp - blinkOffset <= _audioTime && _timeStamp > _audioTime){
            Blink();
            _ableToBlink = false;
            _previousBlink = _audioTime;
        }
    }

    protected int GetAccuracy(double timeStamp, int inputIndex)
    {   
        if (Math.Abs(_audioTime - (timeStamp)) < _marginOfError)
        {
            Hit();
            print($"Hit on {inputIndex} note - time: {timeStamp} audio time {_audioTime}");
            inputIndex++;
        }
        else
        {
            Inaccurate();
            print(
                $"Hit inaccurate on {inputIndex} note with {Math.Abs(_audioTime - timeStamp)} delay - time: {timeStamp} audio time {_audioTime}");
        }
        return inputIndex;
    }

    protected List<double> AddNoteToTimeStamp(Note cur_note, Melanchall.DryWetMidi.MusicTheory.NoteName cur_noteRestriction, List<double> cur_timeStamps){
        if (cur_note.Octave == 1 && cur_note.NoteName == cur_noteRestriction)
        {
            var metricTimeSpan =
                TimeConverter.ConvertTo<MetricTimeSpan>(cur_note.Time, MusicPlayer.MidiFileTest.GetTempoMap());
            var spawnTime = ((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds +
                                (double)metricTimeSpan.Milliseconds / 1000f);

            cur_timeStamps.Add(spawnTime - prespawnWarningSeconds);
        }
        return cur_timeStamps;
    }

    protected int CheckMiss(int inputIndex, double cur_timeStamp){

            if (cur_timeStamp + _marginOfError <= _audioTime)
            {
                Miss();
                print($"Missed {inputIndex} note - time: {cur_timeStamp} audio time {_audioTime}");
                inputIndex++;
            }
        return inputIndex;
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

    public abstract void TriggerScoreCalculation(InputAction.CallbackContext context);
}