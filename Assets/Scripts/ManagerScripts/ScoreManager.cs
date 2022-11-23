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
    
    private void Start()
    {
        playerAccuracyScore = 0;
        playerFishScore = 0;
        accuracyScoreText.text = "x 0";
        fishScoreText.text = "x 0";
    }

    public void UpdateScore(int score)
    {
        playerFishScore += score;
        fishScoreText.text = "x " + playerFishScore;
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
        perfectText.SetActive(false);
        niceText.SetActive(false);
        oopsText.SetActive(true);
        StartCoroutine(DelayOops());
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
    
    public void SetAndUpdatePlayerAccuracyScore(int score)
    {
        playerAccuracyScore = score;
        accuracyScoreText.text = "x " + playerAccuracyScore.ToString("n0");
    }

    public int GetPlayerAccuracyScore()
    {
        return playerAccuracyScore;
    }
}
