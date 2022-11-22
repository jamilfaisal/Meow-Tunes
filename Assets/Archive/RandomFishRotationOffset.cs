using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFishRotationOffset : MonoBehaviour
{

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
        StartCoroutine(Delay());
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(Random.Range(0, 1f));
        animator.enabled = true;
    }
}
