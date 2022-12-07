using System.Collections;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    public RespawnManager respawnManager;
    public AudioSource respawnSound;
    private AudioClip _respawnClip;
    public static Respawner Current;

    private void Awake()
    {
        Current = this;
    }

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
                Respawn();
            }
        }
    }

    public void Respawn()
    {
        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        yield return respawnManager.RespawnPlayer(_respawnClip.length);
    }
}
