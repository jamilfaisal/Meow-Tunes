using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public AudioSource checkpointSound;
    public GameObject Player;
    public Animator animator;

    private void Start()
    {
        Player = GameObject.Find("Player");
        animator = Player.GetComponent<Animator>();
    }
    
    private void OnTriggerEnter(Collider otherCollider)
    {
        if (!otherCollider.gameObject.CompareTag("Player")) return;
        //animator.Play("CatCheckpointCycle", 0, 0f);
        Destroy(gameObject);
        RespawnManager.current.SetMidiTime(MidiManager.current.GetPlaybackTime());
        RespawnManager.current.SetMusicTime(MusicPlayer.current.audioSource.time);
        RespawnManager.current.SetRespawnPoint(otherCollider.gameObject.transform.position);
        checkpointSound.Play();
    }
}
