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
