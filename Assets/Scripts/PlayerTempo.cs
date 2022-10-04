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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
            ChangingTempo?.Invoke();
            Conductor.current.IncreaseTempo();
            PlatformManager.current.IncreaseTempo();
            tempoNormal.SetActive(false);
            tempoFast.SetActive(true);
        } else if (Input.GetKeyDown(KeyCode.Q)) {
            ChangingTempo?.Invoke();
            Conductor.current.DecreaseTempo();
            PlatformManager.current.DecreaseTempo();
            tempoNormal.SetActive(true);
            tempoFast.SetActive(false);
        }
    }
}
