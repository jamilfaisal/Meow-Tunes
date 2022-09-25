using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    private GameObject[] greenPlatforms;    
    private GameObject[] redPlatforms;
    private GameObject conductor;
    // This is the time we're given for each beat
    public float delayTime = 1.8045f;
    private GameObject[] toEnable;
    private GameObject[] toDisable;
    private GameObject[] tempEnable;
    private GameObject[] tempDisable;


    void Start()
    {
        greenPlatforms = GameObject.FindGameObjectsWithTag("Green");
        redPlatforms = GameObject.FindGameObjectsWithTag("Red");
        toEnable = greenPlatforms;
        toDisable = redPlatforms;
        StartTheCoroutine();
    }

    private IEnumerator SwitchPlatformsTimeout() {
     while(true) {
        
        tempEnable = toEnable;
        tempDisable = toDisable;


        for (int i = 0; i < toEnable.Length; i++) 
        {
            toEnable[i].SetActive(true);
        }

        for (int j = 0; j < toDisable.Length; j++) 
        {
            toDisable[j].SetActive(false);
        }

        // Switching the platforms that will be enabled on the next beat
        toEnable = tempDisable;
        toDisable = tempEnable;

        yield return new WaitForSeconds(delayTime);
     }
    }

    private void StartTheCoroutine() {
        StartCoroutine("SwitchPlatformsTimeout");
    }

}
