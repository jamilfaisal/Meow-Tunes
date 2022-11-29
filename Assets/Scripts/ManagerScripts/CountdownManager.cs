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
        

        private void Start()
        {
            shouldCountTime = SceneManager.GetActiveScene().name != "MainMenuScene";
            countdown = 5f;
            _gameStart = true;
        }
        
        private void Update()
        {
            if ( shouldCountTime && !_soundPlayed)
            {
                countdownSound.Play();
                _soundPlayed = true;
            }
            
            if (shouldCountTime && countdown > 0)
            {
                if (_gameStart) SubstractTime();
                else SubstractUnscaledTime();
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

        public void SetCountdown()
        {
            countdown = 5f;
            shouldCountTime = true;
            countdownUI.SetActive(true);
        }

        private void SubstractTime()
        {
            countdown -= Time.deltaTime;
        }
        
        private void SubstractUnscaledTime()
        {
            countdown -= Time.unscaledDeltaTime;
        }
    }
