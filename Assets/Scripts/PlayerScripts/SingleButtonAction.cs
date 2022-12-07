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

    protected override void Start() {
        base.Start();
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
        if (Time.timeSinceLevelLoad > 5 && !GameManager.Current.IsGamePaused() && InputIndex < timeStamps.Count)
        {
            AudioTime = MusicPlayer.Current.GetAudioSourceTime() - (MusicPlayer.Current.inputDelayInMilliseconds / 1000.0);
            TimeStamp = timeStamps[InputIndex];

            (AbleToBlink, PreviousBlink) = CheckBlink(blinkColor, blinkColor, TimeStamp, TimeStamp,  AbleToBlink, PreviousBlink);
            
            InputIndex = CheckMiss(InputIndex, TimeStamp);
        }
    }
    public override void TriggerScoreCalculation(InputAction.CallbackContext context)
    {
        if (context.performed && Time.timeSinceLevelLoad > 5 && !GameManager.Current.IsGamePaused() && InputIndex < timeStamps.Count)
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