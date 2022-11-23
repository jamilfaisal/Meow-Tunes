using Melanchall.DryWetMidi.Interaction;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SingleButtonAction : PlayerAction
{
    public Color blinkColor;
    public static SingleButtonAction Current;

    private void Awake()
    {
        Current = this;
    }

    private void Start() {
        blinkColor = new Color(0.41f, 0.63f, 0.13f); //Green
    }

    public int GetInputIndex()
    {
        return InputIndex;
    }

    public void SetInputIndex(int inputI)
    {
        InputIndex = inputI;
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
        if (Time.timeSinceLevelLoad > 5 && !GameManager.current.IsGamePaused() && InputIndex < timeStamps.Count)
        {
            MarginOfError = MusicPlayer.current.marginOfError;
            AudioTime = MusicPlayer.current.GetAudioSourceTime() - (MusicPlayer.current.inputDelayInMilliseconds / 1000.0);
            TimeStamp = timeStamps[InputIndex];

            (_ableToBlink, _previousBlink) = CheckBlink(blinkColor, blinkColor, TimeStamp, TimeStamp,  _ableToBlink, _previousBlink);
            
            InputIndex = CheckMiss(InputIndex, TimeStamp);
        }
    }
    public override void TriggerScoreCalculation(InputAction.CallbackContext context)
    {
        if (context.performed && Time.timeSinceLevelLoad > 5 && !GameManager.current.IsGamePaused() && InputIndex < timeStamps.Count)
        {
            InputIndex = GetAccuracy(TimeStamp, InputIndex);
        }
    }
    
    public double GetNextTimestamp(int index)
    {
        if (index < timeStamps.Count)
        {
            return timeStamps[index];
        }
        return 999;
    }
}