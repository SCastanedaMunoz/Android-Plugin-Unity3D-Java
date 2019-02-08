using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;



public class PluginTest : MonoBehaviour
{
#if UNITY_IOS 
    [DllImport("__Internal")]
    private static extern double IOSgetElapsedTime();

    private delegate void intCallBack(int result);

    [DllImport("__Internal")]
    private static extern void IOScreateNativeAlert(string[] strings, int stringCount, intCallBack callback);
#endif

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Elapsed Time: " + GetElapsedTime());
        StartCoroutine(ShowDialog(Random.Range(7f, 12f)));
    }

    IEnumerator ShowDialog(float delayTime)
    {
        Debug.Log("Will show alert after " + delayTime + " seconds");

        if (delayTime > 0)
            yield return new WaitForSeconds(delayTime);

        CreateIOSAlert(new string[] { "Title", "Message", "Default Button", "Other Button"});
    }

    double GetElapsedTime()
    {
        if(Application.platform == RuntimePlatform.IPhonePlayer)
            return IOSgetElapsedTime();

        Debug.LogWarning("Platform is not IPhone Player");
        return 0;
    }

    [AOT.MonoPInvokeCallback(typeof(intCallBack))]
    static void NativeAlertHandler(int result)
    {
        Debug.Log("Unity: clicked button at index: " + result);
    }

    public void CreateIOSAlert(string[] strings)
    {
        if(strings.Length < 3)
            Debug.LogError("Alert requires at least 3 strings!");

        if (Application.platform == RuntimePlatform.IPhonePlayer)
            IOScreateNativeAlert(strings, strings.Length, NativeAlertHandler);
        else
            Debug.LogWarning("Can only display alert on iOS");

        Debug.Log("Alert shown after: " + GetElapsedTime() + " seconds");
    }
}

