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
        private bool _shouldCountTime;
        private bool _soundPlayed;

        private void Start()
        {
            _shouldCountTime = SceneManager.GetActiveScene().name != "MainMenuScene";
            countdown = 5f;
        }
        
        private void Update()
        {
            if ( _shouldCountTime && !_soundPlayed)
            {
                countdownSound.Play();
                _soundPlayed = true;
            }
            
            if (_shouldCountTime && countdown > 0)
            {
                countdown -= Time.deltaTime;
                countdownText.text = (countdown).ToString("0");
            }
            else
            {
                countdownSound.Stop();
                countdownUI.SetActive(false);
            }
        }

        public void SetCountdown(float time)
        {
            countdown = time;
            countdownUI.SetActive(true);
        }
    }
