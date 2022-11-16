using System.Collections;
using UnityEngine;

public class NeutralPlatform : Platform
{
    protected override void Start()
    {
        base.Start();
        
        PlatformManager.current.BlinkEvent += Blink;
    }
    private void Blink()
    {
        StartCoroutine(BlinkDelay());
    }

    private IEnumerator BlinkDelay()
    {
        _material.color = _startColor * 1.5f;
        yield return new WaitForSeconds(0.125f);
        _material.color = _startColor;
        yield return new WaitForSeconds(0.125f);
        _material.color = _startColor * 1.5f;
        yield return new WaitForSeconds(0.125f);
        _material.color = _startColor;
        yield return new WaitForSeconds(0.125f);
        yield return null;
    }

}
