using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTempo : MonoBehaviour
{
    public PlatformParent platform;
    public MidiManager midi_manager;
    public Conductor conductor;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
            // platform.setTempo(1);
            midi_manager.SpeedUp();
            conductor.increaseTempo();
        } else if (Input.GetKeyDown(KeyCode.Q)) {
            // platform.setTempo(0);
            midi_manager.SlowDown();
            conductor.decreaseTempo();
        }
    }
}
