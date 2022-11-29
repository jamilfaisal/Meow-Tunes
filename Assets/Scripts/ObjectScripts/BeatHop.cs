using System;
using UnityEngine;

public class BeatHop : MonoBehaviour
{
    public static BeatHop Current;

    private void Awake()
    {
        Current = this;
    }

    private const float Amplitude = 0.4f;
    private Vector3 _initialPosition;
    private float _frequency = 1f;

    private void Start()
    {
        _initialPosition = transform.position;
    }

    public void SetFrequency(float bpm)
    {
        _frequency = bpm / 120;
    }

    private void Update()
    {
        if (Time.timeSinceLevelLoad > 5f)
        {
            transform.position = new Vector3(_initialPosition.x,
                Mathf.Sin(Time.time * _frequency * 2*Mathf.PI) * Amplitude + _initialPosition.y,
                _initialPosition.z);
        }
    }
}
