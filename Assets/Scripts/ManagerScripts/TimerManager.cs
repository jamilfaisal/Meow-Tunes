using System;
using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour
{

    public static TimerManager current;

    private bool _timerActive;
    private float _currentTime;
    public TMP_Text currentTimeText;
    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        _currentTime = 0;
        _timerActive = true;
    }

    private void Update()
    {
        if (_timerActive)
        {
            _currentTime += Time.deltaTime;
            if (GameManager.current.timerOn)
            {
                currentTimeText.text = FormatTime();
            }
        }
    }

    public void ResumeTimer()
    {
        _timerActive = true;
    }

    public void StopTimer()
    {
        _timerActive = false;
    }

    public string FormatTime()
    {
        var time = TimeSpan.FromSeconds(_currentTime);
        return "Time: " + time.ToString(@"mm':'ss'.'ff");
    }
}
