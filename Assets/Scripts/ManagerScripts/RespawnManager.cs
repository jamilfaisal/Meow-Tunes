using System.Collections;
using UnityEngine;


public class RespawnManager : MonoBehaviour
{
    public static RespawnManager current;

    private void Awake()
    {
        current = this;
    }

    private Vector3 _respawnPointLocation;
    private float _musicTime;
    private int _blinkIndex;
    private int _switchIndex;

    [SerializeField] public GameObject playerCharacter;
    private Rigidbody _playerCharacterRb;

    private void Start()
    {
        _respawnPointLocation = playerCharacter.transform.position;
        _playerCharacterRb = playerCharacter.GetComponent<Rigidbody>();
    }

    public IEnumerator RespawnPlayer(float respawnClipLength)
    {
        AdjustMusicTime();
        yield return new WaitForSeconds(respawnClipLength - 5f);
        AdjustPlayerPosition();
        Conductor.current.audioSource.Play();
        GameManager.current.playerIsDying = false;
        yield return null;
    }

    private void AdjustMusicTime()
    {
        Conductor.current.audioSource.time = _musicTime;
    }

    private void AdjustPlayerPosition()
    {
        playerCharacter.transform.position = _respawnPointLocation;
        playerCharacter.transform.rotation = new Quaternion(0,0,0,0);
        _playerCharacterRb.velocity = new Vector3(0,0,1f);
    }

    public void SetRespawnPoint(Vector3 newRespawnPoint) {
        _respawnPointLocation = newRespawnPoint;
    }

    public void SetMusicTime(float musicTime)
    {
        _musicTime = musicTime;
    }
}
