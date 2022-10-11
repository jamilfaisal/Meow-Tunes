using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public RespawnManager respawnManager;
    public AudioSource checkpointSound;
    
    private void OnTriggerEnter(Collider collider) 
    {
        if (collider.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
            checkpointSound.Play();
            respawnManager.setRespawnPoint(collider.gameObject.transform.position);
        }
    }
}
