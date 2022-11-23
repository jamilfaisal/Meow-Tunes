using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    public GameObject pauseMenuUI, settingsMenuUI;
    public MusicPlayer musicPlayer;
    public GameObject playerMovement;
    private PlayerMovement _playerMovementScript;
    public GameObject pauseScreenFirstButton, settingsFirstButton;

    private void Start()
    {
        _playerMovementScript = playerMovement.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (Input.GetAxis("Mouse X") != 0 && GameManager.current.IsGamePaused())
        {
            Cursor.visible = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !GameManager.current.HasGameEnded())
        {
            if (GameManager.current.IsGamePaused() && !settingsMenuUI.activeInHierarchy)
            {
                Resume();
            } else if (GameManager.current.IsGamePaused() && settingsMenuUI.activeInHierarchy)
            {
                SettingsMenu.current.BackToMainMenuOrPauseScreen();
                settingsMenuUI.SetActive(false);
                pauseMenuUI.SetActive(true);
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
        settingsMenuUI.SetActive(false);
        //MidiManager.current.ResumePlayback();
        if (Time.time > 5)
        {
            musicPlayer.Resume();
        }
        Invoke(nameof(EnableMovement), 0.3f);
        Time.timeScale = 1f;
        GameManager.current.ResumeGame();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Pause()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseScreenFirstButton);
        pauseMenuUI.SetActive(true);
        musicPlayer.Pause();
        //MidiManager.current.PausePlayback();
        _playerMovementScript.enabled = false;
        PlayerMovement.current.walkingSound.Stop();
        Time.timeScale = 0f;
        GameManager.current.PauseGame();
        // This is because the camera script locks the cursor,
        // so we need to enable it again to be able to click buttons
        Cursor.lockState = CursorLockMode.None;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameManager.current.RestartLevel();
    }
    public void MainMenu()
    {
        Time.timeScale = 1f;
        GameManager.current.BackToMainMenu();
    }
    public void PauseOrResumeController()
    {
        if (GameManager.current.HasGameEnded()) return;
        if (GameManager.current.IsGamePaused())
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void OpenSettingsMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(settingsFirstButton);
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