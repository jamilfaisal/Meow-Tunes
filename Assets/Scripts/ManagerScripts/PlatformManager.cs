using System;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public static PlatformManager current;

    public int blinkIndex;
    private int _slowBlinkIndex;
    private float[] _slowBlinkTempos;
    private float[] _normalAndFastBlinkTempos;
    public event Action BlinkEvent;

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

    }

    private void Start()
    {
        Conductor.current.SongLooped += ResetIndices;
    }

    private void Update()
    {
        var audioTempo = GameManager.current.GetAudioTempo();
        if (audioTempo is 0 or 1)
        {
            if (Conductor.current.audioSource.time >= _normalAndFastBlinkTempos[CalculateIndex(blinkIndex, audioTempo)])
            {
                BlinkEvent?.Invoke();
                blinkIndex = IncreaseIndex(blinkIndex, audioTempo);
                if (blinkIndex == _normalAndFastBlinkTempos.Length)
                {
                    blinkIndex = _normalAndFastBlinkTempos.Length - 1;
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
        }
    }

    private static int CalculateIndex(int index, int audioTempo)
    {
        if (audioTempo != 0) return index;
        // Even
        if (index % 2 == 0)
        {
            return index + 1;
        }
        return index;
    }

    private static int IncreaseIndex(int index, int audioTempo)
    {
        return audioTempo switch
        {
            0 => index + 2,
            _ => index + 1
        };
    }

    private void ResetIndices()
    {
        blinkIndex = 0;
        _slowBlinkIndex = 0;
    }
}

// Archive
// private void CalculateIndices()
// {
//     var musicTime = Conductor.current.audioSource.time;
//         
//     var minimumIndex = 0;
//     for (var i = 0; i < _slowBlinkTempos.Length; i++)
//     {
//         if (!(musicTime < _slowBlinkTempos[i])) continue;
//         minimumIndex = i;
//         break;
//     }
//     _slowBlinkIndex = minimumIndex;
//     _slowSwitchIndex = minimumIndex;
//
//     minimumIndex = 0;
//     for (var i = 0; i < _normalAndFastBlinkTempos.Length; i++)
//     {
//         if (!(musicTime < _normalAndFastBlinkTempos[i])) continue;
//         minimumIndex = i;
//         break;
//     }
//
//     _blinkIndex = minimumIndex;
//     _switchIndex = minimumIndex;
// }