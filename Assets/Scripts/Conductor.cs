using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    public AudioSource songIntro;
    public AudioSource songLoop;

    private Dictionary<AudioSource, bool> _pauseStates = new Dictionary<AudioSource, bool>();



    void Start()
    {
        _pauseStates.Add(songIntro, false);
        _pauseStates.Add(songLoop, false);
        
        songIntro.Play();
        // Play the Loop version after the intro version is done
        songLoop.PlayDelayed(songIntro.clip.length);
    }

    public void Pause()
    {
        foreach (AudioSource audio in _pauseStates.Keys.ToList())
        {
            _pauseStates[audio] = audio.isPlaying;
            audio.Pause();
        }
    }
    
    public void Resume()
    {
        foreach (AudioSource audio in _pauseStates.Keys.ToList())
        {
            if (_pauseStates[audio])
            {
                audio.Play();
            }
    
            _pauseStates[audio] = false;
        }
    }
}
