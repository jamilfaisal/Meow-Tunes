using UnityEngine;
using UnityEngine.EventSystems;

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
        if (Input.GetAxis("Mouse X") != 0 && GameManager.Current.IsGamePaused())
        {
            Cursor.visible = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !GameManager.Current.HasGameEnded())
        {
            if (GameManager.Current.IsGamePaused() && !settingsMenuUI.activeInHierarchy)
            {
                Resume();
            } else if (GameManager.Current.IsGamePaused() && settingsMenuUI.activeInHierarchy)
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
        GameManager.Current.ResumeGame();
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
        PlayerMovement.Current.walkingSound.Stop();
        Time.timeScale = 0f;
        GameManager.Current.PauseGame();
        // This is because the camera script locks the cursor,
        // so we need to enable it again to be able to click buttons
        Cursor.lockState = CursorLockMode.None;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameManager.Current.RestartLevel();
    }
    public void MainMenu()
    {
        Time.timeScale = 1f;
        GameManager.Current.BackToMainMenu();
    }
    public void PauseOrResumeController()
    {
        if (GameManager.Current.HasGameEnded()) return;
        if (GameManager.Current.IsGamePaused())
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