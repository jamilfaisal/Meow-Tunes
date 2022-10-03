using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    bool gameHasEnded = false;
    public GameObject completeLevelUI;
    public GameObject lostLevelUI;
    public GameObject playerGameObject;
    private PlayerInput _playerInput;

    public AudioSource gameOverSound;
    private PlayerMovement _playerMovement;

    private void Start()
    {
        _playerMovement = playerGameObject.GetComponent<PlayerMovement>();
        _playerInput = playerGameObject.GetComponent<PlayerInput>();
    }

    private void Update() {
        if (Input.GetButton("Submit") && gameHasEnded) {
            restartLevel();
        }
    }
    public void wonLevel() {
        if (gameHasEnded == false) {
            gameHasEnded = true;
            _playerMovement.enabled = false;
            // _playerInput.enabled = false;
            completeLevelUI.SetActive(true);
        }
    }

    public void lostLevel() {
        if (gameHasEnded == false) {
            gameOverSound.Play();
            gameHasEnded = true;
            _playerMovement.enabled = false;
            // _playerInput.enabled = false;
            lostLevelUI.SetActive(true);
        }
    }

    void restartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameHasEnded = false;
    }

    public void StartLevel()
    {
        if (gameHasEnded)
        {
            restartLevel();
        }
    }
    
}
