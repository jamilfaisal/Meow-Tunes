using UnityEngine;

public class PlayerSyncPosition : MonoBehaviour
{
    public static PlayerSyncPosition Current;
    public float offset;

    private void Awake()
    {
        Current = this;
    }

    private Vector3 _initPos;
    public AudioSource teleportSound;

    private void Start()
    {
        _initPos = transform.position;
    }

    public Vector3 GetPlayerPosMusicTimeSyncedPosition(float yValue)
    {
        return new Vector3
        {
            x = PlayerMovement.Current.lanePositions[PlayerMovement.Current.currentLane],
            y = yValue,
            z = (float) ((PlayerMovement.Current.forwardWalkSpeed - offset) * MusicPlayer.Current.GetAudioSourceTime() + _initPos.z)
        };
    }

    public void SyncPlayerPosToMusicTime(float platformPositionY)
    {
        teleportSound.Play();
        transform.position = GetPlayerPosMusicTimeSyncedPosition(platformPositionY);
    }
}