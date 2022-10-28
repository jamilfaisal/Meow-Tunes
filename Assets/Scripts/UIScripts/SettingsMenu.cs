using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer masterAudioMixer;
    public TMP_Text masterAudioTextValue;

    public void SetMasterVolume(float volume)
    {
        string masterTextValue = Mathf.RoundToInt(volume + 80) + "%";
        masterAudioTextValue.text = masterTextValue;
        masterAudioMixer.SetFloat("musicVolume", volume);
    }

    public void SetQualitySetting(int optionIndex)
    {
        QualitySettings.SetQualityLevel(optionIndex);
    }

    public void SetFullscreen(bool toggleOn)
    {
        Screen.fullScreen = toggleOn;
    }
}
