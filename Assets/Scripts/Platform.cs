using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public bool isBlinking = false;
    private MeshRenderer _renderer;
    private Color _startColor;
    private Color _oldColor;
    private Color _startColorTransp;
    private Collider _collider;
    private Material _material;
    private float _blinkTime = 0.4511f;
    
    void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        _material = _renderer.materials[1];
        _startColor = _material.color;
        _oldColor = new Color(_startColor.r, _startColor.g, _startColor.b, 1f);
        _startColorTransp = new Color(_startColor.r, _startColor.g, _startColor.b, 0.3f);
        _collider = GetComponent<BoxCollider>();

    }
    

    public void Disappear()
    {
        Color newColor = _startColor;
        newColor.a = 0.3f;
        _material.color = newColor; 
        _collider.enabled = false;
    }

    public void Reappear()
    {
        _material.color = _startColor;
        _collider.enabled = true;
    }

    public IEnumerator Blink()
    {
        if (_renderer != null)
        {
            for (var i = 0; i < 3; i++)
            {
                _material.color = _startColor * 1.5f;
                yield return new WaitForSeconds(0.1f);
                _material.color = _startColor;
                yield return new WaitForSeconds(_blinkTime - 0.1f);
            }
            isBlinking = false;
        }
    }
}
