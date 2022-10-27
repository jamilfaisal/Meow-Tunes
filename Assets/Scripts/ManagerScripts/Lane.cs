using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;
    // public KeyCode input;
    public GameObject PlatformPrefab;
    public List<Platform> platforms = new List<Platform>();
    public List<Tuple<int, double>> timeStamps = new List<Tuple<int, double>>();


    public float PlatformSpacing = 100;
    public double SpawningHeadstartTime = 1;

    private float x = 0F;
    private float y, z;

    int spawnIndex = 0; //pre-spawned platforms currently does not increase spawnIndex
    // int inputIndex = 0;

    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (var note in array)
        {
            if (note.NoteName == noteRestriction)
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, Conductor.midiFile_test.GetTempoMap());

                double spawn_time = ((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);

                if ((spawn_time-SpawningHeadstartTime) < 0){
                    /* Pre-spawning platforms before game starts */
                    SpawnPlatform((int)(note.Octave), (float)spawn_time);
                }
                else {
                    timeStamps.Add(new Tuple<int, double>(note.Octave, spawn_time));
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (spawnIndex < timeStamps.Count)
        {
            if (Conductor.GetAudioSourceTime() >= (timeStamps[spawnIndex].Item2-SpawningHeadstartTime) - Conductor.current.noteTime)
            {
                /*spawning platforms based on converted time from midifile*/
                SpawnPlatform((int)(timeStamps[spawnIndex].Item1), (float)(timeStamps[spawnIndex].Item2));
                spawnIndex++;
            }
        }

        // if (inputIndex < timeStamps.Count)
        // {
        //     double timeStamp = timeStamps[inputIndex];
        //     double marginOfError = SongManager.Instance.marginOfError;
        //     double audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelayInMilliseconds / 1000.0);

        //     if (Input.GetKeyDown(input))
        //     {
        //         if (Math.Abs(audioTime - timeStamp) < marginOfError)
        //         {
        //             Hit();
        //             print($"Hit on {inputIndex} note");
        //             Destroy(notes[inputIndex].gameObject);
        //             inputIndex++;
        //         }
        //         else
        //         {
        //             print($"Hit inaccurate on {inputIndex} note with {Math.Abs(audioTime - timeStamp)} delay");
        //         }
        //     }
        //     if (timeStamp + marginOfError <= audioTime)
        //     {
        //         Miss();
        //         print($"Missed {inputIndex} note");
        //         inputIndex++;
        //     }
        // }       
    
    }

    private void SpawnPlatform(int octave, float spawn_time)
    //Improvement: check note velocity to spawn different types of platform
    {
        y = (octave - 2) * 3.0F;
        z = spawn_time * PlatformSpacing;
        Vector3 position = new Vector3(x, y, z);
        var new_platform = Instantiate(PlatformPrefab);
        new_platform.transform.parent = transform;
        new_platform.transform.localPosition = position;
        new_platform.transform.rotation = transform.rotation;
        platforms.Add(new_platform.GetComponent<Platform>());
    }
    // private void Hit()
    // {
    //     ScoreManager.Hit();
    // }
    // private void Miss()
    // {
    //     ScoreManager.Miss();
    // }
}