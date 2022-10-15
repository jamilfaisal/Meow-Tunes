using System.Collections;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private MeshRenderer _renderer;
    private Color _startColor;
    private Color _oldColor;
    private Color _startColorTransparent;
    private Collider _collider;
    private Material _material;
    private const float BlinkTime = 0.4511f;
    private const float SlowBlinkTime = 0.9022f;
    private bool _visible = true;

    private void Start()
    {
        
        _renderer = GetComponent<MeshRenderer>();
        _material = _renderer.materials[1];
        _startColor = _material.color;
        _collider = GetComponent<BoxCollider>();
        if (_collider == null)
        {
            _collider = GetComponent<MeshCollider>();
        }

        PlatformManager.current.BlinkEvent += Blink;
        PlatformManager.current.SwitchEvent += Switch;
        if (!gameObject.CompareTag("Green")) return;
        Disappear();

    }

    private void Blink()
    {
        if (_visible == false)
        {
            return;
        }

        StartCoroutine(GameManager.current.GetAudioTempo() == -1 ? BlinkDelay(SlowBlinkTime) : BlinkDelay(BlinkTime));
    }

    private IEnumerator BlinkDelay(float blinkTime)
    {
        for (var i = 0; i < 3; i++)
        {
            _material.color = _startColor * 1.5f;
            yield return new WaitForSeconds(0.1f);
            _material.color = _startColor;
            yield return new WaitForSeconds(blinkTime - 0.1f);
        }
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
