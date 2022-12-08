using UnityEngine;

public class FishHit : MonoBehaviour
{
    private Vector3 _initLocalScale;
    private void Start()
    {
        _initLocalScale = transform.localScale;
        FishTreatManager.Current.FishUnhideEvent += UnhideFishTreat;
    }

    // private void OnTriggerEnter(Collider otherCollider) 
    // {
    //     if (otherCollider.gameObject.CompareTag("Player"))
    //     {
    //         HideFishTreat();
    //         ScoreManager.current.UpdateFishScore(1);
    //     }
    // }

    public void HideFishTreat()
    {
        transform.localScale = new Vector3(0, 0, 0);
    }

    private void UnhideFishTreat()
    {
        transform.localScale = _initLocalScale;
    }
}
