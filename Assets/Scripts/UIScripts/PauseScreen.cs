using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class PauseScreen : MonoBehaviour
{
    public GameObject pauseMenuUI, settingsMenuUI;
    public Conductor musicPlayer;
    public GameObject playerMovement;
    private PlayerMovement _playerMovementScript;
    public PlayerInput playerInput;
    private string _lastUpdated;
    public GameObject pauseScreenFirstButton, settingsFirstButton;

    private void Awake()
    {
        CheckIfPlayerUsingController();
    }
    
    private void CheckIfPlayerUsingController()
    {
        ShowOrHideCursor(CheckLastUpdated());
        InputUser.onChange += (_, change, _) =>
        {
            if (change is InputUserChange.ControlSchemeChanged)
            {
                ShowOrHideCursor(CheckLastUpdated());
            }
        };
    }
    
    private string CheckLastUpdated()
    {
        if (playerInput.currentControlScheme.ToLower().Contains("gamepad") ||
            playerInput.currentControlScheme.ToLower().Contains("joystick"))
        {
            return "gamepad";
        }
        return "keyboard";
    }

    private void Start()
    {
        _playerMovementScript = playerMovement.GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !GameManager.current.HasGameEnded())
        {
            if (GameManager.current.IsGamePaused())
            {
                if (!settingsMenuUI.activeInHierarchy)
                {
                    Resume();
                }
                else
                {
                    SettingsMenu.current.BackToMainMenuOrPauseScreen();
                    settingsMenuUI.SetActive(false);
                    pauseMenuUI.SetActive(true);
                }
            } else {
                Pause();
            }
        } 
    }
    
    private void ShowOrHideCursor(string lastUpdated) {
        if (lastUpdated == "keyboard")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else if (lastUpdated == "gamepad")
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }            
    }


    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(false);
        MidiManager.current.ResumePlayback();
        musicPlayer.Resume();
        Invoke(nameof(EnableMovement), 0.3f);
        Time.timeScale = 1f;
        GameManager.current.ResumeGame();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Pause()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseScreenFirstButton);
        pauseMenuUI.SetActive(true);
        musicPlayer.Pause();
        MidiManager.current.PausePlayback();
        _playerMovementScript.enabled = false;
        PlayerMovement.current.walkingSound.Stop();
        Time.timeScale = 0f;
        GameManager.current.PauseGame();
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
