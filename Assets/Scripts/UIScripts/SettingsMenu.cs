using System;
using TMPro;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public static SettingsMenu current;

    private void Awake()
    {
        current = this;
    }

    public AudioSource musicIntro;
    public AudioSource musicLoop;
    public TMP_Text masterAudioTextValue;

    private void Start()
    {
        UpdateMusicVolumeText(musicIntro.volume);
    }

    public void SetMasterVolume(float volume)
    {
        UpdateMusicVolumeText(volume);
        musicIntro.volume = volume;
        musicLoop.volume = volume;
        SavePlayerPrefs();
    }

    private void UpdateMusicVolumeText(float volume)
    {
        var masterTextValue = Mathf.RoundToInt(volume * 100) + "%";
        masterAudioTextValue.text = masterTextValue;

    }

    public void SetQualitySetting(int optionIndex)
    {
        QualitySettings.SetQualityLevel(optionIndex);
    }

    public void SetFullscreen(bool toggleOn)
    {
        Screen.fullScreen = toggleOn;
    }

    public void LoadPlayerPrefs()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadPlayerPrefsMusicVolume();
        }
    }

    private void LoadPlayerPrefsMusicVolume()
    {
        float musicVolume = PlayerPrefs.GetFloat("musicVolume");
        musicIntro.volume = musicVolume;
        musicLoop.volume = musicVolume;
        UpdateMusicVolumeText(musicVolume);
    }

    private void SavePlayerPrefs()
    {
        PlayerPrefs.SetFloat("musicVolume", musicIntro.volume);
    }
}
