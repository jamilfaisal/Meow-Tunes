using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public AudioSource checkpointSound;
    // public AudioClip checkpointSound;

    private void Awake()
    {
        checkpointSound = GameObject.Find("Reached Checkpoint").GetComponent<AudioSource>();
    }
    
    private void OnTriggerEnter(Collider otherCollider)
    {
        if (!otherCollider.gameObject.CompareTag("Player")) return;
        Destroy(gameObject);
        RespawnManager.current.SetMidiTime(MidiManager.current.GetPlaybackTime());
        RespawnManager.current.SetMusicTime(MusicPlayer.current.audioSource.time);
        RespawnManager.current.SetRespawnPoint(otherCollider.gameObject.transform.position);

        checkpointSound.Play();
        // AudioSource.PlayClipAtPoint(checkpointSound, this.gameObject.transform.position);
    }
}
