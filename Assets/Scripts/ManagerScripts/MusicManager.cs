using System;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager current;

    private void Awake()
    {
        current = this;
        SettingsMenu.current.VolumeChanged += SetVolumes;
    }
    public AudioSource music, gameOver;
    public AudioSource jump1, jump2, jump3, jump4, landJump, catWalking, reachedCheckpoint, catRespawn;
    private void SetVolumes(float musicVolume, float soundEffectVolume)
    {
        music.volume = musicVolume;
        gameOver.volume = musicVolume;
        
        jump1.volume = soundEffectVolume;
        jump2.volume = soundEffectVolume;
        jump3.volume = soundEffectVolume;
        jump4.volume = soundEffectVolume;
        landJump.volume = soundEffectVolume / 3;
        catWalking.volume = soundEffectVolume / 3;
        reachedCheckpoint.volume = soundEffectVolume;
        catRespawn.volume = soundEffectVolume;
    }
}
