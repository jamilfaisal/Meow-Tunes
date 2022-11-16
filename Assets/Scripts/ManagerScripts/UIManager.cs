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
        winLevelFishScoreText.text = "Collected " + ScoreManager.current.playerFishScore +
                                 "/" + ScoreManager.current.maximumFishScore + " Fish Treats!";
                                 
        // Text Example: Meowsic Score: 1050
        winLevelAccuracyScoreText.text = "Meowsic Score:  " + ScoreManager.current.playerAccuracyScore;

        // Text Example: Final Score: 87 x 1050 = 91350
        winLevelFinalScoreText.text = "Final Score: " + ScoreManager.current.playerFishScore + " x " +
                                 + ScoreManager.current.playerAccuracyScore + " = " +
                                 (ScoreManager.current.playerFishScore*ScoreManager.current.playerAccuracyScore);

        gameUI.SetActive(false);
        winLevelUI.SetActive(true);
    }

    public void LostLevelUI()
    {
        gameUI.SetActive(false);
        lostLevelUI.SetActive(true);

    }
}
