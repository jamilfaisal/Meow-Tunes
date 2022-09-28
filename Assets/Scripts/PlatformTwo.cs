using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTwo : MonoBehaviour
{

    public bool switchState = true;
    public bool shouldBlink;

    // private bool _shouldAppear;
    // initial state -> enabled 
    private bool _state = true;
    // This is the time we're given for each beat
    private float _delayTime = 3.609f;
    // The time we should wait before platform starts blinking
    private float _blinkDelay = 2.255f;

    
    private MeshRenderer _renderer;
    private Color _startColor;
    private Color _oldColor;
    private Color _startColorTransp;
    private Collider _collider;
    private Material _material;
    private float _blinkTime = 0.4511f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
        _renderer = GetComponent<MeshRenderer>();
        _material = _renderer.materials[1];
        _startColor = _material.color;
        _collider = GetComponent<BoxCollider>();
        if (_collider == null)
        {
            _collider = GetComponent<MeshCollider>();
        }
        
        if (gameObject.CompareTag("Green"))
        {
            _state = false;
            Disappear();
        }
        
    }
    
    void Update()
    {
        if (switchState)
        {
            switchState = false;
            var coroutine = SwitchState();
            StartCoroutine(coroutine);
        }
        
    }

    private IEnumerator SwitchState()
    {
        var oldState = _state;
        yield return new WaitForSeconds(_blinkDelay);
        
        // Start blink
        if (_state)
        {
            for (var i = 0; i < 3; i++)
            {
                _material.color = _startColor * 1.5f;
                yield return new WaitForSeconds(0.1f);
                _material.color = _startColor;
                yield return new WaitForSeconds(_blinkTime - 0.1f);
            }
            Disappear();
        }
        else
        {
            yield return new WaitForSeconds(_delayTime - _blinkDelay);
            Reappear();
        }
        

        _state = !oldState;
        
        switchState = true;

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
    
}
