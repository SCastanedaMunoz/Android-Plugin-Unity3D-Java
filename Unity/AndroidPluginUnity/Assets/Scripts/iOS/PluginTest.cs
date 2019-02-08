using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;



public class PluginTest : MonoBehaviour
{
#if UNITY_IOS 
    [DllImport("__Internal")]
    private static extern double IOSgetElapsedTime();

    private delegate void intCallBack(int result);

    [DllImport("__Internal")]
    private static extern void IOScreateNativeAlert(string[] strings, int stringCount, intCallBack callback);

    [DllImport("__Internal")]
    private static extern void IOSshareScreenImage(byte[] imagePNG, long imageLen, string caption, intCallBack callback);
#endif

    public Button shareButton;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Elapsed Time: " + GetElapsedTime());
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

    public void ShareScreenTapped()
    {
        if (shareButton != null)
            shareButton.gameObject.SetActive(false);

        ShareScreenShot(Application.productName + " screenshot", (int result) =>
        {
            Debug.Log("Share completed with: " + result);
            CreateIOSAlert(new string[] { "Share Complete", "Share completed with: " + result, "OK" });

            if (shareButton != null)
                shareButton.gameObject.SetActive(true);
        });
    }

    static System.Action<int> ShareCompleteAction;

    static bool isSharingScreenShot;

    [AOT.MonoPInvokeCallback(typeof(intCallBack))]
    static void ShareCallBack(int result)
    {
        Debug.Log("Unity: share completed with: " + result);
        if (ShareCompleteAction != null)
            ShareCompleteAction(result);
        isSharingScreenShot = false;
    }

    public void ShareScreenShot(string caption, System.Action<int> shareComplete)
    {
        if (isSharingScreenShot)
        {
            Debug.LogError("Already sharing screenshot - aborting");
            return;
        }

        isSharingScreenShot = true;
        ShareCompleteAction = shareComplete;
        StartCoroutine(WaitForEndOfFrame(caption));
    }

    IEnumerator WaitForEndOfFrame(string caption)
    {
        yield return new WaitForEndOfFrame();
        Texture2D image = ScreenCapture.CaptureScreenshotAsTexture();
        Debug.Log("Image size: " + image.width + " x " + image.height);

        byte[] imagePNG = image.EncodeToPNG();

        Debug.Log("PNG size: " + imagePNG.Length);

        if (Application.platform == RuntimePlatform.IPhonePlayer)
            IOSshareScreenImage(imagePNG, imagePNG.Length, caption, ShareCallBack);

        Object.Destroy(image);
    }
}

