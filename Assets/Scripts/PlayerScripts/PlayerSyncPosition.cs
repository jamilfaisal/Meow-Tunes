using UnityEngine;

public class PlayerSyncPosition : MonoBehaviour
{
    public static PlayerSyncPosition Current;

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

    public void SyncPlayerPosToMusicTime(float platformPositionY)
    {
        var syncedPosition = new Vector3
        {
            x = PlayerMovement.current.lane_positions[PlayerMovement.current.current_lane],
            y = platformPositionY + 1f,
            z = (float) (8 * MusicPlayer.current.GetAudioSourceTime() + _initPos.z)
        };
        teleportSound.Play();
        transform.position = syncedPosition;
    }
}