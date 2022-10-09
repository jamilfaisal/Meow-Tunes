using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager current;

    private static bool _gameIsPaused = false;
    private bool _gameHasEnded = false;
    public GameObject completeLevelUI;
    public GameObject lostLevelUI;
    public GameObject playerGameObject;
    private PlayerMovement _playerMovement;
    public AudioSource gameOverSound;
    // -1 is slow, 0 is normal, 1 is fast
    public int audioTempo;

    private void Awake()
    {
        _playerMovement = playerGameObject.GetComponent<PlayerMovement>();
        current = this;
        _gameHasEnded = false;
        _playerMovement.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update() {
        if (Input.GetButton("Submit") && _gameHasEnded) {
            RestartLevel();
        }
    }
    public void WonLevel() {
        if (_gameHasEnded) return;
        _gameHasEnded = true;
        _playerMovement.enabled = false;
        completeLevelUI.SetActive(true);
    }

    public void LostLevel() {
        if (_gameHasEnded) return;
        _gameHasEnded = true;
        gameOverSound.Play();
        _playerMovement.enabled = false;
        lostLevelUI.SetActive(true);
    }

    private void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        _gameHasEnded = false;
    }

    public int GetAudioTempo()
    {
        return audioTempo;
    }
    
    
    public void StartLevel()
    {
        if (_gameHasEnded)
        {
            RestartLevel();
        }
    }
    
    public void IncreaseAudioTempo()
    {
        audioTempo = Math.Min(audioTempo + 1, 1); 
    }

    public void DecreaseAudioTempo()
    {
        audioTempo = Math.Max(audioTempo - 1, -1);

    }

    public bool IsGamePaused() {
        return _gameIsPaused;
    }

    public bool HasGameEnded() {
        return _gameHasEnded;
    }

    public void PauseGame() {
        _gameIsPaused = true;
    }

    public void ResumeGame() {
        _gameIsPaused = false;
    }
}
