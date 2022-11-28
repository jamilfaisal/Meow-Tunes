using UnityEngine;

public abstract class Platform : MonoBehaviour
{
    private MeshRenderer _renderer;
    protected Color StartColor;
    protected Collider MeshCollider;
    protected Material Material;
    
    protected virtual void Start()
    {
        // timeInstantiated = Conductor.GetAudioSourceTime();
        _renderer = GetComponent<MeshRenderer>();
        Material = _renderer.materials[0];
        StartColor = Material.color;
        MeshCollider = GetComponent<MeshCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !GameManager.Current.playerIsDying && Time.time > 1)
            PlayerSyncPosition.Current.SyncPlayerPosToMusicTime(transform.position.y  + 1f);
    }
}
