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
        // StartCoroutine(Delay());
        // perfectText.SetActive(false);
    }

    public void Miss()
    {
        perfectText.SetActive(false);
        niceText.SetActive(false);
        oopsText.SetActive(true);
        // StartCoroutine(Delay());
        // oopsText.SetActive(false);
    }

    public void Inaccurate()
    {
        perfectText.SetActive(false);
        niceText.SetActive(true);
        oopsText.SetActive(false);
        // StartCoroutine(Delay());
        // niceText.SetActive(false);
    }
    
    private IEnumerator Delay(){
        yield return new WaitForSeconds(1f);
    }
    
}
