using System;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public static PlatformManager current;
    public event Action BlinkEvent;
    public event Action SwitchEvent;

    private void Awake()
    {
        current = this;
    }

    public void InvokeBlink()
    {
        BlinkEvent?.Invoke();
    }

    public void InvokeSwitch()
    {
        SwitchEvent?.Invoke();
    }
}