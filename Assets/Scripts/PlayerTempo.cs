using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTempo : MonoBehaviour
{
    public PlatformParent platform;
    public Conductor conductor;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
            platform.setTempo(1);
            conductor.increaseTempo();
        } else if (Input.GetKeyDown(KeyCode.Q)) {
            platform.setTempo(0);
            conductor.decreaseTempo();
        }
    }
}
