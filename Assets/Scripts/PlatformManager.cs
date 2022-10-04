using System;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public static PlatformManager current;
    
    private int _audioTempo;
    private int _blinkIndex;
    private int _switchIndex;
    private float[] _blinkTempos;
    private float[] _switchTempos;
    public event Action BlinkEvent;
    public event Action SwitchEvent;

    private void Awake()
    {
        current = this;
        _blinkTempos = new[]
        {
            0.4505f,
            2.255f,
            4.059f,
            5.864f,
            7.668f,
            9.473f,
            11.277f,
            13.081f,
            14.886f,
            16.691f,
            18.495f,
            20.3f,
            22.105f,
            23.91f,
            25.715f,
            27.52f,
            29.325f,
            31.13f,
            32.934f,
            34.738f,
            36.542f,
            38.346f,
            40.15f,
            41.954f,
            43.758f,
            45.562f,
            47.366f,
            49.17f,
            50.974f,
            52.778f,
            54.582f,
            56.386f,
            58.191f,
            59.995f,
            61.799f,
            63.603f,
            65.407f,
            67.212f,
            69.017f,
            70.822f,
            72.627f,
            74.431f,
            76.236f,
            78.041f,
            79.846f,
            81.651f,
            83.456f,
            85.261f,
            100f
        };
        _switchTempos = new[]
        {
            1.8045f,
            3.609f,
            5.413f,
            7.218f,
            9.023f,
            10.828f,
            12.633f,
            14.438f,
            16.242f,
            18.047f,
            19.852f,
            21.657f,
            23.462f,
            25.267f,
            27.072f,
            28.877f,
            30.681f,
            32.486f,
            34.29f,
            36.094f,
            37.898f,
            39.703f,
            41.508f,
            43.312f,
            45.116f,
            46.92f,
            48.724f,
            50.528f,
            52.332f,
            54.136f,
            55.941f,
            57.745f,
            59.549f,
            61.353f,
            63.157f,
            64.962f,
            66.767f,
            68.572f,
            70.377f,
            72.181f,
            73.986f,
            75.791f,
            77.596f,
            79.401f,
            81.206f,
            83.011f,
            84.816f,
            86.621f,
            100f
        };
    }

    private void Start()
    {
        Conductor.current.SongLooped += ResetIndices;
    }

    private void Update()
    {
        if (Conductor.current.audioSource.time >= _blinkTempos[CalculateIndex(_blinkIndex)])
        {
            BlinkEvent?.Invoke();
            _blinkIndex = IncreaseIndex(_blinkIndex);
            if (_blinkIndex == _blinkTempos.Length)
            {
                _blinkIndex = _blinkTempos.Length - 1;
            }
        }

        if (!(Conductor.current.audioSource.time >= _switchTempos[CalculateIndex(_switchIndex)])) return;
        SwitchEvent?.Invoke();
        _switchIndex = IncreaseIndex(_switchIndex);
        if (_switchIndex == _switchTempos.Length)
        {
            _switchIndex = _switchTempos.Length - 1;
        }
    }

    private int CalculateIndex(int index)
    {
        if (_audioTempo == 1) {
            return index;
        }

        if (_audioTempo != 0) return -1;
        // Even
        if (index % 2 == 0)
        {
            return index + 1;
        }
        return index;
    }

    private int IncreaseIndex(int index)
    {
        return _audioTempo switch
        {
            1 => index + 1,
            0 => index + 2,
            _ => -1
        };
    }
    public void IncreaseTempo()
    {
        _audioTempo = Math.Min(_audioTempo + 1, 1); 
    }

    public void DecreaseTempo()
    {
        _audioTempo = Math.Max(_audioTempo - 1, 0); // TODO: Change later to -1
    }

    private void ResetIndices()
    {
        _blinkIndex = 0;
        _switchIndex = 0;
    }
}