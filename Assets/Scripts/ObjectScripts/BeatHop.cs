using System;
using UnityEngine;

public class BeatHop : MonoBehaviour
{
    private const float Amplitude = 0.4f;
    private Vector3 _initialPosition;
    private const float Frequency = 1f;

    private void Start()
    {
        _initialPosition = transform.position;
    }

    private void Update()
    {
        if (Time.timeSinceLevelLoad > 5f)
        {
            transform.position = new Vector3(_initialPosition.x,
                Mathf.Sin(Time.time * Frequency * 2*Mathf.PI) * Amplitude + _initialPosition.y,
                _initialPosition.z);
        }
    }
}
