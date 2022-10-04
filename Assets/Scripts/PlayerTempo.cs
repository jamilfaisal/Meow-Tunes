using UnityEngine;

public class PlayerTempo : MonoBehaviour
{
    public GameObject tempoFast;
    public GameObject tempoNormal;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
            Conductor.current.IncreaseTempo();
            PlatformManager.current.IncreaseTempo();
            tempoNormal.SetActive(false);
            tempoFast.SetActive(true);
        } else if (Input.GetKeyDown(KeyCode.Q)) {
            Conductor.current.DecreaseTempo();
            PlatformManager.current.DecreaseTempo();
            tempoNormal.SetActive(true);
            tempoFast.SetActive(false);
        }
    }
}
