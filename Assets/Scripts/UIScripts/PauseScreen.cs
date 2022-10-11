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
        if (Input.GetAxis("Mouse X") != 0 && gameManager.IsGamePaused())
        {
            Cursor.visible = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !gameManager.HasGameEnded())
        {
            if (gameManager.IsGamePaused())
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
        Invoke(nameof(EnableMovement), 0.3f);
        Time.timeScale = 1f;
        gameManager.ResumeGame();
        TimerManager.current.ResumeTimer();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Pause()
    {
        TimerManager.current.StopTimer();
        pauseMenuUI.SetActive(true);
        musicPlayer.Pause();
        _playerMovementScript.enabled = false;
        walkingSound.Stop();
        Time.timeScale = 0f;
        gameManager.PauseGame();
        // This is because the camera script locks the cursor,
        // so we need to enable it again to be able to click buttons
        Cursor.lockState = CursorLockMode.None;
    }

    public void PauseOrResumeController()
    {
        if (gameManager.HasGameEnded()) return;
        if (gameManager.IsGamePaused())
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
