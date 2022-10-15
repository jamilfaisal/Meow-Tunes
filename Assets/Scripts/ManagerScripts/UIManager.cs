using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager current;

    public GameObject winLevelUI;
    public TMP_Text winLevelScoreText;
    public GameObject lostLevelUI;
    public GameObject gameUI;

    private void Awake()
    {
        current = this;
    }

    public void WonLevelUI()
    {
        winLevelScoreText.text = ScoreManager.current.playerScore + " Fish Treats Collected!";
        gameUI.SetActive(false);
        winLevelUI.SetActive(true);
    }

    public void LostLevelUI()
    {
        gameUI.SetActive(false);
        lostLevelUI.SetActive(true);

    }
}
