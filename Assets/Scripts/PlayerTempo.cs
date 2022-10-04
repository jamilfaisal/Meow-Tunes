using UnityEngine;

public class PlayerTempo : MonoBehaviour
{
    public PlatformParent platform;
    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
            Conductor.current.IncreaseTempo();
        } else if (Input.GetKeyDown(KeyCode.Q)) {
            Conductor.current.DecreaseTempo();
        }
    }
}
