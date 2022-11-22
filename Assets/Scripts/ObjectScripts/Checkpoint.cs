using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public AudioSource checkpointSound;
    public int laneNumber;

    private float _catRspawnOffsetY;

    private void Start()
    {
        var checkpointSoundGameObject = GameObject.FindGameObjectWithTag("checkpointSound");
        _catRspawnOffsetY = 2f;
        if (checkpointSoundGameObject != null)
        {
            checkpointSound = checkpointSoundGameObject.GetComponent<AudioSource>();
        }
        else
        {
            throw new Exception("No game object with checkpointSound tag in the scene");
        }
    }
    
    private void OnTriggerEnter(Collider otherCollider)
    {
        if (!otherCollider.gameObject.CompareTag("Player")) return;
        Destroy(gameObject);
        RespawnManager.current.SetMidiTime(MidiManager.current.GetPlaybackTime());
        RespawnManager.current.SetMusicTime(MusicPlayer.current.audioSource.time);
        RespawnManager.current.SetRespawnPoint(
            PlayerSyncPosition.Current.GetPlayerPosMusicTimeSyncedPosition(transform.position.y + _catRspawnOffsetY),
            laneNumber);
        checkpointSound.Play();
    }
}
