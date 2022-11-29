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
        public GameObject playerGameObject;
        private PlayerMovement _playerMovement;
        private bool _hasSetPlayerInputToTrue;

        private void Start()
        {
            _playerMovement = playerGameObject.GetComponent<PlayerMovement>();
            _shouldCountTime = SceneManager.GetActiveScene().name != "MainMenuScene";
            _hasSetPlayerInputToTrue = false;
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
                if (!_hasSetPlayerInputToTrue)
                {
                    _hasSetPlayerInputToTrue = true;
                    _playerMovement.SetPlayerInputEnabledTrue();
                }
            }
        }

        public void SetCountdown(float time)
        {
            countdown = time;
            countdownUI.SetActive(true);
        }
    }
