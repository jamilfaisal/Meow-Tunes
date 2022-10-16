using UnityEngine;
using TMPro;

public class LifeManager : MonoBehaviour
{
    public static LifeManager current;

    private void Awake()
    {
        current = this;
    }

    public int playerLives = 5;
    public TMP_Text playerLivesText;
    public GameManager gameManager;
    public AudioSource respawnSound;

    private void Start()
    {
        playerLivesText.text = "x " + playerLives;
    }

    public void LostLife()
    {
        playerLives -= 1;
        playerLivesText.text = "x " + playerLives;
        if (playerLives == 0) {
            gameManager.LostLevel();
        }
        else
        {
            respawnSound.Play();
        }
    }
}
