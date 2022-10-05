using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndBox : MonoBehaviour
{

    public GameManager gameManager;
    
    private void OnTriggerEnter(Collider collider) 
    {
        if (collider.gameObject.tag == "Player")
        {
            gameManager.WonLevel();
        }
    }
}
