using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformParent : MonoBehaviour
{
    protected static int tempo = 0;
    protected Dictionary<int, float> tempoTime = new Dictionary<int, float>()
        {
            { 0, 3.609f },
            { 1, 1.8045f }
        };
    // The time we should wait before platform starts blinking
    protected Dictionary<int, float> blinkDelay = new Dictionary<int, float>()
        {
            { 0, 2.255f },
            { 1, 0.255f }
        };
    protected float _blinkTime = 0.4511f;
    public void setTempo(int newTempo) {
        tempo = newTempo;
    }
}
