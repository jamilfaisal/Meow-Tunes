using System.Collections;
using UnityEngine;

public class NeutralPlatform : Platform
{
    private float _blinkWaitTime;
    protected override void Start()
    {
        base.Start();

        _blinkWaitTime = 0.125f;
        
        PlatformManager.current.BlinkEvent += Blink;
    }
    private void Blink(Color blinkColor)
    {
        StartCoroutine(BlinkDelay(blinkColor));
    }

    private IEnumerator BlinkDelay(Color blinkColor)
    {
        _material.color = blinkColor;
        yield return new WaitForSeconds(_blinkWaitTime*4);
        _material.color = blinkColor * 1.2f;
        yield return new WaitForSeconds(_blinkWaitTime*4);
        _material.color = _startColor;
        yield return null;
    }

}
