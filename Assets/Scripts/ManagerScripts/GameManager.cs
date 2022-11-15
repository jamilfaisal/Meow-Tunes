using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager current;

    private static bool _gameIsPaused;
    public bool playerIsDying;
    private bool _gameIsRestarting;
    private bool _gameHasEnded;
    public bool gameIsEnding;
    public GameObject playerGameObject;
    private PlayerMovement _playerMovement;
    public AudioSource gameOverSound;
    
    public Animator animator;

    private void Awake()
    {
        animator = playerGameObject.GetComponent<Animator>();
        _playerMovement = playerGameObject.GetComponent<PlayerMovement>();
        current = this;
        _gameHasEnded = false;
        _playerMovement.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update() {
        if (Input.GetButton("Submit") && _gameHasEnded) {
            RestartLevel();
        }
    }
    public void WonLevel() {
        animator.Play("CatCelebrating", 0, 0f);
        gameIsEnding = false;
        _playerMovement.enabled = false;
        UIManager.current.WonLevelUI();
        StartCoroutine(GameHasEnded());
    }

    public void LostLevel() {
        gameIsEnding = false;
        PlayerMovement.current.walkingSound.Stop();
        MusicPlayer.current.audioSource.Pause();
        gameOverSound.Play();
        _playerMovement.enabled = false;
        UIManager.current.LostLevelUI();
        StartCoroutine(GameHasEnded());
    }
    

    private IEnumerator GameHasEnded()
    {
        yield return new WaitForSeconds(3f);
        _gameHasEnded = true;
    }

    public void RestartLevel()
    {
        _gameIsRestarting = true;
        MidiManager.current.RestartLevel();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }

    public void BackToMainMenu()
    {
        MidiManager.current.RestartLevel();
        SceneManager.LoadScene("MainMenuScene");
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _gameIsRestarting = false;
        _gameIsPaused = false;
        gameIsEnding = false;
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

    public bool IsGameRestarting()
    {
        return _gameIsRestarting;
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
