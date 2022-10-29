using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.Switch;
using UnityEngine.InputSystem.XInput;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioSource musicIntro; 
    public AudioSource musicLoop;
    public GameObject ps4Prompt;
    public GameObject xboxPrompt;
    public GameObject pcPrompt;
    private Gamepad _gamepad;

    public void Start()
    {
        SettingsMenu.current.LoadPlayerPrefs();
        musicIntro.Play();
        musicLoop.PlayDelayed(musicIntro.clip.length);
        
        _gamepad = Gamepad.current;

        if (_gamepad != null)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

    }

    public void Update()
    {
        
        _gamepad = Gamepad.current;
        switch (_gamepad)
        {
            case null:
                ps4Prompt.SetActive(false);
                xboxPrompt.SetActive(false);
                pcPrompt.SetActive(true);
                break;
            case XInputController or SwitchProControllerHID:
                ps4Prompt.SetActive(false);
                pcPrompt.SetActive(false);
                xboxPrompt.SetActive(true);
                break;
            case DualShockGamepad:
                xboxPrompt.SetActive(false);
                pcPrompt.SetActive(false);
                ps4Prompt.SetActive(true);
                break;
        }

    }

    public void PlayGame()
    {
        SceneManager.LoadScene("LevelOneScene");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting...");
    }
}
