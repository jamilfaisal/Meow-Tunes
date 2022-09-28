using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LifeManager : MonoBehaviour
{
    public int playerLives = 5;
    public TMP_Text playerLivesText;
    public GameManager gameManager;
    public AudioSource respawnSound;

    void Start()
    {
        playerLivesText.text = "x " + playerLives;
    }

    public void LostLife()
    {
        playerLives -= 1;
        playerLivesText.text = "x " + playerLives;
        if (playerLives == 0) {
            gameManager.lostLevel();
        }
        else
        {
            respawnSound.Play();
        }
    }
}
