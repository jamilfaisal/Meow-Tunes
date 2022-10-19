using UnityEngine;

public class YarnballHit : MonoBehaviour
{
    private void OnTriggerEnter(Collider otherCollider) 
    {
        if (otherCollider.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            ScoreManager.current.UpdateScore(5);
        }
    }
}
