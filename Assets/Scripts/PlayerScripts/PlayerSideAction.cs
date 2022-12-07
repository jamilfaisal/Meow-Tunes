using Melanchall.DryWetMidi.Interaction;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSideAction : PlayerAction
{
    public static PlayerSideAction Current;

    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestrictionRight;
    public List<double> timeStampsRight = new List<double>();
    public int inputIndexRight;
    private double _timeStampRight;
    private double _previousBlinkRight;
    private bool _ableToBlinkRight;
    private Color _blinkColorLeft;
    private Color _blinkColorRight;

    private void Awake()
    {
        Current = this;
    }

    protected override void Start() {
        base.Start();
        _blinkColorLeft = new Color(1f, 0.83f, 0f); //Yellow
        _blinkColorRight = new Color(0.47f, 0.31f, 0.66f); //Purple
    }

    public int GetInputIndex()
    {
        return InputIndex;
    }

    public void SetInputIndex(int inputI)
    {
        InputIndex = inputI;
    }

    public int GetInputIndexRight()
    {
        return inputIndexRight;
    }

    public void SetInputIndexRight(int inputIR)
    {
        inputIndexRight = inputIR;
    }

    public override void SetTimeStamps(IEnumerable<Note> array, Lane[] lanes)
    {
        foreach (var note in array)
        {
            timeStamps = AddNoteToTimeStamp(note, noteRestriction, timeStamps, lanes, "left");
            timeStampsRight = AddNoteToTimeStamp(note, noteRestrictionRight, timeStampsRight, lanes, "right");
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        if (Time.timeSinceLevelLoad > 5 && !GameManager.Current.IsGamePaused())
        {
            AudioTime = MusicPlayer.Current.GetAudioSourceTime() - (MusicPlayer.Current.inputDelayInMilliseconds / 1000.0);
            if (enableBlink)
            {
                (AbleToBlink, PreviousBlink) = CheckBlink(_blinkColorLeft, SingleButtonAction.Current.blinkColor, TimeStamp, SingleButtonAction.Current.TimeStamp, AbleToBlink, PreviousBlink);
                (_ableToBlinkRight, _previousBlinkRight) = CheckBlink(_blinkColorRight, SingleButtonAction.Current.blinkColor, _timeStampRight, SingleButtonAction.Current.TimeStamp,_ableToBlinkRight, _previousBlinkRight);
            }
            
            if (InputIndex < timeStamps.Count){
                TimeStamp = timeStamps[InputIndex];
                InputIndex = CheckMiss(InputIndex, TimeStamp);
            }
            if (inputIndexRight < timeStampsRight.Count){
                _timeStampRight = timeStampsRight[inputIndexRight];
                inputIndexRight = CheckMiss(inputIndexRight, _timeStampRight);
            }
        }
    }

    private void GetAccuracySide(bool left)
    {
        if (left){
            InputIndex = GetAccuracy( TimeStamp, InputIndex);
        }else{
            inputIndexRight = GetAccuracy( _timeStampRight, inputIndexRight);
        }
    }
    
    public override void TriggerScoreCalculation(InputAction.CallbackContext context)
    {
        if (Time.timeSinceLevelLoad > 5 && !GameManager.Current.IsGamePaused())
        {
            if (context.performed && context.ReadValue<Vector2>().x == -1 && InputIndex < timeStamps.Count){
                GetAccuracySide(true);
            }
            else if (context.performed && context.ReadValue<Vector2>().x == 1 && inputIndexRight < timeStampsRight.Count){
                GetAccuracySide(false);
            }
        }
    }

}