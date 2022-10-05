using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseScreen : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameManager gameManager;
    public Conductor musicPlayer;
    public AudioSource walkingSound;
    public GameObject playerMovement;
    private PlayerMovement _playerMovementScript;
    public Gamepad gamepad;


    private void Start()
    {
        _playerMovementScript = playerMovement.GetComponent<PlayerMovement>();
        gamepad = Gamepad.current;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gameManager.hasGameEnded())
        {
            if (gameManager.isGamePaused())
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        musicPlayer.Resume();
        Invoke(nameof(EnableMovement), 0.3f);
        Time.timeScale = 1f;
        gameManager.resumeGame();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        musicPlayer.Pause();
        _playerMovementScript.enabled = false;
        walkingSound.Stop();
        Time.timeScale = 0f;
        gameManager.pauseGame();
        // This is because the camera script locks the cursor,
        // so we need to enable it again to be able to click buttons
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PauseOrResumeController()
    {
        if (gameManager.hasGameEnded()) return;
        if (gameManager.isGamePaused())
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting...");
    }

    public void EnableMovement()
    {
        _playerMovementScript.enabled = true;
    }

}
