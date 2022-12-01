using System.Collections;
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
    public GameObject Player;
    private PlayerMovement playerMovement;
    public Animator animator;

    private void Start()
    {
        playerLivesText.text = "x " + playerLives;
        playerMovement = Player.GetComponent<PlayerMovement>();
        animator = Player.GetComponent<Animator>();
    }

    public void LostLife()
    {
        playerMovement.SetPlayerInputEnabled(false);
        animator.Play("CatFalling", 0, 0f);
        StartCoroutine(WaitThenEnablePlayerInput());
        playerLives -= 1;
        playerLivesText.text = "x " + playerLives;
        respawnSound.Play();
        if (playerLives == 0)
        {
            StartCoroutine(LoseLevel());
        }
    }

    private IEnumerator LoseLevel()
    {
        yield return new WaitForSeconds(respawnSound.clip.length - 5f);
        gameManager.LostLevel();
    } 

    private IEnumerator WaitThenEnablePlayerInput()
    {
        yield return new WaitForSeconds(1.5f);
        playerMovement.SetPlayerInputEnabled(true);
    }
}
