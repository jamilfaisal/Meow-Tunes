using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager current;

    public GameObject winLevelUI;
    public TMP_Text winLevelFishScoreText;
    public TMP_Text winLevelAccuracyScoreText;
    public TMP_Text winLevelFinalScoreText;
    public GameObject lostLevelUI;
    public GameObject gameUI;

    private void Awake()
    {
        current = this;
    }

    public void WonLevelUI()
    {
        // Text Example: Collected 87/100 Fish Treats!
        winLevelFishScoreText.text = ScoreManager.current.playerFishScore.ToString() + "/" + ScoreManager.current.maximumFishScore.ToString();
                                 
        // Text Example: Meowsic Score: 1,050
        winLevelAccuracyScoreText.text = ScoreManager.current.playerAccuracyScore.ToString("n0");

        // Text Example: Final Score: 87 x 1050 = 91,350
        winLevelFinalScoreText.text = "Final Score: " + (ScoreManager.current.playerFishScore*ScoreManager.current.playerAccuracyScore).ToString("n0");

        gameUI.SetActive(false);
        winLevelUI.SetActive(true);
    }

    public void LostLevelUI()
    {
        gameUI.SetActive(false);
        lostLevelUI.SetActive(true);

    }
}
