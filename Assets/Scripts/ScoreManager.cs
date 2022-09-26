using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int playerScore = 0;
    public TMP_Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        //scoreText = GetComponent<TextMeshProUGUI>();
        scoreText.text = "Score: 0";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore()
    {
        playerScore += 1;
        scoreText.text = "Score: " + playerScore;
    }
}
