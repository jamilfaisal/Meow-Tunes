using System.Collections;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    public RespawnManager respawnManager;
    public AudioSource respawnSound;
    private AudioClip _respawnClip;

    private void Start()
    {
        _respawnClip = respawnSound.clip;
    }

    private void OnTriggerEnter(Collider otherCollider) 
    {
        if (otherCollider.gameObject.CompareTag("Player") && !GameManager.current.playerIsDying)
        {
            LifeManager.current.LostLife();
            if (LifeManager.current.playerLives != 0)
            {
                StartCoroutine(Respawn());
            }
        }
    }

    private IEnumerator Respawn()
    {
        yield return respawnManager.RespawnPlayer(_respawnClip.length);
    }
}
