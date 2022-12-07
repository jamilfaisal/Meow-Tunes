using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Switch;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.XInput;

public class UIManager : MonoBehaviour
{
    public static UIManager Current;

    public GameObject winLevelUI;
    public PlayerInput playerInput;
    public GameObject winLevelReplayPS4, winLevelReplayKeyboard, winLevelReplayXbox;
    public GameObject loseLevelReplayPS4, loseLevelReplayKeyboard, loseLevelReplayXbox;
    public TMP_Text winLevelFishScoreText;
    public TMP_Text winLevelAccuracyScoreText;
    public TMP_Text winLevelFinalScoreText;
    public GameObject lostLevelUI;
    public GameObject gameUI;
    public GameObject loadingScreen;

    // tutorial sign buttons
    public static GameObject PS4Jump, XboxJump, KeyboardJump;
    public static GameObject PS4Eat, XboxEat, KeyboardEat;
    public static GameObject PS4Stomp, XboxStomp, KeyboardStomp;
    public static GameObject PS4Jump2, XboxJump2, KeyboardJump2;

    public GameObject[] PS4Tutorial = new GameObject[4]{ PS4Jump , PS4Eat , PS4Stomp , PS4Jump2 };
    public GameObject[] XboxTutorial = new GameObject[4] { XboxJump, XboxEat, XboxStomp, XboxJump2 };
    public GameObject[] KeyboardTutorial = new GameObject[4] { KeyboardJump, KeyboardEat, KeyboardStomp, KeyboardJump2 };

    private void Awake()
    {
        Current = this;
        CheckIfPlayerUsingControllerOrKeyboard();
    }

    private void CheckIfPlayerUsingControllerOrKeyboard()
    {
        SetReplayActiveBasedOnInputMethod(CheckLastUpdatedInputMethod());
        InputUser.onChange += (_, change, _) =>
        {
            if (change is InputUserChange.ControlSchemeChanged)
            {
                SetReplayActiveBasedOnInputMethod(CheckLastUpdatedInputMethod());
            }
        };
    }

    private string CheckLastUpdatedInputMethod()
    {
        if (playerInput.currentControlScheme.ToLower().Contains("gamepad") ||
            playerInput.currentControlScheme.ToLower().Contains("joystick"))
        {
            return "gamepad";
        }

        return "keyboard";
    }

    private void SetReplayActiveBasedOnInputMethod(string lastUpdatedInputMethod)
    {
        if (lastUpdatedInputMethod == "gamepad")
        {
            var gamepad = Gamepad.current;
            if (gamepad is XInputController || gamepad is SwitchProControllerHID)
            {
                winLevelReplayKeyboard.SetActive(false);
                winLevelReplayPS4.SetActive(false);
                winLevelReplayXbox.SetActive(true);
                loseLevelReplayKeyboard.SetActive(false);
                loseLevelReplayPS4.SetActive(false);
                loseLevelReplayXbox.SetActive(true);

                // Tutorial sign xbox
                for (int i = 0; i < 4; i++)
                {
                    PS4Tutorial[i].SetActive(false);
                    XboxTutorial[i].SetActive(true);
                    KeyboardTutorial[i].SetActive(false);
                }
            }
            else
            {
                winLevelReplayKeyboard.SetActive(false);
                winLevelReplayXbox.SetActive(false);
                winLevelReplayPS4.SetActive(true);
                loseLevelReplayKeyboard.SetActive(false);
                loseLevelReplayXbox.SetActive(false);
                loseLevelReplayPS4.SetActive(true);

                // Tutorial sign PS4
                for (int i = 0; i < 4; i++)
                {
                    PS4Tutorial[i].SetActive(true);
                    XboxTutorial[i].SetActive(false);
                    KeyboardTutorial[i].SetActive(false);
                }
            }
        }
        else
        {
            winLevelReplayXbox.SetActive(false);
            winLevelReplayPS4.SetActive(false);
            winLevelReplayKeyboard.SetActive(true);
            loseLevelReplayXbox.SetActive(false);
            loseLevelReplayPS4.SetActive(false);
            loseLevelReplayKeyboard.SetActive(true);

            // Tutorial sign keyboard
            for (int i = 0; i < 4; i++)
            {
                PS4Tutorial[i].SetActive(false);
                XboxTutorial[i].SetActive(false);
                KeyboardTutorial[i].SetActive(true);
            }
        }
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
        SetReplayActiveBasedOnInputMethod(CheckLastUpdatedInputMethod());
    }

    public void LostLevelUI()
    {
        gameUI.SetActive(false);
        lostLevelUI.SetActive(true);

    }

    public void DisplayLoadingScreen()
    {
        loadingScreen.SetActive(true);
    }
}
