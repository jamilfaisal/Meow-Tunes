using System;
using UnityEngine;

public class PlayerHopManager : MonoBehaviour
{
    public static PlayerHopManager Current;
    public SingleButtonAction jumpAction;

    private bool _hopping;
    private int _hopIndex;
    public event Action HopEvent;

    private void Awake()
    {
        Current = this;
    }

    private void Start()
    {
        _hopping = false;
    }

    public int GetHopIndex()
    {
        return _hopIndex;
    }

    public void SetHopIndex(int hopI)
    {
        _hopIndex = hopI;
    }

    private void Update()
    {
        if (Math.Abs(MusicPlayer.Current.GetAudioSourceTime() - SingleButtonAction.Current.GetNextTimestamp(_hopIndex)) < MusicPlayer.Current.perfectMarginOfError)
        {
            if (!_hopping)
            {
                HopEvent?.Invoke();
                _hopIndex++;
                _hopping = true;
            }
        }
        else
        {
            _hopping = false;
        }
    }
}
