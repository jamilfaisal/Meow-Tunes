using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

    public class CountdownManager: MonoBehaviour
    {
        public float countdown;
        public TMP_Text countdownText;
        public GameObject countdownUI;
        public AudioSource countdownSound;
        public bool shouldCountTime;
        private bool _soundPlayed;
        private bool _gameStart;
        public static CountdownManager current;

        private void Awake()
        {
            current = this;
        }

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
                    countdownSound.Play();
                    _soundPlayed = true;
                }

                if (countdown > 0)
                {
                    SubtractTime(_gameStart);
                    countdownText.text = (countdown).ToString("0");
                }
                else
                {
                    countdownSound.Stop();
                    countdownUI.SetActive(false);
                    _soundPlayed = false;
                    _gameStart = false;
                }
            }
        }

        public void SetCountdown()
        {
            countdown = 5f;
            shouldCountTime = true;
            countdownUI.SetActive(true);
        }
        
        private void SubtractTime(bool gameStart) {
            countdown -= gameStart ? Time.deltaTime : Time.unscaledDeltaTime;
        }
    }
