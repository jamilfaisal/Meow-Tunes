using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager current;

    private static bool _gameIsPaused;
    private bool _gameHasEnded;
    // -1 is slow, 0 is normal, 1 is fast
    private int _audioTempo;
    public GameObject playerGameObject;
    private PlayerMovement _playerMovement;
    public AudioSource gameOverSound;

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
        UIManager.current.WonLevelUI();
    }

    public void LostLevel() {
        if (_gameHasEnded) return;
        _gameHasEnded = true;
        gameOverSound.Play();
        _playerMovement.enabled = false;
        UIManager.current.LostLevelUI();
    }

    private void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        _gameHasEnded = false;
    }

    public int GetAudioTempo()
    {
        return _audioTempo;
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
        _audioTempo = Math.Min(_audioTempo + 1, 1); 
    }

    public void DecreaseAudioTempo()
    {
        _audioTempo = Math.Max(_audioTempo - 1, -1);

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
