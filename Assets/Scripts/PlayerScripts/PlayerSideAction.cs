using Melanchall.DryWetMidi.Interaction;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSideAction : PlayerAction
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestrictionRight;
    public List<double> timeStampsRight = new List<double>();
    private int _inputIndexRight;
    private double _timeStampRight;

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
        if (Time.timeSinceLevelLoad > 5 && !GameManager.current.IsGamePaused())
        {
            MarginOfError = MusicPlayer.current.marginOfError;
            AudioTime = MusicPlayer.current.GetAudioSourceTime() - (MusicPlayer.current.inputDelayInMilliseconds / 1000.0);
            if (InputIndex < timeStamps.Count){
                TimeStamp = timeStamps[InputIndex];
                InputIndex = CheckMiss(InputIndex, TimeStamp);
            }
            else if (_inputIndexRight < timeStampsRight.Count){
                _timeStampRight = timeStampsRight[_inputIndexRight];
                _inputIndexRight = CheckMiss(_inputIndexRight, _timeStampRight);
            }
        }
    }

    private void GetAccuracySide(bool left)
    {
        if (left){
            InputIndex = GetAccuracy( TimeStamp, InputIndex);
        }else{
            _inputIndexRight = GetAccuracy( _timeStampRight, _inputIndexRight);
        }
    }
    
    public override void TriggerScoreCalculation(InputAction.CallbackContext context)
    {
        if (Time.timeSinceLevelLoad > 5 && !GameManager.current.IsGamePaused())
        {
            if (context.ReadValue<Vector2>().x < 0 && InputIndex < timeStamps.Count){
                GetAccuracySide(true);
            }
            else if (context.ReadValue<Vector2>().x > 0 && _inputIndexRight < timeStampsRight.Count){
                GetAccuracySide(false);
            }
        }
    }

}