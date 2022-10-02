using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameManager gameManager;
    public Conductor musicPlayer;
    public AudioSource walkingSound;
    public GameObject playerMovement;
    private PlayerMovement _playerMovementScript;


    private void Start()
    {
        _playerMovementScript = playerMovement.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
        _playerMovementScript.enabled = true;
        Time.timeScale = 1f;
        gameManager.resumeGame();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Pause()
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

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting...");
    }
}
