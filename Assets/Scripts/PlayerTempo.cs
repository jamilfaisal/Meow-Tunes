using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTempo : MonoBehaviour
{
    public static PlayerTempo current;
    public event Action ChangingTempo;
    private void Awake()
    {
        current = this;
    }

    public GameObject tempoFast;
    public GameObject tempoNormal;
    public GameObject tempoSlow;

    private void UpdateTempoUIImage()
    {
        switch (GameManager.current.audioTempo)
        {
            case -1:
                tempoNormal.SetActive(false);
                tempoFast.SetActive(false);
                tempoSlow.SetActive(true);
                break;
            case 0:
                tempoFast.SetActive(false);
                tempoSlow.SetActive(false);
                tempoNormal.SetActive(true);
                break;
            case 1:
                tempoSlow.SetActive(false);
                tempoNormal.SetActive(false);
                tempoFast.SetActive(true);
                break;
        }
    }
    
    public void SpeedUp(InputAction.CallbackContext context)
    {
        
            if (context.performed)
            {
                if (GameManager.current.IsGamePaused()) return;
                ChangingTempo?.Invoke();
                Conductor.current.IncreaseTempo();
                GameManager.current.IncreaseAudioTempo();
                UpdateTempoUIImage();
            }
        
    }

    public void SlowDown(InputAction.CallbackContext context)
    {
        if (context.performed)
            {
                if (GameManager.current.IsGamePaused()) return;
                ChangingTempo?.Invoke();
                Conductor.current.DecreaseTempo();
                GameManager.current.DecreaseAudioTempo();
                UpdateTempoUIImage();

            }
    }
}
