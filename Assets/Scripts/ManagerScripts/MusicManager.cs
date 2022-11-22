using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private void Awake()
    {
        SettingsMenu.current.VolumeChanged += SetVolumes;
    }
    public AudioSource music, gameOver;
    public AudioSource jump1, jump2, jump3, jump4, landJump, catWalking, reachedCheckpoint, catRespawn, catTeleport;
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
        catTeleport.volume = soundEffectVolume;
    }
}
