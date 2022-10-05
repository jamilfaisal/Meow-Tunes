using System;
using UnityEngine;

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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
            ChangingTempo?.Invoke();
            Conductor.current.IncreaseTempo();
            GameManager.current.IncreaseAudioTempo();
            UpdateTempoUIImage();
        } else if (Input.GetKeyDown(KeyCode.Q)) {
            ChangingTempo?.Invoke();
            Conductor.current.DecreaseTempo();
            GameManager.current.DecreaseAudioTempo();
            UpdateTempoUIImage();
        }
    }

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
}
