using UnityEngine;

public class EndWall : MonoBehaviour
{
    private void OnTriggerEnter(Collider otherCollider) 
    {
        if (otherCollider.gameObject.CompareTag("Player"))
        {
            GameManager.Current.gameIsEnding = true;
        }
    }
}
