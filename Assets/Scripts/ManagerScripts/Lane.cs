using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName PlatformNote;
    public Melanchall.DryWetMidi.MusicTheory.NoteName FishTreatNote;
    // public KeyCode input;
    public GameObject PlatformPrefab;
    public GameObject FishTreatPrefab;
    public GameObject CheckpointPrefab;
    public List<Platform> platforms = new List<Platform>();
    public List<FishHit> fishtreats = new List<FishHit>();
    // public List<Checkpoint> checkpoints = new List<Checkpoint>();
    public List<Tuple<int, Melanchall.DryWetMidi.Common.SevenBitNumber, double>> platformTimeStamps = new List<Tuple<int, Melanchall.DryWetMidi.Common.SevenBitNumber, double>>(); //Octave, Velocity, SpawnTime
    public List<Tuple<int, double>> fishTreatTimeStamps = new List<Tuple<int, double>>();


    public float SpacingSize = 2F; //based on the size of the current neutral platform
    public double SpawningHeadstartTime = 5;

    private float x = 0F;
    private float y, z;

    int platformSpawnIndex = 0; //pre-spawned platforms currently does not increase spawnIndex
    int fishTreatSpawnIndex = 0;
    // int inputIndex = 0;

    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (var note in array)
        {
            if (note.NoteName == PlatformNote)
            {
                //Octave 1 is for player input
                if (note.Octave != 1){
                    var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, Conductor.midiFile_test.GetTempoMap());

                    double spawn_time = ((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);

                    if ((spawn_time-SpawningHeadstartTime) < 0){
                        /* Pre-spawning platforms before game starts */
                        SpawnPlatform((int)(note.Octave), note.Velocity, (float)spawn_time);
                    }
                    else {
                        platformTimeStamps.Add(new Tuple<int, Melanchall.DryWetMidi.Common.SevenBitNumber, double>(note.Octave, note.Velocity, spawn_time));
                    }
                }
            }
            if (note.NoteName == FishTreatNote)
            {
                //Octave 1 is for player input
                if (note.Octave != 1){
                    var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, Conductor.midiFile_test.GetTempoMap());

                    double spawn_time = ((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);

                    if ((spawn_time-SpawningHeadstartTime) < 0){
                        /* Pre-spawning platforms before game starts */
                        SpawnFishTreat((int)(note.Octave), (float)spawn_time);
                    }
                    else {
                        fishTreatTimeStamps.Add(new Tuple<int, double>(note.Octave, spawn_time));
                    }
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (platformSpawnIndex < platformTimeStamps.Count)
        {
            if (Conductor.GetAudioSourceTime() >= (platformTimeStamps[platformSpawnIndex].Item3-SpawningHeadstartTime) - Conductor.current.noteTime)
            {
                /*spawning platforms based on converted time from midifile*/
                SpawnPlatform((int)(platformTimeStamps[platformSpawnIndex].Item1), platformTimeStamps[platformSpawnIndex].Item2, (float)(platformTimeStamps[platformSpawnIndex].Item3));
                platformSpawnIndex++;
            }
        }

        if (fishTreatSpawnIndex < fishTreatTimeStamps.Count)
        {
            if (Conductor.GetAudioSourceTime() >= (fishTreatTimeStamps[fishTreatSpawnIndex].Item2-SpawningHeadstartTime) - Conductor.current.noteTime)
            {
                /*spawning platforms based on converted time from midifile*/
                SpawnFishTreat((int)(fishTreatTimeStamps[fishTreatSpawnIndex].Item1), (float)(fishTreatTimeStamps[fishTreatSpawnIndex].Item2));
                fishTreatSpawnIndex++;
            }
        }  
    }

    private void SpawnPlatform(int octave, Melanchall.DryWetMidi.Common.SevenBitNumber velocity, float spawn_time)
    //Improvement: check note velocity to spawn different types of platform
    {
        // Debug.Log("spawned");
        var new_platform = Instantiate(PlatformPrefab);
        y = (octave - 2) * 1.5F;
        z = (spawn_time / 0.25F) * SpacingSize;
        Vector3 position = new Vector3(x, y, z);
        // Debug.Log(spawn_time);
        new_platform.transform.parent = transform;
        new_platform.transform.localPosition = position;
        new_platform.transform.rotation = transform.rotation;
        platforms.Add(new_platform.GetComponent<Platform>());

        if (velocity == (Melanchall.DryWetMidi.Common.SevenBitNumber)83){
            //Checkpoint
            var new_checkpoint = Instantiate(CheckpointPrefab);
            y = (octave - 2) * 1.5F - 3;
            z = (spawn_time / 0.25F) * SpacingSize;
            position = new Vector3(1, y, z);
            new_checkpoint.transform.parent = transform;
            new_checkpoint.transform.localPosition = position;
            new_checkpoint.transform.rotation = transform.rotation;
            // checkpoints.Add(new_checkpoint.GetComponent<Checkpoint>());
        }
    }

    private void SpawnFishTreat(int octave, float spawn_time)
    {
        // Debug.Log("spawned");
        var new_fishtreat = Instantiate(FishTreatPrefab);
        y = (octave - 2) * 1.5F + 3;
        z = (spawn_time / 0.25F) * SpacingSize - 1;
        Vector3 position = new Vector3(x, y, z);
        // Debug.Log(spawn_time);
        new_fishtreat.transform.parent = transform;
        new_fishtreat.transform.localPosition = position;
        new_fishtreat.transform.rotation = transform.rotation;
        fishtreats.Add(new_fishtreat.GetComponent<FishHit>());
    }
}