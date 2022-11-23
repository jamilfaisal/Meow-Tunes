using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SingleButtonAction : PlayerAction
{
    private Color blinkColor;

    private void Start() {
        blinkColor = new Color(0.41f, 0.63f, 0.13f); //Green
    }
    public override void SetTimeStamps(IEnumerable<Note> array)
    {
        foreach (var note in array)
        {
            timeStamps = AddNoteToTimeStamp(note, noteRestriction, timeStamps);
        }
    }

    // Update is called once per frame
    public override void Update()
    {   
        if (Time.time > 5 && !GameManager.current.IsGamePaused() && _inputIndex < timeStamps.Count)
        {
            _marginOfError = MusicPlayer.current.marginOfError;
            _audioTime = MusicPlayer.GetAudioSourceTime() - (MusicPlayer.current.inputDelayInMilliseconds / 1000.0);
            _timeStamp = timeStamps[_inputIndex];

            (_ableToBlink, _previousBlink) = CheckBlink(blinkColor, _timeStamp, _ableToBlink, _previousBlink);
            
            _inputIndex = CheckMiss(_inputIndex, _timeStamp);
        }
    }
    public override void TriggerScoreCalculation(InputAction.CallbackContext context)
    {
        if (context.performed && Time.time > 5 && !GameManager.current.IsGamePaused() && _inputIndex < timeStamps.Count)
        {
            _inputIndex = GetAccuracy(_timeStamp, _inputIndex);
        }
    }
}