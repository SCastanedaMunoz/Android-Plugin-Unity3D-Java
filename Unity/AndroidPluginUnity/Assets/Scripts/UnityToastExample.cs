using UnityEngine;

public class UnityToastExample : MonoBehaviour
{
    private AndroidJavaObject toastExample = null;
    private AndroidJavaObject activityContext = null;

    void Start()
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
                    activityContext.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                        toastExample.Call("showMessage", "This is a Toast message");
                    }));
                }
            }
        }
    }
}
