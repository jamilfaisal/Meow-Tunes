using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformParent : MonoBehaviour
{
    // GameObject[] red_platforms, green_platforms;
    private void Start() {
        // red_platforms = GameObject.FindGameObjectsWithTag("Red");
        // green_platforms = GameObject.FindGameObjectsWithTag("Green");
    }

    public void Appear_Disappear()
    {
        // foreach (var platform in red_platforms){
        //     Platform platform_component = platform.GetComponent<Platform>();

        //     if (platform_component != null){
        //         platform_component.Appear_Disappear_single();
        //     }
        // }

        // foreach (var platform in green_platforms){
        //     Platform platform_component = platform.GetComponent<Platform>();

        //     if (platform_component != null){
        //         platform_component.Appear_Disappear_single();
        //     }
        // }
        BroadcastMessage("Appear_Disappear_single");
    }

    public void Blink()
    {
        // foreach (var platform in red_platforms){
        //     //Cannot GetComponenet from here (not the main thread)
        //     // How about try boardcasting
        //     Platform platform_component = platform.GetComponent<Platform>();

        //     if (platform_component != null){
        //         platform_component.Blink_single();
        //     }
        // }

        // foreach (var platform in green_platforms){
        //     Platform platform_component = platform.GetComponent<Platform>();

        //     if (platform_component != null){
        //         platform_component.Blink_single();
        //     }
        // }
        BroadcastMessage("Blink_single");
    }
    // protected static int tempo = 0;
    // protected Dictionary<int, float> tempoTime = new Dictionary<int, float>()
    //     {
    //         { 0, 3.609f },
    //         { 1, 1.8045f }
    //     };
    // // The time we should wait before platform starts blinking
    // protected Dictionary<int, float> blinkDelay = new Dictionary<int, float>()
    //     {
    //         { 0, 2.255f },
    //         { 1, 0.255f }
    //     };
    // protected float _blinkTime = 0.4511f;
    // public void setTempo(int newTempo) {
    //     tempo = newTempo;
    // }


}
