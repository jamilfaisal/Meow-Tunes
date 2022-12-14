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

    private void Blink(Color blinkColor)
    {
        if (_visible)
        {
            StartCoroutine(BlinkDelay());
        }
    }

    private IEnumerator BlinkDelay()
    {
        Material.color = StartColor * 1.5f;
        yield return new WaitForSeconds(0.1f);
        Material.color = StartColor;
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
        var newColor = StartColor;
        newColor.a = 0.3f;
        Material.color = newColor; 
        MeshCollider.enabled = false;
        _visible = false;
    }

    private void Appear()
    {
        Material.color = StartColor;
        MeshCollider.enabled = true;
        _visible = true;
    }
}
