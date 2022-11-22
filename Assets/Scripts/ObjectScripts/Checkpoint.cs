using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public AudioSource checkpointSound;
    public int laneNumber;

    private float catRspawnOffsetX;
    private float catRspawnOffsetY;
    private float catRspawnOffsetZ;

    private void Start()
    {
        var checkpointSoundGameObject = GameObject.FindGameObjectWithTag("checkpointSound");
        catRspawnOffsetX = 0.6f;
        catRspawnOffsetY = 5f;
        catRspawnOffsetZ = 0.7f;
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
        var position = transform.position;
        position.y += catRspawnOffsetY;
        position.x -= catRspawnOffsetX;
        position.z -= catRspawnOffsetZ;
        RespawnManager.current.SetRespawnPoint(position, laneNumber);

        checkpointSound.Play();
    }
}
