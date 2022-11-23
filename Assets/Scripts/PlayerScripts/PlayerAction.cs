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
    protected int InputIndex;
    public int prespawnWarningSeconds;
    protected double TimeStamp;
    protected double MarginOfError;
    protected double AudioTime;

    private void Start() {
        MarginOfError = MusicPlayer.current.marginOfError;
        _ableToBlink = true;
    }

    public abstract void SetTimeStamps(IEnumerable<Note> array);

    // Update is called once per frame
    public abstract void Update();

    protected (bool ableToBlink, double previousBlink) CheckBlink(Color blinkColor, double timeStamp, bool ableToBlink, double previousBlink){
        if(!ableToBlink && AudioTime > previousBlink + blinkCooldown){
                ableToBlink = true;
            }

        if (timeStamp - blinkOffset <= AudioTime && timeStamp > AudioTime){
            Blink(blinkColor);
            ableToBlink = false;
            previousBlink = AudioTime;
        }
        return (ableToBlink, previousBlink);
    }

    protected int GetAccuracy(double timeStamp, int inputIndex)
    {
        if (Math.Abs(AudioTime - (timeStamp)) < MarginOfError)
        {
            Hit();
            print($"Hit on {inputIndex} note - time: {timeStamp} audio time {AudioTime}");
            inputIndex++;
        }
        else
        {
            Inaccurate();
            print(
                $"Hit inaccurate on {inputIndex} note with {Math.Abs(AudioTime - timeStamp)} delay - time: {timeStamp} audio time {AudioTime}");
        }
        return inputIndex;
    }

    protected List<double> AddNoteToTimeStamp(Note curNote, Melanchall.DryWetMidi.MusicTheory.NoteName curNoteRestriction, List<double> curTimeStamps){
        if (curNote.Octave == 1 && curNote.NoteName == curNoteRestriction)
        {
            var metricTimeSpan =
                TimeConverter.ConvertTo<MetricTimeSpan>(curNote.Time, MusicPlayer.MidiFileTest.GetTempoMap());
            var spawnTime = ((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds +
                                (double)metricTimeSpan.Milliseconds / 1000f);

            curTimeStamps.Add(spawnTime - prespawnWarningSeconds);
        }
        return curTimeStamps;
    }

    protected int CheckMiss(int inputIndex, double curTimeStamp) {

        if (curTimeStamp + MarginOfError <= AudioTime)
        {
            Miss();
            print($"Missed {inputIndex} note - time: {curTimeStamp} audio time {AudioTime}");
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

    private void Blink(Color blinkColor)
    {
        PlatformManager.current.InvokeBlink(blinkColor);
    }

    public abstract void TriggerScoreCalculation(InputAction.CallbackContext context);
}