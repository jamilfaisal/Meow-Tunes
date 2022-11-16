using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public AudioSource checkpointSound;

    private void Start()
    {
        var checkpointSoundGameObject = GameObject.FindGameObjectWithTag("checkpointSound");
        if (checkpointSoundGameObject != null)
        {
            checkpointSound = checkpointSoundGameObject.GetComponent<AudioSource>();
        }
        else
        {
            throw new Exception("No gameobject with checkpointSound tag in the scene");
        }
    }

    private void OnTriggerEnter(Collider otherCollider)
    {
        if (!otherCollider.gameObject.CompareTag("Player")) return;
        Destroy(gameObject);
        RespawnManager.current.SetMidiTime(MidiManager.current.GetPlaybackTime());
        RespawnManager.current.SetMusicTime(MusicPlayer.current.audioSource.time);
        RespawnManager.current.SetRespawnPoint(otherCollider.gameObject.transform.position);
        checkpointSound.Play();
    }
}
