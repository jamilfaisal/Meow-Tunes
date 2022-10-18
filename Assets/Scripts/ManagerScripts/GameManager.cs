using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager current;

    private static bool _gameIsPaused;
    public bool playerIsDying;
    private bool _gameHasEnded;
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
        Conductor.current.audioSource.Pause();
        gameOverSound.Play();
        _playerMovement.enabled = false;
        UIManager.current.LostLevelUI();
    }

    private void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        _gameHasEnded = false;
    }

    public void StartLevel()
    {
        if (_gameHasEnded)
        {
            RestartLevel();
        }
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
