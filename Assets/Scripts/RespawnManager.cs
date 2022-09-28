using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class RespawnManager : MonoBehaviour
{
    [SerializeField] public Vector3 respawnPoint;
    [SerializeField] public GameObject playerCharacter;
    void Start()
    {
        respawnPoint = playerCharacter.transform.position;
    }

    public void respawnPlayer() {
        playerCharacter.transform.position = respawnPoint;
        playerCharacter.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
    }

    public void setRespawnPoint(Vector3 newRespawnPoint) {
        respawnPoint = newRespawnPoint;
    }
}
