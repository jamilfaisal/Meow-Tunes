using System;
using System.Collections;
using Melanchall.DryWetMidi.Interaction;
using UnityEngine;


public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Current;

    private void Awake()
    {
        Current = this;
    }

    private Vector3 _respawnPointLocation;
    public int respawnLane;
    private float _musicTime;

    [SerializeField] public GameObject playerCharacter;
    private Rigidbody _playerCharacterRb;
    private PlayerMovement _playerCharacterMovement;
    public SingleButtonAction jumpAction;
    public PlayerSideAction sideAction;
    public SingleButtonAction stompAction;

    private int _playerFishScore;
    private int _playerAccuracyScore;

    private int _inputIndexJA; // Jump Action
    private int _inputIndexPSA; // Player Side Action
    private int _inputIndexSA; // Stomp Action
    private int _inputIndexRightPSA; // PSA Input index right

    private int _hopIndex;

    private void Start()
    {
        _respawnPointLocation = playerCharacter.transform.position;
        respawnLane = 3;
        _playerCharacterRb = playerCharacter.GetComponent<Rigidbody>();
        _playerCharacterMovement = playerCharacter.GetComponent<PlayerMovement>();
        _playerFishScore = ScoreManager.current.GetPlayerFishScore();
        _playerAccuracyScore = ScoreManager.current.GetPlayerAccuracyScore();
        _inputIndexJA = jumpAction.GetInputIndex();
        _inputIndexPSA = sideAction.GetInputIndex();
        _inputIndexRightPSA = sideAction.GetInputIndexRight();
        _inputIndexSA = stompAction.GetInputIndex();
        _hopIndex = PlayerHopManager.Current.GetHopIndex();
    }

    public IEnumerator RespawnPlayer(float respawnClipLength)
    {
        GameManager.Current.playerIsDying = true;
        
        // Reset values
        AdjustMusicTime();
        //AdjustMidiTime();
        yield return new WaitForSeconds(respawnClipLength - 5f);
        // StartCoroutine(RespawnPlayerAfterCountdown());
        AdjustPlayerPosition();
        _playerCharacterMovement.enabled = false;
        //PlayerMovement.Current.SetPlayerInputEnabled(false);
        PlayerMovement.Current.walkingSound.Stop();
        // Reset Fish Treats on the lanes
        MusicPlayer.Current.ResetAllFishTreats();
        ScoreManager.current.SetAndUpdateFishScore(_playerFishScore);
        ScoreManager.current.SetAndUpdatePlayerAccuracyScore(_playerAccuracyScore);
        jumpAction.SetInputIndex(_inputIndexJA);
        sideAction.SetInputIndex(_inputIndexPSA);
        sideAction.SetInputIndexRight(_inputIndexRightPSA);
        stompAction.SetInputIndex(_inputIndexSA);
        PlayerHopManager.Current.SetHopIndex(_hopIndex);

        CountdownManager.Current.SetCountdown(3f);
        yield return new WaitForSeconds(3f);
        _playerCharacterMovement.enabled = true;
        // Reset music related values
        MusicPlayer.Current.Resume();
        //MidiManager.current.ResumePlayback();
        GameManager.Current.playerIsDying = false;
        yield return null;
    }

    private void AdjustMusicTime()
    {
        MusicPlayer.Current.Pause();
        MusicPlayer.Current.audioSource.time = _musicTime;
    }

    private void AdjustPlayerPosition()
    {
        playerCharacter.transform.position = _respawnPointLocation;
        playerCharacter.transform.rotation = new Quaternion(0,0,0,0);
        _playerCharacterRb.velocity = new Vector3(0,0,1f);
        _playerCharacterMovement.currentLane = respawnLane;
    }

    // private void AdjustMidiTime()
    // {
    //     MidiManager.current.PausePlayback();
    //     MidiManager.current.AdjustMidiTime(_midiTime);
    // }

    public void SetRespawnPoint(Vector3 newRespawnPoint, int newRespawnLane) {
        _respawnPointLocation = newRespawnPoint;
        respawnLane = newRespawnLane;
    }

    public void SetMusicTime(float musicTime)
    {
        _musicTime = musicTime;
    }

    public void SetMidiTime(ITimeSpan midiTime)
    {
    }

    public void SetPlayerFishScore(int score)
    {
        _playerFishScore = score;
    }

    public void SetPlayerAccuracyScore(int score)
    {
        _playerAccuracyScore = score;
    }

    public void SetInputIndexJA(int inputI)
    {
        _inputIndexJA = inputI;
    }

    public void SetInputIndexSA(int inputI)
    {
        _inputIndexSA = inputI;
    }

    public void SetInputIndexPSA(int inputI)
    {
        _inputIndexPSA = inputI;
    }

    public void SetInputIndexRightPSA(int inputIR)
    {
        _inputIndexRightPSA = inputIR;
    }

    public void SetHopIndex(int hopI)
    {
        _hopIndex = hopI;
    }
}
