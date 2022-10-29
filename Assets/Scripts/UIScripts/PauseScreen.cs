using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;
    public Conductor musicPlayer;
    public GameObject playerMovement;
    private PlayerMovement _playerMovementScript;


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
            if (GameManager.current.IsGamePaused())
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
        pauseMenuUI.SetActive(true);
        musicPlayer.Pause();
        MidiManager.current.PausePlayback();
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
