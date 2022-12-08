using System.Collections;
using UnityEngine;
using TMPro;


public class ScoreManager : MonoBehaviour
{
    public static ScoreManager current;

    private void Awake()
    {
        current = this;
    }

    public int playerAccuracyScore;
    public int playerFishScore;
    public int maximumFishScore;
    public TMP_Text accuracyScoreText;
    public TMP_Text fishScoreText;
    public GameObject oopsText;
    public GameObject niceText;
    public GameObject perfectText;
    public AudioSource oopsSound;
    public GameObject cinemachineVirtualCam;
    private CameraShake cameraShake;
    public AudioSource respawnSound;
    public GameManager gameManager;

    private void Start()
    {
        playerAccuracyScore = 0;
        playerFishScore = 0;
        accuracyScoreText.text = "x 0";
        fishScoreText.text = "x 0";
        cameraShake = cinemachineVirtualCam.GetComponent<CameraShake>();
    }

    public int GetPlayerFishScore()
    {
        return playerFishScore;
    }

    public int GetPlayerAccuracyScore()
    {
        return playerAccuracyScore;
    }

    public void UpdateFishScore(int score)
    {
        playerFishScore += score;
        fishScoreText.text = "x " + playerFishScore;
    }

    public void SetAndUpdateFishScore(int score)
    {
        playerFishScore = score;
        fishScoreText.text = "x " + playerFishScore;
    }
    
    public void SetAndUpdatePlayerAccuracyScore(int score)
    {
        playerAccuracyScore = score;
        accuracyScoreText.text = "x " + playerAccuracyScore.ToString("n0");
    }

    public void Hit()
    {
        playerAccuracyScore += 100;
        accuracyScoreText.text = "x " + playerAccuracyScore.ToString("n0");
        perfectText.SetActive(true);
        niceText.SetActive(false);
        oopsText.SetActive(false);
        StartCoroutine(DelayPerfect());
    }

    public void Miss()
    {
        playerAccuracyScore -= 200;
        accuracyScoreText.text = "x " + playerAccuracyScore.ToString("n0");
        oopsSound.Play();
        cameraShake.Shake(0.5f);
        perfectText.SetActive(false);
        niceText.SetActive(false);
        oopsText.SetActive(true);
        StartCoroutine(DelayOops());
        if (playerAccuracyScore <= -1000)
        {
            LifeManager.current.LostLife();
            if (LifeManager.current.playerLives != 0)
            {
                Respawner.Current.Respawn();
            }
        }
    }

    public void Inaccurate()
    {
        playerAccuracyScore += 50;
        accuracyScoreText.text = "x " + playerAccuracyScore.ToString("n0");
        perfectText.SetActive(false);
        niceText.SetActive(true);
        oopsText.SetActive(false);
        StartCoroutine(DelayNice());
    }
    
    private IEnumerator DelayPerfect(){
        yield return new WaitForSeconds(1f);
        perfectText.SetActive(false);
    }

    private IEnumerator DelayOops(){
        yield return new WaitForSeconds(1f);
        oopsText.SetActive(false);
    }

    private IEnumerator DelayNice(){
        yield return new WaitForSeconds(1f);
        niceText.SetActive(false);
    }
}
