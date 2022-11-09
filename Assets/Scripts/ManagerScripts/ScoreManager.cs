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

    public int playerScore;
    public int maximumPlayerScore;
    public TMP_Text scoreText;
    public GameObject oopsText;
    public GameObject niceText;
    public GameObject perfectText;
    
    private void Start()
    {
        scoreText.text = "0";
    }
    public void UpdateScore(int score)
    {
        playerScore += score;
        scoreText.text = playerScore.ToString();
    }

    public void Hit()
    {
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
