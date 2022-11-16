using System.Collections;
using Melanchall.DryWetMidi.Interaction;
using UnityEngine;


public class RespawnManager : MonoBehaviour
{
    public static RespawnManager current;

    private void Awake()
    {
        current = this;
    }

    private Vector3 _respawnPointLocation;

    public int respawnLane;
    private float _musicTime;
    private ITimeSpan _midiTime;

    [SerializeField] public GameObject playerCharacter;

    private PlayerMovement _playerCharacterMovement;
    private Rigidbody _playerCharacterRb;

    private void Start()
    {
        _respawnPointLocation = playerCharacter.transform.position;
        _playerCharacterRb = playerCharacter.GetComponent<Rigidbody>();
        _playerCharacterMovement = playerCharacter.GetComponent<PlayerMovement>();
        _midiTime = new MetricTimeSpan(0);
    }

    public IEnumerator RespawnPlayer(float respawnClipLength)
    {
        GameManager.current.playerIsDying = true;
        AdjustMusicTime();
        AdjustMidiTime();
        yield return new WaitForSeconds(respawnClipLength - 5f);
        AdjustPlayerPosition();
        MusicPlayer.current.Resume();
        MidiManager.current.ResumePlayback();
        GameManager.current.playerIsDying = false;
        yield return null;
    }

    private void AdjustMusicTime()
    {
        MusicPlayer.current.Pause();
        MusicPlayer.current.audioSource.time = _musicTime;
    }

    private void AdjustPlayerPosition()
    {
        playerCharacter.transform.position = _respawnPointLocation;
        
        playerCharacter.transform.rotation = new Quaternion(0,0,0,0);
        _playerCharacterRb.velocity = new Vector3(0,0,1f);
        _playerCharacterMovement.current_lane = respawnLane;
    }

    private void AdjustMidiTime()
    {
        MidiManager.current.PausePlayback();
        MidiManager.current.AdjustMidiTime(_midiTime);
    }

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
        _midiTime = midiTime;
    }
}
