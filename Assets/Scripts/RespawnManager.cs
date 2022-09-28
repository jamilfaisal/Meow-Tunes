using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    [SerializeField] public Vector3 respawnPoint;
    [SerializeField] public GameObject playerCharacter;
    void Start()
    {
        respawnPoint = playerCharacter.transform.position;

    }

    public void respawnPlayer() {
        Destroy(playerCharacter);
        playerCharacter = GameObject.Instantiate(Resources.Load("Player"),
             respawnPoint, Quaternion.identity) as GameObject;
    }

    public void setRespawnPoint(Vector3 newRespawnPoint) {
        respawnPoint = newRespawnPoint;
    }
}
