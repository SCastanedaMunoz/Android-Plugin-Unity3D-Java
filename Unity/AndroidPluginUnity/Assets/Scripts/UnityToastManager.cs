using UnityEngine;

public class UnityToastManager : MonoBehaviour
{
#if UNITY_ANDROID 
    private AndroidJavaObject toastExample = null;
    private AndroidJavaObject activityContext = null;
    public bool Initialized { get; private set; }

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (toastExample == null)
        {
            using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
            }

            using (AndroidJavaClass pluginClass = new AndroidJavaClass("net.scastanedamunoz.androidlib.ToastExample"))
            {
                if (pluginClass != null)
                {
                    toastExample = pluginClass.CallStatic<AndroidJavaObject>("instance");
                    toastExample.Call("setContext", activityContext);
                    Initialized = true;
                }
            }
        }
    }

    public void CallMessage(string Message)
    {
        if (!Initialized) { Debug.LogWarning("Toast Manager has't initialized properly"); return; }
        if (toastExample == null) { Debug.LogWarning("Could Not Find Toast Example Android Java Class"); return; }
        if (activityContext == null) { Debug.LogWarning("Could Not Find Activity Context"); return; }

        activityContext.Call("runOnUiThread", new AndroidJavaRunnable(() => {
            toastExample.Call("showMessage", Message);
        }));
    }

#endif
}
