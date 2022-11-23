using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public AudioSource checkpointSound;
    public GameObject Player;
    public Animator animator;
    public int laneNumber;

    private float catRspawnOffsetX;
    private float catRspawnOffsetY;
    private float catRspawnOffsetZ;

    public ScoreManager scoreManager;
    private int playerAccuracyScore;

    private void Start()
    {
        Player = GameObject.Find("Player");
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        animator = Player.GetComponent<Animator>();
        var checkpointSoundGameObject = GameObject.FindGameObjectWithTag("checkpointSound");
        playerAccuracyScore = scoreManager.GetPlayerAccuracyScore();
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
        // TODO: Add revised checkpoint animation
        //animator.Play("CatCheckpointCycle", 0, 0f);
        Destroy(gameObject);
        RespawnManager.current.SetMidiTime(MidiManager.current.GetPlaybackTime());
        RespawnManager.current.SetMusicTime(MusicPlayer.current.audioSource.time);

        playerAccuracyScore = scoreManager.GetPlayerAccuracyScore();
        RespawnManager.current.SetPlayerAccuracyScore(playerAccuracyScore);

        var position = transform.position;
        position.y += catRspawnOffsetY;
        position.x -= catRspawnOffsetX;
        position.z -= catRspawnOffsetZ;
        RespawnManager.current.SetRespawnPoint(position, laneNumber);

        checkpointSound.Play();
    }
}
