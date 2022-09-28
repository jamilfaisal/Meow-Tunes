using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public RespawnManager respawnManager;
    public Transform checkpointTransform;
    
    private void OnTriggerEnter(Collider collider) 
    {
        if (collider.gameObject.tag == "Player")
        {
            respawnManager.setRespawnPoint(checkpointTransform.position);
        }
    }
}
