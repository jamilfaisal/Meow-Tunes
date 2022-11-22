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

    public Vector3 GetPlayerPosMusicTimeSyncedPosition(float yValue)
    {
        return new Vector3
        {
            x = PlayerMovement.current.lane_positions[PlayerMovement.current.current_lane],
            y = yValue,
            z = (float) (8 * MusicPlayer.current.GetAudioSourceTime() + _initPos.z)
        };
    }

    public void SyncPlayerPosToMusicTime(float platformPositionY)
    {
        // For some reason, the sound plays when the scene starts. Hacky way to avoid this
        if (Time.time > 5)
        {
            teleportSound.Play();
        }
        transform.position = GetPlayerPosMusicTimeSyncedPosition(platformPositionY + 1f);
    }
}