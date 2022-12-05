using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

    public class CountdownManager: MonoBehaviour
    {
        public static CountdownManager Current;

        private void Awake()
        {
            Current = this;
        }

        [HideInInspector] public bool countingDown;
        public float countdown;
        public TMP_Text countdownText;
        public GameObject countdownUI;
        public AudioSource countdownSound;
        public bool shouldCountTime;
        private bool _soundPlayed;
        private bool _gameStart;

        private void Start()
        {
            shouldCountTime = true;
            countdown = 5f;
            _gameStart = true;
        }
        
        private void Update()
        {
            if (shouldCountTime)
            {
                if (!_soundPlayed)
                {
                    countingDown = true;
                    countdownSound.Play();
                    _soundPlayed = true;
                }

                if (countdown > 0)
                {
                    SubtractTime();
                    countdownText.text = (countdown).ToString("0");
                }
                else
                {
                    countdownSound.Stop();
                    countdownUI.SetActive(false);
                    _soundPlayed = false;
                    _gameStart = false;
                    countingDown = false;
                    shouldCountTime = false;
                }
            }
        }

        public void SetCountdown(float time)
        {
            countdown = time;
            shouldCountTime = true;
            countdownUI.SetActive(true);
        }
        
        private void SubtractTime() {
            countdown -= _gameStart ? Time.deltaTime : Time.unscaledDeltaTime;
        }
    }
