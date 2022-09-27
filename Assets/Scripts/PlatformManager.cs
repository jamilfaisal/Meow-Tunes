using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    private GameObject[] _greenPlatforms;    
    private GameObject[] _redPlatforms;
    private readonly Platform[] _greenPlatformScripts = new Platform[50];
    private readonly Platform[] _redPlatformScripts = new Platform[50];
    private Platform[] _toEnableScript = new Platform[50];
    private Platform[] _toDisableScript = new Platform[50];
    private GameObject _conductor;
    // This is the time we're given for each beat
    private float _delayTime = 3.609f;
    // The time we should wait before platform starts blinking
    private float _blinkTime = 2.255f;
    private GameObject[] _toEnable;
    private GameObject[] _toDisable;
    private GameObject[] _tempEnable;
    private GameObject[] _tempDisable;
    private MeshRenderer[] _toBlink;
    private MeshRenderer[] _toNotBlink;
    private MeshRenderer[] _tempBlink;
    private MeshRenderer[] _tempNoBlink;

    // For blinking effect
    private Color _startColor;
    void Start()
    {
        _greenPlatforms = GameObject.FindGameObjectsWithTag("Green");
        _redPlatforms = GameObject.FindGameObjectsWithTag("Red");
        
        GetPlatforms();
        // DisableInitial();
        
        _toEnable = _redPlatforms;
        _toDisable = _greenPlatforms;
        _toEnableScript = _redPlatformScripts;
        _toDisableScript = _greenPlatformScripts;

        
        StartTheCoroutine();

    }

    private IEnumerator SwitchPlatformsTimeout() {
     while(true)
     {

         var tempEnableScripts = _toEnableScript;
         var tempDisableScripts = _toDisableScript;
        _tempEnable = _toEnable;
        _tempDisable = _toDisable;
        
        yield return new WaitForSeconds(_blinkTime - .55f);
        
        // Start blinking the platforms that are about to disappear
        for (var i = 0; i < _toDisable.Length; i++)
        {
            if (_toDisableScript[i] == null) continue;
            var coroutine = _toDisableScript[i].Blink();
            StartCoroutine(coroutine);
        }

        yield return new WaitForSeconds(_delayTime - _blinkTime);
        
        for (var i = 0; i < _toEnable.Length; i++)
        {
            if (_toEnableScript[i] == null) continue;
            _toEnableScript[i].Reappear();
        }

        for (var i = 0; i < _toDisable.Length; i++)
        {
            if (_toDisableScript[i] == null) continue;
            _toDisableScript[i].Disappear();
        }
        
        // Switching the platforms that will be enabled on the next beat
        _toEnable = _tempDisable;
        _toDisable = _tempEnable;

        _toEnableScript = tempDisableScripts;
        _toDisableScript = tempEnableScripts;

        yield return new WaitForSeconds(.55f);
     }
    }
    
    

    private void StartTheCoroutine() {
        StartCoroutine(nameof(SwitchPlatformsTimeout));
    }
    
    private void GetPlatforms()
    {
        
        for (int i = 0; i < _greenPlatforms.Length; i++)
        {
            if (_greenPlatforms[i] == null) continue;
            _greenPlatformScripts[i] = _greenPlatforms[i].GetComponent<Platform>();
        }
        
        for (int i = 0; i < _redPlatforms.Length; i++)
        {
            if (_redPlatforms[i] == null) continue;
            _redPlatformScripts[i] = _redPlatforms[i].GetComponent<Platform>();
        }
        
    }

}
