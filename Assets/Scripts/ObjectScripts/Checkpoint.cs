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
    private int playerFishScore;
    private int playerAccuracyScore;

    private int inputIndexSBA; // Single Button Action
    private int inputIndexPSA; // Player Side Action
    private int inputIndexRightPSA; // PSA Input index right

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

        playerFishScore = scoreManager.GetPlayerFishScore();
        RespawnManager.current.SetPlayerFishScore(playerFishScore);
        playerAccuracyScore = scoreManager.GetPlayerAccuracyScore();
        RespawnManager.current.SetPlayerAccuracyScore(playerAccuracyScore);

        inputIndexSBA = SingleButtonAction.Current.GetInputIndex();
        RespawnManager.current.SetInputIndexSBA(inputIndexSBA);
        inputIndexPSA = SingleButtonAction.Current.GetInputIndex();
        RespawnManager.current.SetInputIndexPSA(inputIndexPSA);
        inputIndexRightPSA = SingleButtonAction.Current.GetInputIndex();
        RespawnManager.current.SetInputIndexRightPSA(inputIndexRightPSA);

        var position = transform.position;
        position.y += catRspawnOffsetY;
        position.x -= catRspawnOffsetX;
        position.z -= catRspawnOffsetZ;
        RespawnManager.current.SetRespawnPoint(position, laneNumber);

        checkpointSound.Play();
    }
}
