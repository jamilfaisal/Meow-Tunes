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

    public ScoreManager scoreManager;
    private int _playerFishScore;
    private int _playerAccuracyScore;

    private int _inputIndexSBA; // Single Button Action
    private int _inputIndexPSA; // Player Side Action
    private int _inputIndexRightPSA; // PSA Input index right

    private int _hopIndex;

    private void Start()
    {
        _respawnPointLocation = playerCharacter.transform.position;
        respawnLane = 2;
        _playerCharacterRb = playerCharacter.GetComponent<Rigidbody>();
        _playerCharacterMovement = playerCharacter.GetComponent<PlayerMovement>();
        _playerFishScore = scoreManager.GetPlayerFishScore();
        _playerAccuracyScore = scoreManager.GetPlayerAccuracyScore();
        _inputIndexSBA = SingleButtonAction.Current.GetInputIndex();
        _inputIndexPSA = PlayerSideAction.Current.GetInputIndex();
        _inputIndexRightPSA = PlayerSideAction.Current.GetInputIndexRight();
        _hopIndex = PlayerHopManager.Current.GetHopIndex();
    }

    public IEnumerator RespawnPlayer(float respawnClipLength)
    {
        GameManager.Current.playerIsDying = true;

        // Reset Fish Treats on the lanes
        MusicPlayer.Current.ResetAllFishTreats();
        
        // Reset values
        AdjustMusicTime();
        //AdjustMidiTime();
        yield return new WaitForSeconds(respawnClipLength - 5f);
        AdjustPlayerPosition();
        scoreManager.SetAndUpdateFishScore(_playerFishScore);
        scoreManager.SetAndUpdatePlayerAccuracyScore(_playerAccuracyScore);
        SingleButtonAction.Current.SetInputIndex(_inputIndexSBA);
        PlayerSideAction.Current.SetInputIndex(_inputIndexPSA);
        PlayerSideAction.Current.SetInputIndexRight(_inputIndexRightPSA);
        PlayerHopManager.Current.SetHopIndex(_hopIndex);

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

    public void SetInputIndexSBA(int inputI)
    {
        _inputIndexSBA = inputI;
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
