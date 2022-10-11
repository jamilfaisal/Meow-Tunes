using System;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public static PlatformManager current;
    
    private int _blinkIndex;
    private int _switchIndex;
    private int _slowBlinkIndex;
    private int _slowSwitchIndex;
    private float[] _slowBlinkTempos;
    private float[] _slowSwitchTempos;
    private float[] _normalAndFastBlinkTempos;
    private float[] _normalAndFastSwitchTempos;
    public event Action BlinkEvent;
    public event Action SwitchEvent;

    private void Awake()
    {
        current = this;
        _normalAndFastBlinkTempos = new[]
        {
            0.4505f, 2.255f, 4.059f, 5.864f, 7.668f, 9.473f, 11.277f, 13.081f, 14.886f, 16.691f, 18.495f, 20.3f,
            22.105f, 23.91f, 25.715f, 27.52f, 29.325f, 31.13f, 32.934f, 34.738f, 36.542f, 38.346f, 40.15f, 41.954f,
            43.758f, 45.562f, 47.366f, 49.17f, 50.974f, 52.778f, 54.582f, 56.386f, 58.191f, 59.995f, 61.799f, 63.603f,
            65.407f, 67.212f, 69.017f, 70.822f, 72.627f, 74.431f, 76.236f, 78.041f, 79.846f, 81.651f, 83.456f, 85.261f,
            100f
        };
        _slowBlinkTempos = new[]
        {
            4.511f, 11.729f, 18.947f, 26.165f, 33.383f, 40.601f, 47.819f, 55.037f, 62.255f, 69.473f, 76.691f, 83.909f,
            100f
        };
        

        _normalAndFastSwitchTempos = new[]
        {
            1.8045f, 3.609f, 5.413f, 7.218f, 9.023f, 10.828f, 12.633f, 14.438f, 16.242f, 18.047f, 19.852f, 21.657f,
            23.462f, 25.267f, 27.072f, 28.877f, 30.681f, 32.486f, 34.29f, 36.094f, 37.898f, 39.703f, 41.508f, 43.312f,
            45.116f, 46.92f, 48.724f, 50.528f, 52.332f, 54.136f, 55.941f, 57.745f, 59.549f, 61.353f, 63.157f, 64.962f,
            66.767f, 68.572f, 70.377f, 72.181f, 73.986f, 75.791f, 77.596f, 79.401f, 81.206f, 83.011f, 84.816f, 86.621f,
            100f
        };

        _slowSwitchTempos = new[]
        {
            7.218f, 14.436f, 21.654f, 28.872f, 36.09f, 43.308f, 50.526f, 57.744f, 64.962f, 72.18f, 79.398f, 86.616f,
            100f
        };
    }

    private void Start()
    {
        Conductor.current.SongLooped += ResetIndices;
        PlayerTempo.current.ChangingTempo += CalculateIndices;
    }

    private void Update()
    {
        var audioTempo = GameManager.current.GetAudioTempo();
        if (audioTempo is 0 or 1)
        {
            if (Conductor.current.audioSource.time >= _normalAndFastBlinkTempos[CalculateIndex(_blinkIndex, audioTempo)])
            {
                BlinkEvent?.Invoke();
                _blinkIndex = IncreaseIndex(_blinkIndex, audioTempo);
                if (_blinkIndex == _normalAndFastBlinkTempos.Length)
                {
                    _blinkIndex = _normalAndFastBlinkTempos.Length - 1;
                }
            }

            if (Conductor.current.audioSource.time >= _normalAndFastSwitchTempos[CalculateIndex(_switchIndex, audioTempo)])
            {

                SwitchEvent?.Invoke();
                _switchIndex = IncreaseIndex(_switchIndex, audioTempo);
                if (_switchIndex == _normalAndFastSwitchTempos.Length)
                {
                    _switchIndex = _normalAndFastSwitchTempos.Length - 1;
                }
            }
        }
        else
        {
            if (Conductor.current.audioSource.time >= _slowBlinkTempos[CalculateIndex(_slowBlinkIndex, audioTempo)])
            {
                BlinkEvent?.Invoke();
                _slowBlinkIndex = IncreaseIndex(_slowBlinkIndex, audioTempo);
                if (_slowBlinkIndex == _slowBlinkTempos.Length)
                {
                    _slowBlinkIndex = _slowBlinkTempos.Length - 1;
                }
            }

            if (Conductor.current.audioSource.time >= _slowSwitchTempos[CalculateIndex(_slowSwitchIndex, audioTempo)])
            {

                SwitchEvent?.Invoke();
                _slowSwitchIndex = IncreaseIndex(_slowSwitchIndex, audioTempo);
                if (_slowSwitchIndex == _slowSwitchTempos.Length)
                {
                    _slowSwitchIndex = _slowSwitchTempos.Length - 1;
                }
            }
        }
    }

    private int CalculateIndex(int index, int audioTempo)
    {
        if (audioTempo != 0) return index;
        // Even
        if (index % 2 == 0)
        {
            return index + 1;
        }
        return index;
    }

    private int IncreaseIndex(int index, int audioTempo)
    {
        if (audioTempo == 0)
        {
            return index + 2;
        }
        else
        {
            return index + 1;
        }
    }

    private void CalculateIndices()
    {
        var musicTime = Conductor.current.audioSource.time;
        
        var minimumIndex = 0;
        for (var i = 0; i < _slowBlinkTempos.Length; i++)
        {
            if (!(musicTime < _slowBlinkTempos[i])) continue;
            minimumIndex = i;
            break;
        }
        _slowBlinkIndex = minimumIndex;
        _slowSwitchIndex = minimumIndex;

        minimumIndex = 0;
        for (var i = 0; i < _normalAndFastBlinkTempos.Length; i++)
        {
            if (!(musicTime < _normalAndFastBlinkTempos[i])) continue;
            minimumIndex = i;
            break;
        }

        _blinkIndex = minimumIndex;
        _switchIndex = minimumIndex;
    }

    private void ResetIndices()
    {
        _blinkIndex = 0;
        _switchIndex = 0;
        _slowBlinkIndex = 0;
        _slowSwitchIndex = 0;
    }
}