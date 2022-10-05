using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager current;

    private bool _gameHasEnded;
    public GameObject completeLevelUI;
    public GameObject lostLevelUI;
    public GameObject playerGameObject;

    public AudioSource gameOverSound;
    // -1 is slow, 0 is normal, 1 is fast
    public int audioTempo;

    private void Awake()
    {
        current = this;
    }

    private void Update() {
        if (Input.GetButton("Submit") && _gameHasEnded) {
            RestartLevel();
        }
    }
    public void WonLevel()
    {
        if (_gameHasEnded) return;
        _gameHasEnded = true;
        playerGameObject.GetComponent<PlayerMovement>().enabled = false;
        completeLevelUI.SetActive(true);
    }

    public void LostLevel()
    {
        if (_gameHasEnded) return;
        gameOverSound.Play();
        _gameHasEnded = true;
        playerGameObject.GetComponent<PlayerMovement>().enabled = false;
        lostLevelUI.SetActive(true);
    }

    private void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        _gameHasEnded = false;
    }

    public int GetAudioTempo()
    {
        return audioTempo;
    }
    
    public void IncreaseAudioTempo()
    {
        audioTempo = Math.Min(audioTempo + 1, 1); 
    }

    public void DecreaseAudioTempo()
    {
        audioTempo = Math.Max(audioTempo - 1, -1);

    }
}
