using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public static SettingsMenu current;

    private void Awake()
    {
        current = this;
    }

    public AudioSource musicIntro, musicLoop;
    public AudioSource catJump4;
    public GameObject settingsMenu;
    public TMP_Text musicAudioTextValue;
    public Slider musicAudioSlider;
    public TMP_Text soundEffectsTextValue;
    public Slider soundEffectsAudioSlider;
    public TMP_Dropdown qualitySettingDropdown;
    public Toggle fullScreenToggle;

    public void SetMusicVolume(float volume)
    {
        UpdateMusicVolumeText(volume);
        musicIntro.volume = volume;
        musicLoop.volume = volume;
        SavePlayerPrefsMusicVolume(volume);
    }

    public void SetSoundEffectVolume(float volume)
    {
        UpdateSoundEffectVolumeText(volume);
        catJump4.volume = volume;
        if (!catJump4.isPlaying && settingsMenu.activeInHierarchy)
        {
            catJump4.Play();
        }
        SavePlayerPrefsSoundEffectVolume(volume);
    }

    public void SetQualitySetting(int optionIndex)
    {
        QualitySettings.SetQualityLevel(optionIndex);
        SavePlayerPrefsQualitySetting(optionIndex);
    }

    public void SetFullscreen(bool toggleOn)
    {
        Screen.fullScreen = toggleOn;
        SavePlayerPrefsFullscreen(toggleOn);
    }

    public void LoadPlayerPrefs()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadPlayerPrefsMusicVolume();
        }

        if (PlayerPrefs.HasKey("soundEffectVolume"))
        {
            LoadPlayerPrefsSoundEffectsVolume();
        }

        if (PlayerPrefs.HasKey("qualitySetting"))
        {
            LoadPlayerPrefsQualitySetting();
        }

        if (PlayerPrefs.HasKey("fullscreen"))
        {
            LoadPlayerPrefsFullscreen();
        }
    }
    private void UpdateMusicVolumeSliderAndText(float volume)
    {
        UpdateMusicVolumeText(volume);
        musicAudioSlider.value = volume;
    }
    
    private void UpdateMusicVolumeText(float volume)
    {
        var volumeText = Mathf.RoundToInt(volume * 100) + "%";
        musicAudioTextValue.text = volumeText;
    }
    
    private void UpdateSoundEffectVolumeSliderAndText(float volume)
    {
        UpdateSoundEffectVolumeText(volume);
        soundEffectsAudioSlider.value = volume;
    }
    
    private void UpdateSoundEffectVolumeText(float volume)
    {
        var volumeText = Mathf.RoundToInt(volume * 100) + "%";
        soundEffectsTextValue.text = volumeText;
    }

    private void LoadPlayerPrefsMusicVolume()
    {
        var musicVolume = PlayerPrefs.GetFloat("musicVolume");
        musicIntro.volume = musicVolume;
        musicLoop.volume = musicVolume;
        UpdateMusicVolumeSliderAndText(musicVolume);
    }
    
    private void SavePlayerPrefsMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
    
    private void LoadPlayerPrefsSoundEffectsVolume()
    {
        var soundEffectVolume = PlayerPrefs.GetFloat("soundEffectVolume");
        UpdateSoundEffectVolumeSliderAndText(soundEffectVolume);    
    }

    private void SavePlayerPrefsSoundEffectVolume(float volume)
    {
        PlayerPrefs.SetFloat("soundEffectVolume", volume);
    }

    private void LoadPlayerPrefsQualitySetting()
    {
        var qualitySetting = PlayerPrefs.GetInt("qualitySetting");
        qualitySettingDropdown.value = qualitySetting;
    }
    
    private void SavePlayerPrefsQualitySetting(int qualitySetting)
    {
        PlayerPrefs.SetInt("qualitySetting", qualitySetting);
    }

    private void LoadPlayerPrefsFullscreen()
    {
        var fullscreen = PlayerPrefs.GetInt("fullscreen");
        fullScreenToggle.isOn = IntToBool(fullscreen);
    }

    private void SavePlayerPrefsFullscreen(bool fullscreen)
    {
        PlayerPrefs.SetInt("fullscreen", BoolToInt(fullscreen));
    }
    
    private int BoolToInt(bool val)
    {
        return val ? 1 : 0;
    }

    private bool IntToBool(int val)
    {
        return val != 0;
    }
}
