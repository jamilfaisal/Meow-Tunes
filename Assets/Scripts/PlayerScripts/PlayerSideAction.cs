using Melanchall.DryWetMidi.Interaction;
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
        if (Time.timeSinceLevelLoad > 5 && !GameManager.current.IsGamePaused())
        {
            MarginOfError = MusicPlayer.current.marginOfError;
            AudioTime = MusicPlayer.current.GetAudioSourceTime() - (MusicPlayer.current.inputDelayInMilliseconds / 1000.0);

            (_ableToBlink, _previousBlink) = CheckBlink(blinkColorLeft, TimeStamp, _ableToBlink, _previousBlink);
            (_ableToBlinkRight, _previousBlinkRight) = CheckBlink(blinkColorRight, _timeStampRight, _ableToBlinkRight, _previousBlinkRight);
            
            if (InputIndex < timeStamps.Count){
                TimeStamp = timeStamps[InputIndex];
                InputIndex = CheckMiss(InputIndex, TimeStamp);
            }
            if (_inputIndexRight < timeStampsRight.Count){
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
            if (context.performed && context.ReadValue<Vector2>().x == -1 && InputIndex < timeStamps.Count){
                GetAccuracySide(true);
            }
            else if (context.performed && context.ReadValue<Vector2>().x == 1 && _inputIndexRight < timeStampsRight.Count){
                GetAccuracySide(false);
            }
        }
    }

}