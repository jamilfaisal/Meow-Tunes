using System;
using UnityEngine;

// [System.Serializable]
// public class BlinkUnityEvent : UnityEvent<Color>
// {
//     public Color blinkColor;
// }

public class PlatformManager : MonoBehaviour
{
    public static PlatformManager current;

    public delegate void BlinkDelegateEvent (Color blinkColorUsed);
    public BlinkDelegateEvent BlinkEvent;
    // public event Action BlinkEvent;
    public event Action SwitchEvent;

    private void Awake()
    {
        current = this;
    }

    public void InvokeBlink(Color blinkColor)
    {
        BlinkEvent?.Invoke(blinkColor);
    }

    public void InvokeSwitch()
    {
        SwitchEvent?.Invoke();
    }
}