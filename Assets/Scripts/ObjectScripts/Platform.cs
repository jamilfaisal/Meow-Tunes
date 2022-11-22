using System;
using UnityEngine;

public abstract class Platform : MonoBehaviour
{
    protected MeshRenderer _renderer;
    protected Color _startColor;
    protected Collider _meshCollider;
    protected Material _material;

    // protected double timeInstantiated; //may be used for destroy platform if needed

    protected virtual void Start()
    {
        // timeInstantiated = Conductor.GetAudioSourceTime();
        _renderer = GetComponent<MeshRenderer>();
        _material = _renderer.materials[0];
        _startColor = _material.color;
        _meshCollider = GetComponent<MeshCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            PlayerMovement.current.SyncPlayerPosToMusicTime();
    }

    //     private void Update() {
    //     double timeSinceInstantiated = Conductor.GetAudioSourceTime() - timeInstantiated;
    //     float t = (float)(timeSinceInstantiated / (Conductor.current.noteTime * 2));
    //     if (t > 1)
    //     {
    //         Destroy(gameObject);
    //     }
    // }
}
