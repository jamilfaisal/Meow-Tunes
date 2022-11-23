using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSideAction : PlayerAction
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestrictionRight;
    public List<double> timeStampsRight = new List<double>();
    public int _inputIndexRight;
    private double _timeStampRight;
    protected double _previousBlinkRight;
    protected bool _ableToBlinkRight;
    private Color blinkColorLeft;
    private Color blinkColorRight;


    private void Start() {
        blinkColorLeft = new Color(1f, 0.83f, 0f); //Yellow
        blinkColorRight = new Color(0.47f, 0.31f, 0.66f); //Purple
    }
    public override void SetTimeStamps(IEnumerable<Note> array)
    {
        foreach (var note in array)
        {
            timeStamps = AddNoteToTimeStamp(note, noteRestriction, timeStamps);
            timeStampsRight = AddNoteToTimeStamp(note, noteRestrictionRight, timeStampsRight);
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        if (Time.time > 5 && !GameManager.current.IsGamePaused())
        {
            _marginOfError = MusicPlayer.current.marginOfError;
            _audioTime = MusicPlayer.GetAudioSourceTime() - (MusicPlayer.current.inputDelayInMilliseconds / 1000.0);

            (_ableToBlink, _previousBlink) = CheckBlink(blinkColorLeft, _timeStamp, _ableToBlink, _previousBlink);
            (_ableToBlinkRight, _previousBlinkRight) = CheckBlink(blinkColorRight, _timeStampRight, _ableToBlinkRight, _previousBlinkRight);
            
            if (_inputIndex < timeStamps.Count){
                _timeStamp = timeStamps[_inputIndex];
                _inputIndex = CheckMiss(_inputIndex, _timeStamp);
            }
            if (_inputIndexRight < timeStampsRight.Count){
                _timeStampRight = timeStampsRight[_inputIndexRight];
                _inputIndexRight = CheckMiss(_inputIndexRight, _timeStampRight);
            }
        }
    }
    
    protected void GetAccuracySide(bool left)
    {
        if (left){
            _inputIndex = GetAccuracy( _timeStamp, _inputIndex);
        }else{
            _inputIndexRight = GetAccuracy( _timeStampRight, _inputIndexRight);
        }
    }
    
    public override void TriggerScoreCalculation(InputAction.CallbackContext context)
    {
        if (Time.time > 5 && !GameManager.current.IsGamePaused())
        {
            if (context.performed && context.ReadValue<Vector2>().x == -1 && _inputIndex < timeStamps.Count){
                GetAccuracySide(true);
            }
            else if (context.performed && context.ReadValue<Vector2>().x == 1 && _inputIndexRight < timeStampsRight.Count){
                GetAccuracySide(false);
            }
        }
    }

}