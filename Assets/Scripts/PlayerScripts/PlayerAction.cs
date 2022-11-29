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
    protected double PreviousBlink;
    protected bool AbleToBlink;
    protected int InputIndex;
    public int prespawnWarningSeconds;
    public double TimeStamp;
    protected double PerfectMarginOfError;
    protected double NiceMarginOfError;
    protected double AudioTime;

    protected virtual void Start() {
        PerfectMarginOfError = MusicPlayer.Current.perfectMarginOfError;
        NiceMarginOfError = MusicPlayer.Current.niceMarginOfError;
        AbleToBlink = true;
    }

    public abstract void SetTimeStamps(IEnumerable<Note> array);

    // Update is called once per frame
    public abstract void Update();

    protected (bool ableToBlink, double previousBlink) CheckBlink(Color blinkColor, Color otherBlinkColor, double timeStamp, double otherTimeStamp, bool ableToBlink, double previousBlink){
        if(!ableToBlink && AudioTime > previousBlink + blinkCooldown){
                ableToBlink = true;
        }

        if (timeStamp - blinkOffset <= AudioTime && timeStamp > AudioTime){
            Blink(otherTimeStamp == timeStamp ? otherBlinkColor : blinkColor);
            ableToBlink = false;
            previousBlink = AudioTime;
        }
        return (ableToBlink, previousBlink);
    }

    protected int GetAccuracy(double timeStamp, int inputIndex)
    {
        if (Math.Abs(AudioTime - (timeStamp)) < PerfectMarginOfError)
        {
            //Perfect
            Hit();
            print($"Hit on {inputIndex} note - time: {timeStamp} audio time {AudioTime}");
            inputIndex++;
        }
        else if (Math.Abs(AudioTime - (timeStamp)) < NiceMarginOfError)
        {
            //Nice
            Inaccurate();
            print(
                $"Hit inaccurate on {inputIndex} note with {Math.Abs(AudioTime - timeStamp)} delay - time: {timeStamp} audio time {AudioTime}");
        }
        else{
            //Oops
            Miss();
            print($"Missed {inputIndex} note - time: {timeStamp} audio time {AudioTime}");

            if(AudioTime - timeStamp > NiceMarginOfError){
                inputIndex++;
            }
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

        if (curTimeStamp + NiceMarginOfError <= AudioTime)
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