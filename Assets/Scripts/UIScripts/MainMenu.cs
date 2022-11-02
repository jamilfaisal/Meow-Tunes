using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.Switch;
using UnityEngine.InputSystem.XInput;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioSource musicIntro, musicLoop;
    public GameObject ps4Prompt, xboxPrompt, pcPrompt;
    private Gamepad _gamepad;
    // First button that is highlighted when player navigates to the main menu
    public GameObject mainMenuFirstButton, settingsFirstButton;

    private void Awake()
    {
        SettingsMenu.current.VolumeChanged += SetVolumes;
    }

    public void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(mainMenuFirstButton);
        musicIntro.Play();
        musicLoop.PlayDelayed(musicIntro.clip.length);
        
        _gamepad = Gamepad.current;

    }

    public void Update()
    {
        PressButtonToSelect();
    }

    private void PressButtonToSelect()
    {
        _gamepad = Gamepad.current;
        var lastUpdated = CheckLastUpdated();
        if (_gamepad == null || lastUpdated == "mouse")
        {
            ps4Prompt.SetActive(false);
            xboxPrompt.SetActive(false);
            pcPrompt.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else if (lastUpdated == "keyboard")
        {
            ps4Prompt.SetActive(false);
            xboxPrompt.SetActive(false);
            pcPrompt.SetActive(true);
        }
        else
        {
            switch (_gamepad)
            {
                case XInputController or SwitchProControllerHID:
                    ps4Prompt.SetActive(false);
                    pcPrompt.SetActive(false);
                    xboxPrompt.SetActive(true);
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    break;
                case DualShockGamepad:
                    xboxPrompt.SetActive(false);
                    pcPrompt.SetActive(false);
                    ps4Prompt.SetActive(true);
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    break;
            }
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

    private void SetVolumes(float musicVolume, float soundEffectVolume)
    {
        musicIntro.volume = musicVolume;
        musicLoop.volume = musicVolume;
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

    public void OpenSettingsMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(settingsFirstButton);
    }
}
