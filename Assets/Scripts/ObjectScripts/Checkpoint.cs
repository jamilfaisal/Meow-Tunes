using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public AudioSource checkpointSound;
    public GameObject Player;
    public Animator animator;
    public int laneNumber;

    private float _catRspawnOffsetY;

    public ScoreManager scoreManager;
    private int playerFishScore;
    private int playerAccuracyScore;

    private int inputIndexSBA; // Single Button Action
    private int inputIndexPSA; // Player Side Action
    private int inputIndexRightPSA; // PSA Input index right

    private int hopIndex;

    private void Start()
    {
        Player = GameObject.Find("Player");
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        animator = Player.GetComponent<Animator>();
        var checkpointSoundGameObject = GameObject.FindGameObjectWithTag("checkpointSound");

        playerFishScore = scoreManager.GetPlayerFishScore();
        playerAccuracyScore = scoreManager.GetPlayerAccuracyScore();
        inputIndexSBA = SingleButtonAction.Current.GetInputIndex();
        inputIndexPSA = PlayerSideAction.Current.GetInputIndex();
        inputIndexRightPSA = PlayerSideAction.Current.GetInputIndexRight();
        hopIndex = PlayerHop.Current.GetHopIndex();
        _catRspawnOffsetY = 3f;
        
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
        // TODO: Add revised checkpoint animation
        //animator.Play("CatCheckpointCycle", 0, 0f);
        Destroy(gameObject);
        //RespawnManager.current.SetMidiTime(MidiManager.current.GetPlaybackTime());
        RespawnManager.Current.SetMusicTime(MusicPlayer.Current.audioSource.time);

        playerFishScore = scoreManager.GetPlayerFishScore();
        RespawnManager.Current.SetPlayerFishScore(playerFishScore);
        playerAccuracyScore = scoreManager.GetPlayerAccuracyScore();
        RespawnManager.Current.SetPlayerAccuracyScore(playerAccuracyScore);

        inputIndexSBA = SingleButtonAction.Current.GetInputIndex();
        RespawnManager.Current.SetInputIndexSBA(inputIndexSBA);
        inputIndexPSA = SingleButtonAction.Current.GetInputIndex();
        RespawnManager.Current.SetInputIndexPSA(inputIndexPSA);
        inputIndexRightPSA = SingleButtonAction.Current.GetInputIndex();
        RespawnManager.Current.SetInputIndexRightPSA(inputIndexRightPSA);

        hopIndex = PlayerHop.Current.GetHopIndex();
        RespawnManager.Current.SetHopIndex(hopIndex);

        RespawnManager.Current.SetRespawnPoint(
            PlayerSyncPosition.Current.GetPlayerPosMusicTimeSyncedPosition(transform.position.y + _catRspawnOffsetY),
            laneNumber);
        checkpointSound.Play();
    }
}
