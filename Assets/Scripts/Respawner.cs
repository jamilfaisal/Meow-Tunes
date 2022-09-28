using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    public RespawnManager respawnManager;
    public LifeManager lifeManager;
    
    private void OnTriggerEnter(Collider collider) 
    {
        if (collider.gameObject.tag == "Player")
        {
            respawnManager.respawnPlayer();
            lifeManager.LostLife();
        } else {
            Destroy(collider.gameObject);
        }
    }
}
