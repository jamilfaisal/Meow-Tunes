using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    bool gameHasEnded = false;
    public static bool gameIsPaused = false;
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
            completeLevelUI.SetActive(true);
        }
    }

    public void lostLevel() {
        if (gameHasEnded == false) {
            gameHasEnded = true;
            gameOverSound.Play();
            _playerMovement.enabled = false;
            lostLevelUI.SetActive(true);
        }
    }

    public void restartLevel() {
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
    

    public bool isGamePaused() {
        return gameIsPaused;
    }

    public bool hasGameEnded() {
        return gameHasEnded;
    }

    public void pauseGame() {
        gameIsPaused = true;
    }

    public void resumeGame() {
        gameIsPaused = false;
    }
}
