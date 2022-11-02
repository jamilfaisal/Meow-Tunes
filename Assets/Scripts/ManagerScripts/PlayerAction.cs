using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;
    public KeyCode input;
    public List<double> timeStamps = new List<double>();
    int inputIndex = 0;
    public int PrespawnWarningSeconds = 0;
    public ScoreManager scoreManager;

    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (var note in array)
        {
            if (note.Octave == 1){
                if (note.NoteName == noteRestriction)
                {
                    var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, Conductor.midiFile_test.GetTempoMap());
                    double spawn_time = ((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
                    
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
            if (inputIndex < timeStamps.Count)
            {
                double timeStamp = timeStamps[inputIndex];
                double marginOfError = Conductor.current.marginOfError;
                double audioTime = Conductor.GetAudioSourceTime() - (Conductor.current.inputDelayInMilliseconds / 1000.0);

                // Only when PreSpawnWarningSeconds > 0
                // if (Math.Abs(audioTime - timeStamp) < marginOfError){
                //     StartCoroutine(ActionWarning());
                // }

                if (Input.GetKeyDown(input))
                {
                    if (Math.Abs(audioTime - (timeStamp)) < marginOfError)
                    {
                        Hit();
                        print($"Hit on {inputIndex} note - time: {timeStamp} audio time {audioTime}");
                        inputIndex++;
                    }
                    else
                    {
                        Inaccurate();
                        print($"Hit inaccurate on {inputIndex} note with {Math.Abs(audioTime - timeStamp)} delay - time: {timeStamp} audio time {audioTime}");
                    }
                }
                if (timeStamp + marginOfError <= audioTime)
                {
                    Miss();
                    print($"Missed {inputIndex} note - time: {timeStamp} audio time {audioTime}");
                    inputIndex++;
                }
            }    
        }   
    }

    // IEnumerator ActionWarning()
    // {

    // }

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