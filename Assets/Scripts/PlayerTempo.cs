using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTempo : MonoBehaviour
{
    public static PlayerTempo current;
    public event Action ChangingTempo;
    private void Awake()
    {
        current = this;
    }

    private readonly int _cooldownTime = 5;
    private bool _tempoOnCooldown;
    public Image tempoFast;
    public Image tempoNormal;
    public Image tempoSlow;
    private void Update()
    {
        if (GameManager.current.IsGamePaused() || _tempoOnCooldown) return;
        if (Input.GetKeyDown(KeyCode.E))
        {
            TriggerTempoCooldown();
            ChangingTempo?.Invoke();
            Conductor.current.IncreaseTempo();
            GameManager.current.IncreaseAudioTempo();
            UpdateTempoUIImage();
        } else if (Input.GetKeyDown(KeyCode.Q))
        {
            TriggerTempoCooldown();
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
                tempoNormal.gameObject.SetActive(false);
                tempoFast.gameObject.SetActive(false);
                tempoSlow.gameObject.SetActive(true);
                UpdateTempoOpacity(tempoSlow);
                break;
            case 0:
                tempoFast.gameObject.SetActive(false);
                tempoSlow.gameObject.SetActive(false);
                tempoNormal.gameObject.SetActive(true);
                UpdateTempoOpacity(tempoNormal);
                break;
            case 1:
                tempoSlow.gameObject.SetActive(false);
                tempoNormal.gameObject.SetActive(false);
                tempoFast.gameObject.SetActive(true);
                UpdateTempoOpacity(tempoFast);
                break;
        }
    }

    private void TriggerTempoCooldown()
    {
        _tempoOnCooldown = true;
        StartCoroutine(DecreaseCooldownTime());
    }
    private IEnumerator DecreaseCooldownTime()
    {
        var i = 0;
        while (i < _cooldownTime)
        {
            if (GameManager.current.IsGamePaused()) yield return new WaitForSeconds(1f);
            yield return new WaitForSeconds(1f);
            i++;
        }
        _tempoOnCooldown = false;
        UpdateTempoUIImage();
    }

    private void UpdateTempoOpacity(Graphic tempoImage)
    {
        ChangeTempoOpacity(tempoImage, _tempoOnCooldown ? 0.5f : 1f);
    }
    private static void ChangeTempoOpacity(Graphic tempoImage, float opacity)
    {
        var temp = tempoImage.color;
        temp.a = opacity;
        tempoImage.color = temp;
    }
}
