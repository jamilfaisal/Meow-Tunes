using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.Switch;
using UnityEngine.InputSystem.XInput;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    public GameObject pauseMenuUI, settingsMenuUI;
    public Conductor musicPlayer;
    public GameObject playerMovement;
    private PlayerMovement _playerMovementScript;
    public GameObject pauseScreenFirstButton, settingsFirstButton;

    private void Start()
    {
        _playerMovementScript = playerMovement.GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (GameManager.current.IsGamePaused())
        {
            StartCoroutine(nameof(ShowOrHideCursor));
        }
        
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
    
    private IEnumerator ShowOrHideCursor() {
        for(;;) {
            var gamepad = Gamepad.current;
            var lastUpdated = CheckLastUpdated();
            if (gamepad == null || lastUpdated == "mouse")
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            } else if (lastUpdated == "gamepad")
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }            
            yield return new WaitForSeconds(0.5f);
        }
    }
    
    private string CheckLastUpdated()
    {
        var gamepadLastUpdateTime = Gamepad.current.lastUpdateTime;
        var keyboardLastUpdateTime = Keyboard.current.lastUpdateTime;
        var mouseLastUpdateTime = Mouse.current.lastUpdateTime;
        if (keyboardLastUpdateTime > gamepadLastUpdateTime && keyboardLastUpdateTime > mouseLastUpdateTime)
        {
            return "keyboard";
        } else if (mouseLastUpdateTime > keyboardLastUpdateTime && mouseLastUpdateTime > gamepadLastUpdateTime)
        {
            return "mouse";
        }
        else
        {
            return "gamepad";
        }
    }
    
    

    public void Resume()
    {
        StopCoroutine(nameof(ShowOrHideCursor));
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

    public void Pause()
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
