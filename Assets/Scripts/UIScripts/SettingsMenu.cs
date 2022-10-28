using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.ProBuilder;

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

}
