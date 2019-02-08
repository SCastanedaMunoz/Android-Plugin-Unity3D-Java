using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;



public class PluginTest : MonoBehaviour
{
#if UNITY_IOS 
    [DllImport("__Internal")]
    private static extern double IOSgetElpasedTime();
#endif

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Elapsed Time: " + GetElapsedTime());
    }

    int frameCounter = 0;

    // Update is called once per frame
    void Update()
    {
        frameCounter++;
        if (frameCounter >= 5)
        {
            Debug.Log("Tick: " + GetElapsedTime());
            frameCounter = 0;
        }
    }

    double GetElapsedTime()
    {
        if(Application.platform == RuntimePlatform.IPhonePlayer)
            return IOSgetElpasedTime();

        Debug.LogWarning("Platform is not IPhone Player");
        return 0;
    }

}

