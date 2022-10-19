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

    private void Start()
    {
        //scoreText = GetComponent<TextMeshProUGUI>();
        scoreText.text = "Score: 0";
    }
    public void UpdateScore(int score)
    {
        playerScore += score;
        scoreText.text = "Score: " + playerScore;
    }
}
