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
        if (otherCollider.gameObject.CompareTag("Player") && !GameManager.Current.playerIsDying)
        {
            LifeManager.current.LostLife();
            if (LifeManager.current.playerLives != 0)
            {
                // CountdownManager.Current.SetCountdown(3f);
                StartCoroutine(Respawn());
            }
        }
    }

    private IEnumerator Respawn()
    {
        // yield return new WaitForSecondsRealtime(3f);
        yield return respawnManager.RespawnPlayer(_respawnClip.length);
    }
}
