using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName platformNote;
    public Melanchall.DryWetMidi.MusicTheory.NoteName fishTreatNote;
    public GameObject platformPrefab;
    public GameObject fishTreatPrefab;
    public GameObject checkpointPrefab;
    public List<Platform> platforms = new List<Platform>();
    public List<FishHit> fishtreats = new List<FishHit>();
    
    public float spacingSize = 2F; //based on the size of the current neutral platform

    private const float X = 0F;
    private float _y, _z;

    public void SpawnPlatformsAndFishTreats(IEnumerable<Note> array)
    {
        foreach (var note in array)
        {
            //Octave 1 is for player input
            if (note.Octave == 1) continue;
            
            var metricTimeSpan =
                TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, MusicPlayer.MidiFileTest.GetTempoMap());

            var spawnTime = ((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds +
                             (double)metricTimeSpan.Milliseconds / 1000f);
            if (note.NoteName == platformNote) {
                /* Pre-spawning platforms before game starts */
                SpawnPlatform(note.Octave, note.Velocity, (float)spawnTime);
            }
            if (note.NoteName == fishTreatNote)
            {
                ScoreManager.current.maximumFishScore += 1;
                SpawnFishTreat(note.Octave, (float)spawnTime);
            }
        }
    }

    private void SpawnPlatform(int octave, Melanchall.DryWetMidi.Common.SevenBitNumber velocity, float spawnTime)
    //TODO: Check note velocity to spawn different types of platform
    {
        var newPlatform = Instantiate(platformPrefab, transform, true);
        _y = (octave - 2) * 2F;
        _z = (spawnTime / 0.25F) * spacingSize;
        var position = new Vector3(X, _y, _z);
        newPlatform.transform.localPosition = position;
        newPlatform.transform.rotation = transform.rotation;

        Color alteredColor = new Color();
        alteredColor.r = newPlatform.GetComponent<Renderer>().material.color.r;
        alteredColor.g = newPlatform.GetComponent<Renderer>().material.color.g;
        alteredColor.b = newPlatform.GetComponent<Renderer>().material.color.b + (_y/50);

        newPlatform.GetComponent<Renderer>().material.color = alteredColor;

        platforms.Add(newPlatform.GetComponent<Platform>());

        if (velocity == (Melanchall.DryWetMidi.Common.SevenBitNumber)83){
            //Checkpoint
            var newCheckpoint = Instantiate(checkpointPrefab, transform, true);
            _y = (octave - 2) * 2F - 3;
            _z = (spawnTime / 0.25F) * spacingSize;
            position = new Vector3(1, _y, _z);
            newCheckpoint.transform.localPosition = position;
            newCheckpoint.transform.rotation = transform.rotation;
        }
    }

    private void SpawnFishTreat(int octave, float spawnTime)
    {
        // Debug.Log("spawned");
        var newFishtreat = Instantiate(fishTreatPrefab, transform, true);
        _y = (octave - 2) * 2F + 3;
        _z = (spawnTime / 0.25F) * spacingSize - 1;
        var position = new Vector3(X, _y, _z);
        // Debug.Log(spawn_time);
        newFishtreat.transform.localPosition = position;
        newFishtreat.transform.rotation = transform.rotation;
        fishtreats.Add(newFishtreat.GetComponent<FishHit>());
    }
}