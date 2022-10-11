using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishHit : MonoBehaviour
{
    public ScoreManager ScoreManager;

    // Start is called before the first frame update
    void Start()
    {
        ScoreManager = FindObjectOfType<ScoreManager>();
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }

    private void OnTriggerEnter(Collider collider) 
    {
        if (collider.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
            ScoreManager.UpdateScore();
        }
    }
}
