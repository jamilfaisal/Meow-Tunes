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
        private bool _shouldCountTime;
        private bool _soundPlayed;
        public GameObject playerGameObject;
        private PlayerMovement _playerMovement;

        private void Start()
        {
            _playerMovement = playerGameObject.GetComponent<PlayerMovement>();
            _shouldCountTime = SceneManager.GetActiveScene().name != "MainMenuScene";
            countdown = 5f;
        }
        
        private void Update()
        {
            if ( _shouldCountTime && !_soundPlayed)
            {
                countingDown = true;
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

                if (countingDown)
                {
                    _playerMovement.SetPlayerInputEnabled(true);
                }
                countingDown = false;
            }
        }

        public void SetCountdown(float time)
        {
            countdown = time;
            countdownUI.SetActive(true);
        }
    }
