using UnityEngine;

public class FishHit : MonoBehaviour
{
    private void OnTriggerEnter(Collider otherCollider) 
    {
        if (otherCollider.gameObject.CompareTag("Player"))
        {
            DestroyFishTreat();
            ScoreManager.current.UpdateFishScore(1);
        }
    }

    public void DestroyFishTreat()
    {
        // Check it is not NULL
        if (gameObject)
        {
            Destroy(gameObject);
        }
    }
}
