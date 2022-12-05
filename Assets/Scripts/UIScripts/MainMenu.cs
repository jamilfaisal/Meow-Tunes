using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.Switch;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.XInput;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioSource musicIntro, musicLoop;
    public GameObject ps4Prompt, xboxPrompt, pcPrompt;
    private Gamepad _gamepad;
    public PlayerInput playerInput;
    // First button that is highlighted when player navigates to the main menu
    public GameObject mainMenuFirstButton, settingsFirstButton;
    public GameObject loadingScreen;

    private void Awake()
    {
        SettingsMenu.current.VolumeChanged += SetVolumes;
        CheckIfPlayerUsingController();
    }

    public void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(mainMenuFirstButton);
        musicIntro.Play();
        musicLoop.PlayDelayed(musicIntro.clip.length);
        
    }
    

    private void CheckIfPlayerUsingController()
    {
        PressButtonToSelect(CheckLastUpdated());
        InputUser.onChange += (_, change, _) =>
        {
            if (change is InputUserChange.ControlSchemeChanged)
            {
                PressButtonToSelect(CheckLastUpdated());
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
        else
        {
            return "keyboard";
        }
    }

    private void PressButtonToSelect(string lastUpdated)
    {
        _gamepad = Gamepad.current;
        if (lastUpdated == "keyboard")
        {
            ps4Prompt.SetActive(false);
            xboxPrompt.SetActive(false);
            pcPrompt.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
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

    private void SetVolumes(float musicVolume, float soundEffectVolume)
    {
        musicIntro.volume = musicVolume;
        musicLoop.volume = musicVolume;
    }

    public void PlayGame()
    {
        loadingScreen.SetActive(true);
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
