using System.Collections;
using UnityEngine;

public class SwitchPlatform : Platform
{
    private Color _oldColor;
    private Color _startColorTransparent;
    private bool _visible = true;

    protected override void Start()
    {
        base.Start();
        
        PlatformManager.current.BlinkEvent += Blink;
        PlatformManager.current.SwitchEvent += Switch;
        if (gameObject.CompareTag("Green")) return;
        Disappear();
    }

    private void Blink()
    {
        if (_visible)
        {
            StartCoroutine( BlinkDelay());
        }
    }

    private IEnumerator BlinkDelay()
    {
        _material.color = _startColor * 1.5f;
        yield return new WaitForSeconds(0.1f);
        _material.color = _startColor;
        yield return null;
    }

    private void Switch()
    {
        if (_visible)
        {
            Disappear();
        }
        else
        {
            Appear();
        }
    }
    private void Disappear()
    {
        var newColor = _startColor;
        newColor.a = 0.3f;
        _material.color = newColor; 
        _collider.enabled = false;
        _visible = false;
    }

    private void Appear()
    {
        _material.color = _startColor;
        _collider.enabled = true;
        _visible = true;
    }
}
