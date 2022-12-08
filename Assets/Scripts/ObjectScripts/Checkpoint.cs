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
    public SingleButtonAction jumpAction;
    public SingleButtonAction stompAction;
    public PlayerSideAction sideAction;
    private int playerFishScore;
    private int playerAccuracyScore;

    private int inputIndexJA; // Jump Action
    private int inputIndexPSA; // Player Side Action
    private int inputIndexRightPSA; // PSA Input index right
    private int inputIndexSA; // Stomp Action

    private int hopIndex;

    private void Start()
    {
        Player = GameObject.Find("Player");
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        jumpAction = GameObject.Find("ActionJump").GetComponent<SingleButtonAction>();
        sideAction = GameObject.Find("ActionSide").GetComponent<PlayerSideAction>();
        stompAction = GameObject.Find("ActionStomp").GetComponent<SingleButtonAction>();
        animator = Player.GetComponent<Animator>();
        var checkpointSoundGameObject = GameObject.FindGameObjectWithTag("checkpointSound");

        playerFishScore = scoreManager.GetPlayerFishScore();
        playerAccuracyScore = scoreManager.GetPlayerAccuracyScore();
        inputIndexJA = jumpAction.GetInputIndex();
        inputIndexPSA = sideAction.GetInputIndex();
        inputIndexRightPSA = sideAction.GetInputIndexRight();
        inputIndexSA = stompAction.GetInputIndex();
        hopIndex = PlayerHopManager.Current.GetHopIndex();
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

        inputIndexJA = jumpAction.GetInputIndex();
        RespawnManager.Current.SetInputIndexJA(inputIndexJA);
        inputIndexPSA = sideAction.GetInputIndex();
        RespawnManager.Current.SetInputIndexPSA(inputIndexPSA);
        inputIndexRightPSA = sideAction.GetInputIndex();
        RespawnManager.Current.SetInputIndexRightPSA(inputIndexRightPSA);
        inputIndexSA = stompAction.GetInputIndex();
        RespawnManager.Current.SetInputIndexSA(inputIndexSA);

        hopIndex = PlayerHopManager.Current.GetHopIndex();
        RespawnManager.Current.SetHopIndex(hopIndex);

        RespawnManager.Current.SetRespawnPoint(
            PlayerSyncPosition.Current.GetPlayerPosMusicTimeSyncedPosition(transform.position.y + _catRspawnOffsetY),
            laneNumber);
        checkpointSound.Play();
    }
}
