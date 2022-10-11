using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    public RespawnManager respawnManager;
    public LifeManager lifeManager;
    public AudioSource respawnSound;
    private AudioClip _respawnClip;

    private void Start()
    {
        _respawnClip = respawnSound.clip;
    }

    private void OnTriggerEnter(Collider collider) 
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            lifeManager.LostLife();
            Invoke(nameof(Respawn), _respawnClip.length - 5f);
        }
    }

    private void Respawn()
    {
        respawnManager.respawnPlayer();
    }
}
