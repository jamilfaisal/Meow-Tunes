using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{

    public static CameraShake Current;

    private CinemachineVirtualCamera vcam;
    private CinemachineBasicMultiChannelPerlin noise;
    private float _timeAtCurrentFrame;
    private float _timeAtLastFrame;
    private float _fakeDelta;


    void Awake()
    {
        Current = this;
        vcam = GetComponent<CinemachineVirtualCamera>();
        noise = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void Update()
    {
        // Calculate a fake delta time, so we can Shake while game is paused.
        _timeAtCurrentFrame = Time.realtimeSinceStartup;
        _fakeDelta = _timeAtCurrentFrame - _timeAtLastFrame;
        _timeAtLastFrame = _timeAtCurrentFrame;
    }

    public void Shake(float duration)
    {
        Current.StopAllCoroutines();
        Current.StartCoroutine(Current.cShake(duration));
    }

    public IEnumerator cShake(float duration)
    {
        while (duration > 0)
        {
            noise.m_AmplitudeGain = 1.7f;
            duration -= _fakeDelta;
            yield return null;
        }

        noise.m_AmplitudeGain = 0f;
    }
}